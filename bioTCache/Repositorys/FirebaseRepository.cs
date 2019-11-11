using bioTCache.Adapters;
using bioTCache.Firebase;
using bioTCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache.Repositorys
{
    public class FirebaseRepository : IRepository
    {
        private FirebaseDB m_Fb;

        public FirebaseRepository(FirebaseDB i_Database)
        {
            m_Fb = i_Database;
        }

        public IResponse Add(string i_Key, string i_Value)
        {
            var path = m_Fb.Node(i_Key);
            var response = path.Put(i_Value);
            return  new FirebaseIResponseAdapter(response);
        }
        public IResponse Get(int i_Id)
        {
            string key = i_Id.ToString();
            FirebaseDB path = m_Fb.Node(key);
            return new FirebaseIResponseAdapter(path.Get());
        }
        public IResponse GetAll()
        {
            return new FirebaseIResponseAdapter(m_Fb.Get());
        }
        public IResponse Remove(int i_Id)
        {
            string key = i_Id.ToString();
            FirebaseDB path = m_Fb.Node(key);
            return new FirebaseIResponseAdapter(path.Delete());
        }
        public IResponse Update(string i_Key, string i_Value)
        {
            var path = m_Fb.Node(i_Key);
            return new FirebaseIResponseAdapter(path.Put(i_Value));
        }
    }
}
