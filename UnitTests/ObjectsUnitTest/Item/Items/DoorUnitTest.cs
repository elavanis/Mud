using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Items;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class DoorUnitTest
    {
        Door door;

        [TestInitialize]
        public void Setup()
        {
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

            Mock<ITagWrapper> tagwrapper = new Mock<ITagWrapper>();
            tagwrapper.Setup(e => e.WrapInTag("OpenMessage", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;

            IResult result = door.Open();
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }
    }
}
