using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache
{
    public interface ISerializer
    {
        string Serialize<T>(T i_Obj);
        T Deserialize<T>(string i_Obj);
    }
}
