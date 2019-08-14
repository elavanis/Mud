//using Objects.Global.Serialization.Interface;
//using Objects.Item.Items;
//using Objects.Magic;
//using Objects.Material;
//using Objects.Personality.Interface;
//using Objects.Personality;
//using Objects.Race;
//using Objects.Room;
//using Objects.Zone.Interface;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using XSerializer;

//namespace Objects.Global.Serialization
//{
//    public class XmlSerialization : ISerialization
//    {
//        ConcurrentDictionary<Type, IXSerializer> xmlSerializerCache { get; } = new ConcurrentDictionary<Type, IXSerializer>();

//        private IXSerializer GetSerializer(Type type)
//        {
//            IXSerializer xmlSerializer;

//            xmlSerializerCache.TryGetValue(type, out xmlSerializer);
//            if (xmlSerializer == null)
//            {
//                lock (xmlSerializerCache)
//                {
//                    xmlSerializerCache.TryGetValue(type, out xmlSerializer);
//                    if (xmlSerializer == null)
//                    {
//                        xmlSerializer = XmlSerializer.Create(type, (ExtraObjectTypes));
//                        xmlSerializerCache.TryAdd(type, xmlSerializer);
//                    }
//                }
//            }

//            return xmlSerializer;
//        }

//        private Type[] extraObjectTypes = null;
//        private Type[] ExtraObjectTypes
//        {
//            get
//            {
//                if (extraObjectTypes == null)
//                {
//                    Assembly assembly = Assembly.GetAssembly(this.GetType());

//                    extraObjectTypes = assembly.GetTypes().Where(x => x.IsClass
//                                                           && x.IsPublic
//                                                           && !x.IsAbstract).ToArray();
//                }
//                return extraObjectTypes;
//            }
//        }


//        public string Serialize(object obj)
//        {
//            IXSerializer x = GetSerializer(obj.GetType());
//            return x.Serialize(obj);
//        }

//        public T Deserialize<T>(string serializedObject)
//        {
//            IXSerializer x = GetSerializer(typeof(T));
//            return (T)x.Deserialize(serializedObject);
//        }
//    }
//}
