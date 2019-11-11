using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace bioTCache.Firebase
{
    public class FirebaseRequest
    {
        private const string k_JsonSuffix = ".json";
        private const string k_UserAgent   = "firebase-net/1.0";

        public HttpMethod Method { get; set; }

        public string Json { get; set; }

        public string Uri { get; set; }

        public FirebaseRequest(HttpMethod i_Method, string i_Uri, string i_JsonString = null)
        {
            Method = i_Method;
            Json = i_JsonString;
            if (i_Uri.Replace("/", string.Empty).EndsWith("firebaseio.com"))
            {
                this.Uri = i_Uri + '/' + k_JsonSuffix;
            }
            else
            {
                this.Uri = i_Uri + k_JsonSuffix;
            }
        }

        public FirebaseResponse Execute()
        {
            Uri    uri = new Uri(Uri);
            var    response = send(Method, uri, Json);
            response.Wait();
            var result = response.Result;
            var firebaseResponse = new FirebaseResponse()
            {
                HttpResponse = result,
                ErrorMessage = result.StatusCode.ToString() + " : " + result.ReasonPhrase,
                Success = response.Result.IsSuccessStatusCode
            };

            if (Method.Equals(HttpMethod.Get))
            {
                var content = result.Content.ReadAsStringAsync();
                content.Wait();
                firebaseResponse.Content = content.Result;
            }

            return firebaseResponse;
        }

        private bool validateURI(string url)
        {
            Uri locurl;
            if (System.Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out locurl))
            {
                if (!(locurl.IsAbsoluteUri &&(locurl.Scheme == "http" || locurl.Scheme == "https")) || !locurl.IsAbsoluteUri)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private Task<HttpResponseMessage> send(HttpMethod method, Uri uri, string json = null)
        {
            var client = new HttpClient();
            var msg = new HttpRequestMessage(method, uri);
            msg.Headers.Add("user-agent", k_UserAgent);
            if (json != null)
            {
                msg.Content = new StringContent(json, UnicodeEncoding.UTF8,"application/json");
            }

            return client.SendAsync(msg);
        }

    }
}
