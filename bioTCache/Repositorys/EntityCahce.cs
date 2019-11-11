using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;


namespace bioTCache
{
    public class CacheEventArgs : EventArgs
    {
        public string Key { get; set; }
    }

    public class EntityCache<T> where T : IEntity
    {
        private readonly IRepository      r_Repository;
        private readonly ISerializer      r_Serializer;
        private readonly ReaderWriterLock r_ReadWritelock = new ReaderWriterLock();
        private Dictionary<string, T>     m_CachedData = new Dictionary<string, T>();

        public event EventHandler Event_AddCompleted;
        public event EventHandler Event_RemoveCompleted;
        public event EventHandler Event_UpdateCompleted;
        public event EventHandler Event_GetCompleted;
        public event EventHandler Event_GetAllCompleted;

        public uint HourDiffrenceForUpdatingCache { get; set; }

        public DateTime LastEagerUpdate { get; private set; }

        public EntityCache(IRepository i_Repository, ISerializer i_Serializer, eCacheLoadingMode i_LoadingMode = eCacheLoadingMode.Eager)
        {
            r_Repository = i_Repository;
            r_Serializer = i_Serializer;
            m_CachedData = new Dictionary<string, T>();
            HourDiffrenceForUpdatingCache = 1;
            LastEagerUpdate = DateTime.MinValue;
            if (i_LoadingMode == eCacheLoadingMode.Eager)
            {
                populateCache();
            }
        }

        public void Add(T i_Obj)
        {
            r_ReadWritelock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string json = r_Serializer.Serialize(i_Obj);
                string key = i_Obj.Id.ToString();
                var response = r_Repository.Add(key, json);
                if(!response.Success)
                {
                    throw new Exception(response.ErrorMessage);
                }
                m_CachedData[key] = deepCopy(i_Obj);
                Event_AddCompleted?.Invoke(this, new CacheEventArgs() { Key = key });
            }
            finally
            {
                r_ReadWritelock.ReleaseWriterLock();
            }

        }

        public T Get(int i_Id)
        {
            T result;
            string key = i_Id.ToString();
            r_ReadWritelock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                bool wasCached = m_CachedData.TryGetValue(key, out result);
                if (!wasCached)
                {
                    var response = r_Repository.Get(i_Id);
                    string obj = response.Content;
                    result = r_Serializer.Deserialize<T>(obj);
                    if (!response.Success || result ==  null)
                    {
                        throw new Exception(response.ErrorMessage);
                    }
                   
                    m_CachedData.Add(result.Id.ToString(), deepCopy(result));
                }
            }
            finally
            {
                r_ReadWritelock.ReleaseReaderLock();
            }

            Event_GetCompleted?.Invoke(this, new CacheEventArgs() { Key = key });
            return deepCopy(result);
        }

        public void Update(T i_Obj)
        {
            r_ReadWritelock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string json = r_Serializer.Serialize(i_Obj);
                string key = i_Obj.Id.ToString();
                var response = r_Repository.Update(key, json);
                if (!response.Success)
                {
                    throw new Exception(response.ErrorMessage);
                }

                m_CachedData[key] = deepCopy(i_Obj);
                Event_UpdateCompleted?.Invoke(this, new CacheEventArgs() { Key = key });

            }
            finally
            {
                r_ReadWritelock.ReleaseWriterLock();
            }
        }

        public void Remove(int i_Id)
        {
            r_ReadWritelock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                string key = i_Id.ToString();
                var response = r_Repository.Remove(i_Id);
                if (!response.Success)
                {
                    throw new Exception(response.ErrorMessage);
                }
                m_CachedData.Remove(key);
                Event_RemoveCompleted?.Invoke(this, new CacheEventArgs() { Key = key });
            }
            finally
            {
                r_ReadWritelock.ReleaseWriterLock();
            }
        }

        public ImmutableDictionary<string,T> GetAll()
        {
            if (DateTime.Now.Date != LastEagerUpdate.Date || DateTime.Now.Hour - LastEagerUpdate.Hour >= HourDiffrenceForUpdatingCache)
            {
                populateCache();
            }

            r_ReadWritelock.AcquireReaderLock(Timeout.Infinite);
            var res = m_CachedData.ToImmutableDictionary();
            r_ReadWritelock.ReleaseReaderLock();
            Event_GetAllCompleted?.Invoke(this, new EventArgs());

            return res ;
        }

        public void ForceCacheUpdate()
        {
            populateCache();
            Event_GetAllCompleted?.Invoke(this, new EventArgs());
        }

        private void populateCache()
        {
            r_ReadWritelock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                var response = r_Repository.GetAll();
                if (!response.Success || response.Content == null)
                {
                    throw new Exception(response.ErrorMessage);
                }

                m_CachedData = r_Serializer.Deserialize<Dictionary<string, T>>(response.Content);
                if (m_CachedData == null) m_CachedData = new Dictionary<string, T>();
                LastEagerUpdate = DateTime.Now;
            }
            finally
            {
                r_ReadWritelock.ReleaseWriterLock();
            }
        }

        private T deepCopy(T i_Data)
        {
            return r_Serializer.Deserialize<T>(r_Serializer.Serialize(i_Data));
        }

        public void Clear()
        {
            LastEagerUpdate = DateTime.MinValue;
            HourDiffrenceForUpdatingCache = 1;
            m_CachedData.Clear();
        }

        public T ForceGet(int i_Id)
        {
            T result;
            string key = i_Id.ToString();

            r_ReadWritelock.AcquireReaderLock(Timeout.Infinite);
            bool wasCached = m_CachedData.TryGetValue(key, out result);
            try
            {
                var response = r_Repository.Get(i_Id);
                string obj = response.Content;
                if (!response.Success || response.Content == null)
                {
                    throw new Exception(response.ErrorMessage);
                }

                result = r_Serializer.Deserialize<T>(obj);
                m_CachedData[result.Id.ToString()]= result;
            }
            finally
            {
                r_ReadWritelock.ReleaseReaderLock();
            }

            if(wasCached)
            {
                Event_UpdateCompleted?.Invoke(this, new CacheEventArgs() { Key = key });
            }
            else
            {
                Event_GetCompleted?.Invoke(this, new CacheEventArgs() { Key = key });
            }

            return deepCopy(result);
        }

        public ImmutableDictionary<string, T> GetCurrentCache()
        {
            r_ReadWritelock.AcquireReaderLock(Timeout.Infinite);
            var res = m_CachedData.ToImmutableDictionary();
            r_ReadWritelock.ReleaseReaderLock();

            return res;
        }

    }
}
