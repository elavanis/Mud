using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class ContainerUnitTest
    {
        Container container;
        Mock<ITagWrapper> tagWrapper;


        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            container = new Container();
        }

        [TestMethod]
        public void Container_Items_Blank()
        {
            Assert.IsNotNull(container.Items);
            Assert.AreEqual(0, container.Items.Count);
        }

        [TestMethod]
        public void Container_Items_Populated()
        {
            Mock<IItem> item = new Mock<IItem>();
            container.Items.Add(item.Object);

            Assert.AreEqual(1, container.Items.Count);
            Assert.AreSame(item.Object, container.Items[0]);
        }

        [TestMethod]
        public void Container_Open()
        {
            container.Opened = false;
            container.OpenMessage = "OpenMessage";

            IResult result = container.Open();
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("OpenMessage", result.ResultMessage);
        }
    }
}
