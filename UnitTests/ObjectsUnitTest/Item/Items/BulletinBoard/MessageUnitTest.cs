using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Language.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using static Objects.Global.Language.Translator;

namespace ObjectsUnitTest.Item.Items.BulletinBoard
{
    [TestClass]
    public class MessageUnitTest
    {
        Objects.Item.Items.BulletinBoard.Message message;
        Mock<IMobileObject> mob;
        Mock<ITranslator> translator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mob = new Mock<IMobileObject>();
            translator = new Mock<ITranslator>();

            mob.Setup(e => e.KnownLanguages).Returns(new HashSet<Languages>() { Languages.Common });
            translator.Setup(e => e.Translate(Languages.AncientMagic, "Subject\r\nText\r\nSincerely\r\nPoster")).Returns("translatedMessage");

            GlobalReference.GlobalValues.Translator = translator.Object;

            message = new Objects.Item.Items.BulletinBoard.Message();

            message.Poster = "Poster";
            message.Subject = "Subject";
            message.Text = "Text";
        }


        [TestMethod]
        public void Message_Read_KnownLanguage()
        {
            string result = message.Read(mob.Object);
            string expected = "Subject\r\nText\r\nSincerely\r\nPoster";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Message_Read_UnknownLanguage()
        {
            message.WrittenLanguage = Languages.AncientMagic;

            string result = message.Read(mob.Object);
            string expected = "translatedMessage";
            Assert.AreEqual(expected, result);
        }
    }
}
