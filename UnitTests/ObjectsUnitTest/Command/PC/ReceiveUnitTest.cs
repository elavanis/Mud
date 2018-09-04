using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects;
using Objects.Command.Interface;
using Objects.Command.PC;
using Objects.Crafting.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class ReceiveUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        List<IItem> items = new List<IItem>();
        Mock<IPlayerCharacter> pc;
        Mock<INonPlayerCharacter> npc;
        Mock<IRoom> room;
        List<ICraftsmanObject> craftsmanObjects = new List<ICraftsmanObject>();
        Mock<ICraftsmanObject> craftsmanObject1 = new Mock<ICraftsmanObject>();
        Mock<IItem> item1;


        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            pc = new Mock<IPlayerCharacter>();
            npc = new Mock<INonPlayerCharacter>();
            room = new Mock<IRoom>();
            craftsmanObject1 = new Mock<ICraftsmanObject>();
            item1 = new Mock<IItem>();

            tagWrapper.Setup(e => e.WrapInTag("Receive", TagType.Info)).Returns("message");
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.CraftsmanObjects).Returns(craftsmanObjects);
            pc.Setup(e => e.Items).Returns(items);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "pc" });
            npc.Setup(e => e.Zone).Returns(1);
            npc.Setup(e => e.Id).Returns(2);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            craftsmanObject1.Setup(e => e.Completion).Returns(DateTime.Now);
            craftsmanObject1.Setup(e => e.CraftsmanId).Returns(new BaseObjectId() { Zone = 1, Id = 2 });
            craftsmanObject1.Setup(e => e.Item).Returns(item1.Object);
            item1.Setup(e => e.KeyWords).Returns(new List<string>() { "item" });


            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new Receive();
        }

        [TestMethod]
        public void Receive_Instructions()
        {
            IResult result = command.Instructions;

             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Receive_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Receive"));
        }

        [TestMethod]
        public void Receive_NotPc()
        {
            tagWrapper.Setup(e => e.WrapInTag("Only player characters can receive.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(npc.Object, null);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Receive_NoCraftsman()
        {
            tagWrapper.Setup(e => e.WrapInTag("No craftsman found at this location.", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(pc.Object, null);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
        }


        [TestMethod]
        public void Receive_GetItem()
        {
            Mock<ICraftsman> craftsman = new Mock<ICraftsman>();
            craftsmanObjects.Add(craftsmanObject1.Object);

            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { craftsman.Object });
            tagWrapper.Setup(e => e.WrapInTag("", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(pc.Object, null);
             Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            npc.Verify(e => e.EnqueueCommand("Tell pc As promised, here is your item."));
        }

        [TestMethod]
        public void Receive_NoItem()
        {
            Mock<ICraftsman> craftsman = new Mock<ICraftsman>();
            craftsmanObjects.Add(craftsmanObject1.Object);

            craftsmanObject1.Setup(e => e.Completion).Returns(new DateTime(9999, 1, 1));
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { craftsman.Object });
            tagWrapper.Setup(e => e.WrapInTag("", TagType.Info)).Returns("message");

            IResult result = command.PerformCommand(pc.Object, null);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            npc.Verify(e => e.EnqueueCommand("Tell pc Sorry I don't have anything for you to pick up at this time."));
        }
    }
}
