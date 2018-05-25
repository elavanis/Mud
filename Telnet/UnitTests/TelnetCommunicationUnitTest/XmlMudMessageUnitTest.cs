//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TelnetCommunication;
//using TelnetCommunication.Interface;

//namespace TelnetCommunicationUnitTest
//{
//    [TestClass]
//    public class XmlMudMessageUnitTest
//    {
//        [TestMethod]
//        public void XmlMudMessage_SerializeDeserialize()
//        {
//            XmlMudMessage xmlMudMessage = new XmlMudMessage();
//            xmlMudMessage.Guid = "1";
//            xmlMudMessage.InBoundMessage = "in message";
//            xmlMudMessage.OutBoundMessage = "out message";

//            string serializeMessage = xmlMudMessage.Serialize();

//            IMudMessage mudMessage = XmlMudMessage.Instance.StringToMessage(serializeMessage);

//            Assert.AreEqual(xmlMudMessage.Guid, mudMessage.Guid);
//            Assert.AreEqual(xmlMudMessage.InBoundMessage, mudMessage.InBoundMessage);
//            Assert.AreEqual(xmlMudMessage.OutBoundMessage, mudMessage.OutBoundMessage);
//        }

//        [TestMethod]
//        public void XmlMudMessage_CreateNewInstance()
//        {
//            XmlMudMessage xmlMudMessage = new XmlMudMessage();
//            Assert.IsNotNull(xmlMudMessage.CreateNewInstance() as XmlMudMessage);
//        }
//    }
//}
