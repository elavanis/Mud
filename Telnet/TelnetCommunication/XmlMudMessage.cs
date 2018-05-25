//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Serialization;
//using TelnetCommunication.Interface;

//namespace TelnetCommunication
//{
//    public class XmlMudMessage : IMudMessage
//    {
//        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlMudMessage));

//        public static XmlMudMessage Instance { get; } = new XmlMudMessage();


//        [ExcludeFromCodeCoverage]
//        public string Guid { get; set; }
//        [ExcludeFromCodeCoverage]
//        public string OutBoundMessage { get; set; }
//        [ExcludeFromCodeCoverage]
//        public string InBoundMessage { get; set; }

//        [ExcludeFromCodeCoverage]
//        public string MessageDelimiter { get; set; } = "<?xml";

//        public string Serialize()
//        {
//            string sendMessage;
//            using (StringWriter sw = new StringWriter())
//            {
//                using (XmlWriter xw = XmlWriter.Create(sw))
//                {
//                    xmlSerializer.Serialize(xw, this);
//                    sendMessage = sw.ToString();
//                }
//            }

//            return sendMessage;
//        }

//        public IMudMessage StringToMessage(string stringMessage)
//        {
//            XmlMudMessage message;
//            using (TextReader reader = new StringReader(stringMessage))
//            {
//                message = (XmlMudMessage)xmlSerializer.Deserialize(reader);
//            }
//            return message;
//        }

//        public IMudMessage CreateNewInstance()
//        {
//            return new XmlMudMessage();
//        }
//    }
//}
