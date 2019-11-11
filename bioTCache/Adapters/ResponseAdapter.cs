using bioTCache.Firebase;
using bioTCache.Interfaces;

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
