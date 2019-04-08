using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Logging;
using static Objects.Global.Logging.LogMessage;

namespace ObjectsUnitTest.Global.Logging
{
    [TestClass]
    public class LogMessageUnitTest
    {
        [TestMethod]
        public void LogMessage_Constructor()
        {
            LogMessage message = new LogMessage("message", LogType.PlayerCharacter, "key");

            Assert.AreEqual("message", message.Message);
            Assert.AreEqual(LogType.PlayerCharacter, message.Type);
            Assert.AreEqual("key", message.LogKey);
        }
    }
}
