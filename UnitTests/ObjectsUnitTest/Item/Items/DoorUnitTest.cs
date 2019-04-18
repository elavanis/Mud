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
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

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

            IResult result = door.Open();
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("OpenMessage", result.ResultMessage);
        }
    }
}
