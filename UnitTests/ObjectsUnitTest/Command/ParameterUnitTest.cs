using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command;
using Objects.Global;

namespace ObjectsUnitTest.Command
{
    [TestClass]
    public class ParameterUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void Parameter_Constructor_NoParamNumber()
        {
            Parameter param = new Parameter("abc");

            Assert.AreEqual("abc", param.ParameterValue);
            Assert.AreEqual(0, param.ParameterNumber);
        }

        [TestMethod]
        public void Parameter_Constructor_ParamNumber()
        {
            Parameter param = new Parameter("abc[2]");

            Assert.AreEqual("abc", param.ParameterValue);
            Assert.AreEqual(2, param.ParameterNumber);
        }
    }
}
