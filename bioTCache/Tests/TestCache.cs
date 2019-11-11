using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache.Tests
{
    class TestCache<T> where T : IEntity
    {
        private EntityCache<T> m_Cache;
        private T              m_Dummy;

        public TestCache(EntityCache<T> cache, T i_Dummy)
        {
            m_Cache = cache;
            m_Dummy = i_Dummy;

        }

        public bool TestGetAll()
        {
            try
            {
                m_Cache.GetAll();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TestAddGet()
        {
            try
            {
                bool res;
                m_Cache.Add(m_Dummy);
                m_Cache.Clear();
                res = m_Cache.ForceGet(m_Dummy.Id).Id == m_Dummy.Id; // get from Repository
                return res && m_Cache.Get(m_Dummy.Id).Id == m_Dummy.Id; // get form Cache
            }
            catch
            {
                return false;
            }
        }

        public bool TestRemove()
        {
            bool res = true;
            m_Cache.Clear();
         
            try // try remove
            {
                m_Cache.Remove(m_Dummy.Id);
            }
            catch 
            {
                try
                {
                    m_Cache.Add(m_Dummy);
                    res = m_Cache.Get(m_Dummy.Id).Id != m_Dummy.Id;
                    try
                    {
                        m_Cache.Remove(m_Dummy.Id);
                    }
                    catch 
                    {
                        res = false;
                    }

                }
                catch 
                {
                    res = false; //failed at adding or getting
                }

            }

            try // remove was ok; veritfy remove
            {
                m_Cache.Get(m_Dummy.Id);
                res = res && false; // should fail getting after we removed
            }
            catch 
            {
                res = true && res; // removed ok
            }

            return res;
        }

        public bool RunTests()
        {
            bool testAddGet = TestAddGet();
            bool testRemove = TestRemove();
            bool testGetAll = TestGetAll();

            printTestResult("Test<AddGet> ", testAddGet);
            printTestResult("Test<Remove> ", testRemove);
            printTestResult("Test<GetAll> ", testGetAll);

            return testAddGet && testRemove;
        }

        private void printTestResult(string i_TestName, bool i_Result)
        {
            if (i_Result)
            {
                Console.Write(i_TestName);
                printColor("Good", ConsoleColor.Green);
            }
            else
            {
                Console.Write(i_TestName);
                printColor("BAD", ConsoleColor.Red);
            }

        }

        private void printColor(string i_Text, ConsoleColor i_Color)
        {
            Console.ForegroundColor = i_Color;
            Console.WriteLine(i_Text);
            Console.ForegroundColor = ConsoleColor.White;
        }
      

    }
}
