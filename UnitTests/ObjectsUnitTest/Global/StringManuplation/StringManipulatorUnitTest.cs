using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.StringManuplation
{
    [TestClass]
    public class StringManipulatorUnitTest
    {
        Objects.Global.StringManuplation.StringManipulator stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            stringManipulator = new Objects.Global.StringManuplation.StringManipulator();
        }

        [TestMethod]
        public void StringManipulatorUnitTest_Manipulate()
        {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("{findMe}", "replaced");
            string result = stringManipulator.Manipulate(new List<KeyValuePair<string, string>>() { keyValuePair }, "this should be {findMe}.");

            string expected = "this should be replaced.";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringManipulatorUnitTest_ManipulateMultiple()
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>("{findMe}", "replaced"));
            keyValuePairs.Add(new KeyValuePair<string, string>("{findAndMe}", "alsoReplaced"));

            string result = stringManipulator.Manipulate(keyValuePairs, "this should be {findMe}, {findAndMe}, {findAndMe}.");

            string expected = "this should be replaced, alsoReplaced, alsoReplaced.";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringManipulatorUnitTest_UpdateTargetPerformer()
        {
            string result = stringManipulator.UpdateTargetPerformer("performer", "target", "this should be {target}, {performer}.");

            string expected = "this should be target, performer.";
            Assert.AreEqual(expected, result);
        }
    }
}
