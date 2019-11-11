using System;
using System.Collections.Generic;
namespace bioTCache.Interfaces
{
    public interface IResponse
    {
       bool Success { get; set; }

       string Content { get; set; }

       string ErrorMessage { get; set; }
    }
}
