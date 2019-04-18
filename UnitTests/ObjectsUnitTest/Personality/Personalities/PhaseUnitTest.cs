using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class PhaseUnitTest
    {
        Phase phase;
        Mock<INonPlayerCharacter> npc;
        Mock<IBaseObjectId> room;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            phase = new Phase();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IBaseObjectId>();
            random = new Mock<IRandom>();

            room.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.Id).Returns(2);
            phase.RoomsToPhaseTo.Add(room.Object);
            random.Setup(e => e.PercentDiceRoll(1)).Returns(true);
            random.Setup(e => e.Next(1)).Returns(0);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void Phase_Process_NoCombat()
        {
            string result = phase.Process(npc.Object, null);
            Assert.AreEqual("Goto 1 2", result);
        }

        [TestMethod]
        public void Phase_Process_ExistingCommand()
        {
            string result = phase.Process(npc.Object, "oldCommand");
            Assert.AreEqual("oldCommand", result);
        }
    }
}