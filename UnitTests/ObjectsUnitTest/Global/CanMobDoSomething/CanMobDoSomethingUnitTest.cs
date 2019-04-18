using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Mob.Interface;
using Moq;
using Objects.Room.Interface;
using Objects.Command.Interface;
using System.Collections.Generic;
using static Objects.Room.Room;
using Objects.Global.GameDateTime.Interface;
using Objects.Global;
using static Objects.Mob.MobileObject;
using Objects.Item.Items.Interface;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Item;
using Objects.Item.Interface;
using Objects.Global.Random.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.GameDateTime.Interface;

namespace ObjectsUnitTest.Global.CanMobDoSomething
{
    [TestClass]
    public class CanMobDoSomethingUnitTest
    {
        Objects.Global.CanMobDoSomething.CanMobDoSomething canDoSomething;
        Mock<IMobileObject> mob;
        Mock<IInGameDateTime> inGameDateTime;
        Mock<IGameDateTime> gameDateTime;
        Mock<ITagWrapper> tagWrapper;
        Mock<IRoom> room;
        Mock<IItem> item;
        Mock<IMobileObject> altMob;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            canDoSomething = new Objects.Global.CanMobDoSomething.CanMobDoSomething();

            mob = new Mock<IMobileObject>();
            inGameDateTime = new Mock<IInGameDateTime>();
            room = new Mock<IRoom>();
            item = new Mock<IItem>();
            altMob = new Mock<IMobileObject>();
            gameDateTime = new Mock<IGameDateTime>();
            tagWrapper = new Mock<ITagWrapper>();

            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>());
            mob.Setup(e => e.Room).Returns(room.Object);
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.Invisible });
            altMob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.Invisibile });
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        #region SeeDueToLight
        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_RoomDark()
        {
            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.NoLight });

            Assert.IsFalse(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_Night()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            Assert.IsFalse(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_NightRoomLite()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            room.Setup(e => e.Attributes).Returns(new HashSet<RoomAttribute>() { RoomAttribute.Light });

            Assert.IsTrue(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_Day()
        {
            Assert.IsTrue(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_NightInfravision()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            mob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.Infravision });

            Assert.IsTrue(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_NightHoldLight()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Held);
            equipment.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.Light });
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equipment.Object });

            Assert.IsTrue(canDoSomething.SeeDueToLight(mob.Object));
        }


        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_NightHoldNotLight()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Held);
            equipment.Setup(e => e.Attributes).Returns(new List<ItemAttribute>());
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equipment.Object });

            Assert.IsFalse(canDoSomething.SeeDueToLight(mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeDueToLight_NightNotHeldLight()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);

            Mock<IEquipment> equipment = new Mock<IEquipment>();
            equipment.Setup(e => e.ItemPosition).Returns(AvalableItemPosition.Head);
            equipment.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.Light });
            mob.Setup(e => e.EquipedEquipment).Returns(new List<IEquipment>() { equipment.Object });

            Assert.IsFalse(canDoSomething.SeeDueToLight(mob.Object));
        }
        #endregion SeeDueToLight

        #region SeeObject
        [TestMethod]
        public void CanMobDoSomething_SeeObject_CanNotSeeSelf()
        {
            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, mob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_Asleep()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Sleep);
            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, room.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_Dark()
        {
            gameDateTime.Setup(e => e.Hour).Returns(12);
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);

            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, room.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_ItemInvisibleNotSeeInvisible()
        {
            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, item.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_ItemInvisibleCanSeeInvisible()
        {
            mob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.SeeInvisible });

            Assert.IsTrue(canDoSomething.SeeObject(mob.Object, item.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_MobInvisibleNotSeeInvisible()
        {
            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, altMob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_SeeObject_MobInvisibleCanSeeInvisible()
        {
            mob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.SeeInvisible });

            Assert.IsTrue(canDoSomething.SeeObject(mob.Object, altMob.Object));
        }

        [TestMethod]
        public void ConMobDoSomething_SeeObject_MobHiddenNotDetected()
        {
            altMob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.Hidden });
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(0)).Returns(0);
            random.Setup(e => e.Next(1)).Returns(1);

            altMob.Setup(e => e.DexterityEffective).Returns(1);
            altMob.Setup(e => e.IntelligenceEffective).Returns(1);
            mob.Setup(e => e.DexterityEffective).Returns(0);
            mob.Setup(e => e.IntelligenceEffective).Returns(0);
            GlobalReference.GlobalValues.Random = random.Object;

            Assert.IsFalse(canDoSomething.SeeObject(mob.Object, altMob.Object));
        }

        [TestMethod]
        public void ConMobDoSomething_SeeObject_MobHiddenDetected()
        {
            altMob.Setup(e => e.AttributesCurrent).Returns(new List<MobileAttribute>() { MobileAttribute.Hidden });
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(0)).Returns(0);
            random.Setup(e => e.Next(1)).Returns(1);

            altMob.Setup(e => e.DexterityEffective).Returns(0);
            altMob.Setup(e => e.IntelligenceEffective).Returns(0);
            mob.Setup(e => e.DexterityEffective).Returns(1);
            mob.Setup(e => e.IntelligenceEffective).Returns(1);
            GlobalReference.GlobalValues.Random = random.Object;

            Assert.IsTrue(canDoSomething.SeeObject(mob.Object, altMob.Object));
        }
        #endregion SeeObject

        #region Move
        [TestMethod]
        public void CanMobDoSomething_Move_Sleep()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Sleep);

            IResult result = canDoSomething.Move(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not move while asleep.", result.ResultMessage);
        }

        [TestMethod]
        public void CanMobDoSomething_Move_Sit()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Sit);

            IResult result = canDoSomething.Move(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not move while sitting.", result.ResultMessage);
        }

        [TestMethod]
        public void CanMobDoSomething_Move_Relax()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Relax);

            IResult result = canDoSomething.Move(mob.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not move while relaxing.", result.ResultMessage);
        }

        [TestMethod]
        public void CanMobDoSomething_Move_Other()
        {
            mob.Setup(e => e.Position).Returns(CharacterPosition.Stand);

            IResult result = canDoSomething.Move(mob.Object);
            Assert.IsNull(result);
        }
        #endregion Move

        #region Hear
        [TestMethod]
        public void CanMobDoSomething_Hear_NotSelf()
        {
            Assert.IsTrue(canDoSomething.Hear(mob.Object, altMob.Object));
        }

        [TestMethod]
        public void CanMobDoSomething_Hear_Self()
        {
            Assert.IsFalse(canDoSomething.Hear(mob.Object, mob.Object));
        }
        #endregion Hear
    }
}
