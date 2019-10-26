using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Item.Items;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class DoorUnitTest
    {
        Door door;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<IEngine> engine;
        Mock<IEvent> eventMock;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            mob = new Mock<IMobileObject>();
            engine = new Mock<IEngine>();
            eventMock = new Mock<IEvent>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            engine.Setup(e => e.Event).Returns(eventMock.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;

            door = new Door();
        }

        [TestMethod]
        public void Door_KeyNumber()
        {
            Assert.AreEqual(-1, door.KeyNumber);
        }

        [TestMethod]
        public void Door_Open()
        {
            door.Opened = false;
            door.OpenMessage = "OpenMessage";

            IResult result = door.Open(mob.Object);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("OpenMessage", result.ResultMessage);
            eventMock.Verify(e => e.Open(mob.Object, door));
        }
    }
}
