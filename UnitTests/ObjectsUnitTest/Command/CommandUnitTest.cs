using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Command
{
    [TestClass]
    public class CommandUnitTest
    {
        Objects.Command.Command command;

        [TestInitialize]
        public void Setup()
        {
            command = new Objects.Command.Command();
        }


        [TestMethod]
        public void Command_Parameters()
        {
            Assert.IsInstanceOfType(command.Parameters, typeof(List<IParameter>));
        }
    }
}
