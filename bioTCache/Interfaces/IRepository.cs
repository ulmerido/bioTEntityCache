using bioTCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache
{
    public interface IRepository
    {
        IResponse Get(int i_Id);
        IResponse GetAll();
        IResponse Add(string i_Key, string i_Value);
        IResponse Update(string i_Key, string i_Value);
        IResponse Remove(int i_Id);
    }
}
