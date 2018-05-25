using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global.Commands;
using Objects.Command.Interface;

namespace ObjectsUnitTest.Global.Commands
{
    [TestClass]
    public class ParserUnitTest
    {
        Parser parser;

        [TestInitialize]
        public void Setup()
        {
            parser = new Parser();
        }

        [TestMethod]
        public void Parser_Parse()
        {
            ICommand command = parser.Parse("This one two[3] ");

            Assert.AreEqual("THIS", command.CommandName);
            Assert.AreEqual(2, command.Parameters.Count);

            Assert.AreEqual("one", command.Parameters[0].ParameterValue);
            Assert.AreEqual(0, command.Parameters[0].ParameterNumber);

            Assert.AreEqual("two", command.Parameters[1].ParameterValue);
            Assert.AreEqual(3, command.Parameters[1].ParameterNumber);
        }
    }
}
