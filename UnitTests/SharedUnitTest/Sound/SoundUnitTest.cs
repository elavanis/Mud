using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedUnitTest.Sound
{
    [TestClass]
    public class SoundUnitTest
    {
        Shared.Sound.Sound sound;

        [TestInitialize]
        public void Setup()
        {
            //We actually don't need to reference GlobalValues but I want this here to make sure I didn't miss it
            //GlobalReference.GlobalValues = new GlobalValues();

            sound = new Shared.Sound.Sound();
        }

        [TestMethod]
        public void Sound_SoundName()
        {
            sound.RandomSounds.Add("abc");
            sound.RandomSounds.Add("abc");

            Assert.AreEqual("abc", sound.SoundName);
        }
    }
}
