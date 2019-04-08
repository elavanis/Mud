using Newtonsoft.Json;
using Objects.Global.Serialization.Interface;

namespace Objects.Global.Serialization
{
    public class JsonSerialization : ISerialization
    {
        private JsonSerializerSettings _settings;
        private JsonSerializerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    JsonSerializerSettings temp = new JsonSerializerSettings();
                    temp.TypeNameHandling = TypeNameHandling.Objects;
                    _settings = temp;
                }
                return _settings;
            }
        }



        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }

        public T Deserialize<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject, Settings);
        }
    }
}
