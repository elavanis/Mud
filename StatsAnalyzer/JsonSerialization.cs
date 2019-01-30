using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer
{
    public class JsonSerialization
    {
        private JsonSerializerSettings _settings;
        private JsonSerializerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    JsonSerializerSettings temp = new JsonSerializerSettings();
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