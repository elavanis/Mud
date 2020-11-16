using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Custom.UnderGrandViewCastle;
using Objects.Item.Items.Custom.UnderGrandViewCastle.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.World.Interface;
using Objects.Zone.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Item.Items.Custom.UnderGrandViewCastle
{
    [TestClass]
    public class RunicButtonUnitTest
    {
        RunicButton button;
        Mock<ITagWrapper> tagWrapper;
        Mock<IWorld> world;
        Mock<IZone> zone;
        Mock<IRoom> room4;
        Mock<IRoom> room6;
        Mock<IRoom> room22;
        Mock<IMobileObject> performer;
        Mock<IItem> itemStatue4;
        Mock<IItem> itemStatue6;
        Mock<IItem> itemStatue22;
        Mock<IRunicStatue> statue4;
        Mock<IRunicStatue> statue6;
        Mock<IRunicStatue> statue22;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            world = new Mock<IWorld>();
            zone = new Mock<IZone>();
            room4 = new Mock<IRoom>();
            room6 = new Mock<IRoom>();
            room22 = new Mock<IRoom>();
            performer = new Mock<IMobileObject>();
            statue4 = new Mock<IRunicStatue>();
            statue6 = new Mock<IRunicStatue>();
            statue22 = new Mock<IRunicStatue>();
            itemStatue4 = statue4.As<IItem>();
            itemStatue6 = statue6.As<IItem>();
            itemStatue22 = statue22.As<IItem>();
            Dictionary<int, IZone> dictionaryZone = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dictionaryRoom = new Dictionary<int, IRoom>();
            List<IItem> itemsForRoom4 = new List<IItem>() { itemStatue4.Object };
            List<IItem> itemsForRoom6 = new List<IItem>() { itemStatue6.Object };
            List<IItem> itemsForRoom22 = new List<IItem>() { itemStatue22.Object };

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            world.Setup(e => e.Zones).Returns(dictionaryZone);
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);
            performer.Setup(e => e.Room).Returns(room4.Object);
            room4.Setup(e => e.Items).Returns(itemsForRoom4);
            room6.Setup(e => e.Items).Returns(itemsForRoom6);
            room22.Setup(e => e.Items).Returns(itemsForRoom22);
            dictionaryZone.Add(0, zone.Object);
            dictionaryRoom.Add(4, room4.Object);
            dictionaryRoom.Add(6, room6.Object);
            dictionaryRoom.Add(22, room22.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.World = world.Object;

            button = new RunicButton();
        }



        #region 0 Red
        #region 0 Green
        [TestMethod]
        public void RunicButton_Push_0Red0Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the roars with bubbles splashing on the floor around the pool black.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_0Red0Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the roars with bubbles splashing on the floor around the pool dark blue.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }
        [TestMethod]
        public void RunicButton_Push_0Red0Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns blue.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 0 Green
        #region 128 Green
        [TestMethod]
        public void RunicButton_Push_0Red128Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the roars with bubbles splashing on the floor around the pool dark green.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_0Red128Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns teal.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_0Red128Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns azure blue.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 128 Green
        #region 255 Green
        [TestMethod]
        public void RunicButton_Push_0Red255Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns green.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_0Red255Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns spring green.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_0Red255Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(2);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns cyan.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 255 Green
        #endregion 0 Red

        #region 128 Red
        #region 0 Green
        [TestMethod]
        public void RunicButton_Push_128Red0Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the roars with bubbles splashing on the floor around the pool maroon.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_128Red0Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns purple.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }
        [TestMethod]
        public void RunicButton_Push_128Red0Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns violet.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 0 Green
        #region 128 Green
        [TestMethod]
        public void RunicButton_Push_128Red128Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns olive.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_128Red128Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns gray.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_128Red128Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns slate blue.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 128 Green
        #region 255 Green
        [TestMethod]
        public void RunicButton_Push_128Red255Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns chartreuse.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_128Red255Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns mint green.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_128Red255Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(1);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns sky blue.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 255 Green
        #endregion 128 Red

        #region 255 Red
        #region 0 Green
        [TestMethod]
        public void RunicButton_Push_255Red0Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns red.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_255Red0Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns rose.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }
        [TestMethod]
        public void RunicButton_Push_255Red0Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(2);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns magenta.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 0 Green
        #region 128 Green
        [TestMethod]
        public void RunicButton_Push_255Red128Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles and turns orange.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_255Red128Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns dark pink.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_255Red128Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(1);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns orchid.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 128 Green
        #region 255 Green
        [TestMethod]
        public void RunicButton_Push_255Red255Green0Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(2);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns yellow.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(0, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_255Red255Green128Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(1);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("The pool of liquid in the center of the room bubbles slightly and turns pale yellow.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(128, GetPrivateColor(button, Colors.Blue));
        }

        [TestMethod]
        public void RunicButton_Push_255Red255Green255Blue()
        {
            statue4.Setup(e => e.SelectedRune).Returns(0);
            statue6.Setup(e => e.SelectedRune).Returns(0);
            statue22.Setup(e => e.SelectedRune).Returns(0);

            IResult result = button.Push(performer.Object, null);

            Assert.AreEqual("You push the button but nothing happens.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Red));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Green));
            Assert.AreEqual(255, GetPrivateColor(button, Colors.Blue));
        }
        #endregion 255 Green
        #endregion 255 Red

        private int GetPrivateColor(RunicButton runicButton, Colors colors)
        {
            string fieldName = "old" + colors.ToString();

            FieldInfo fieldInfo = runicButton.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            int value = (int)fieldInfo.GetValue(runicButton);

            return value;
        }

        private enum Colors
        {
            Red,
            Green,
            Blue
        }

    }
}
