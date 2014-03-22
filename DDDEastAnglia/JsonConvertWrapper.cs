using Newtonsoft.Json;

namespace DDDEastAnglia
{
    public class JsonConvertWrapper
    {
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}