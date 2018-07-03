using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedUnitTest.Sound
{
    [TestClass]
    public class SoundUnitTest
    {
        Shared.Sound.Sound sound;

        [TestInitialize]
        public void Setup()
        {
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
