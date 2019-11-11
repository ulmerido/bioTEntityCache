using System.Text.Json;

namespace bioTCache
{
  
    public class DbJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string i_Json)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };
            
            return JsonSerializer.Deserialize<T>(i_Json, options);
        }

        public string Serialize<T>(T i_Obj)
        {
            return JsonSerializer.Serialize<T>(i_Obj);
        }

        public string SerializePrettyPrint<T>(T i_Obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize<T>(i_Obj, options);
        }
    }
}
