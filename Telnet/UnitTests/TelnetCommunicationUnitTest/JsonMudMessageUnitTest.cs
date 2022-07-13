using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TelnetCommunication;

namespace TelnetCommunicationUnitTest
{
    [TestClass]
    public class JsonMudMessageUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            //We actually don't need to reference GlobalValues but I want this here to make sure I didn't miss it
            //GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void JsonMudMessage_Serialize()
        {
            JsonMudMessage mudMessage = new JsonMudMessage("", "test");
            //mudMessage.Message = "test";

            string expected = "ù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": \"\",\r\n  \"Message\": \"test\"\r\n}ÿ";
            Assert.AreEqual(expected, mudMessage.Serialize());
        }

        [TestMethod]
        public void JsonMudMessage_Deserialize()
        {
            string rawMessage = "ù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": null,\r\n  \"Message\": \"1st message\"\r\n}ÿù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": null,\r\n  \"Message\": \"2nd message\"\r\n}ù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": null,\r\n  \"Message\": \"3rd message\"\r\n}ÿù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": null,\r\n  \"Message\": \"4th message\"\r\n}";
            JsonMudMessage mudMessage = new JsonMudMessage("","");

            Tuple<List<string>, string> result = mudMessage.ParseRawMessage(rawMessage);

            Assert.AreEqual(2, result.Item1.Count);
            Assert.IsTrue(result.Item1.Contains("1st message"));
            Assert.IsTrue(result.Item1.Contains("3rd message"));
            string expectedRemainingMessage = "ù{\r\n  \"$type\": \"TelnetCommunication.JsonMudMessage, TelnetCommunication\",\r\n  \"Guid\": null,\r\n  \"Message\": \"4th message\"\r\n}";
            Assert.AreEqual(expectedRemainingMessage, result.Item2);
        }

        [TestMethod]
        public void JsonMudMessage_Constructor()
        {
            JsonMudMessage mudMessage = new JsonMudMessage("","");
            Assert.IsTrue(mudMessage.CreateNewInstance("","") is JsonMudMessage);
        }
    }
}
