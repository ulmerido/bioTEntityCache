using bioTCache.Firebase;
using bioTCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache.Adapters
{
    class FirebaseIResponseAdapter : IResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }
        public string ErrorMessage { get; set; }

        public FirebaseIResponseAdapter(FirebaseResponse i_Adptee) 
        {
            Success = i_Adptee.Success;
            Content = i_Adptee.Content;
            ErrorMessage = i_Adptee.ErrorMessage;
        }
    }
}
