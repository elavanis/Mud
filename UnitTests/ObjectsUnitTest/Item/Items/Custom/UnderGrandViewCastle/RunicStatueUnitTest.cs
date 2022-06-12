using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Items.Custom.UnderGrandViewCastle;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items.Custom.UnderGrandViewCastle
{
    class RunicStatueUnitTest
    {
        [TestClass]
        public class RunisStatueUnitTest
        {
            RunicStatue statue;
            Mock<ITagWrapper> tagWrapper;

            [TestInitialize]
            public void Setup()
            {
                GlobalReference.GlobalValues = new GlobalValues();

                tagWrapper = new Mock<ITagWrapper>();

                tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

                GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

                statue = new RunicStatue("examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
            }

            [TestMethod]
            public void RunisStatue_Constructor()
            {
                Assert.AreEqual("examineDescription", statue.ExamineDescription);
                Assert.AreEqual("lookDescription", statue.LookDescription);
                Assert.AreEqual("sentenceDescription", statue.SentenceDescription);
                Assert.AreEqual("shortDescription", statue.ShortDescription);
            }

            [TestMethod]
            public void RunisStatue_CalculateExamDescription()
            {
                string result = statue.CalculateExamDescription();
                string expected = "The priest is dressed in robes with a medallion hanging from their belt.  The rune on the medallion appears to be able to be turned.  The run that is currently showing on the medallion is ᚠ.";
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void RunisStatue_Turn_TurnsUntilLoopBack()
            {
                IResult result = statue.Turn(null, null);
                string expected = "You turn the medallion so that the rune ᚢ is showing.";
                Assert.AreEqual(expected, result.ResultMessage);
                Assert.IsTrue(result.AllowAnotherCommand);

                result = statue.Turn(null, null);
                expected = "You turn the medallion so that the rune ᚱ is showing.";
                Assert.AreEqual(expected, result.ResultMessage);
                Assert.IsTrue(result.AllowAnotherCommand);

                result = statue.Turn(null, null);
                expected = "You turn the medallion so that the rune ᚠ is showing.";
                Assert.AreEqual(expected, result.ResultMessage);
                Assert.IsTrue(result.AllowAnotherCommand);

                result = statue.Turn(null, null);
                expected = "You turn the medallion so that the rune ᚢ is showing.";
                Assert.AreEqual(expected, result.ResultMessage);
                Assert.IsTrue(result.AllowAnotherCommand);
            }
        }
    }
}
