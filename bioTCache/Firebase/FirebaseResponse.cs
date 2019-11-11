using System.Net.Http;

namespace bioTCache.Firebase
{
    public class FirebaseResponse
    {
        public bool                Success { get; set; }

        public string              Content { get; set; }

        public string              ErrorMessage { get; set; }

        public HttpResponseMessage HttpResponse { get; set; }

        public FirebaseResponse()
        {
        }

      
        public FirebaseResponse(bool success, string errorMessage, HttpResponseMessage httpResponse = null, string jsonContent = null)
        {
            Success = success;
            Content = jsonContent;
            ErrorMessage = errorMessage;
            HttpResponse = httpResponse;
        }

   
    }
}

