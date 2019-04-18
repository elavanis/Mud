using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Personality.Personalities;
using Moq;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global;
using static Objects.Global.Direction.Directions;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class GuardUnitTest
    {
        Guard guard;
        Mock<INonPlayerCharacter> npc;
        Mock<INonPlayerCharacter> npc2;
        Mock<IPlayerCharacter> pc;
        Mock<IRoom> room;
        Mock<IEngine> engine;
        Mock<ICombat> combat;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            guard = new Guard();
            npc = new Mock<INonPlayerCharacter>();
            npc2 = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            room = new Mock<IRoom>();
            engine = new Mock<IEngine>();
            combat = new Mock<ICombat>();

            npc.Setup(e => e.Room).Returns(room.Object);
            engine.Setup(e => e.Combat).Returns(combat.Object);

            GlobalReference.GlobalValues.Engine = engine.Object;
        }

        [TestMethod]
        public void Guard_Constructor()
        {
            Guard guard = new Guard(Direction.Down);

            Assert.IsTrue(guard.GuardDirections.Contains(Direction.Down));
        }

        [TestMethod]
        public void Guard_Process_CommandNotNull()
        {
            string command = "original";

            Assert.AreSame(command, guard.Process(npc.Object, command));
        }
    }
}
