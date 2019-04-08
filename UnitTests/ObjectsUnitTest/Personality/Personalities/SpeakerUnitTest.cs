using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Global.Random.Interface;
using Objects.Global;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class SpeakerUnitTest
    {
        Speaker speaker;
        Mock<INonPlayerCharacter> npc;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            speaker = new Speaker();
            npc = new Mock<INonPlayerCharacter>();
            random = new Mock<IRandom>();

            speaker.ThingsToSay.Add("test");
        }

        [TestMethod]
        public void Spearker_Process_NotNullCommand()
        {
            string command = "command";
            Assert.AreSame(command, speaker.Process(npc.Object, command));
        }

        [TestMethod]
        public void Spearker_Process_Speak()
        {
            random.Setup(e => e.PercentDiceRoll(1)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            Assert.AreEqual("Say test", speaker.Process(npc.Object, null));
        }

        [TestMethod]
        public void Spearker_Process_NoSpeak()
        {
            random.Setup(e => e.PercentDiceRoll(2)).Returns(true);

            GlobalReference.GlobalValues.Random = random.Object;

            Assert.IsNull(speaker.Process(npc.Object, null));
        }
    }
}
