using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache.Interfaces
{
    public interface IResponse
    {
       bool Success { get; set; }

       string Content { get; set; }

       string ErrorMessage { get; set; }
    }
}
