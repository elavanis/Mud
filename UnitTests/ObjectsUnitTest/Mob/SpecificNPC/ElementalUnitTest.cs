using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Settings.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC;
using Objects.Room.Interface;
using Objects.World.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Mob.SpecificNPC
{
    [TestClass]
    public class ElementalUnitTest
    {
        Elemental elemental;
        PropertyInfo roundTickCounter;
        Mock<IWorld> world;
        Mock<ISettings> settings;
        Mock<IDefaultValues> defaultValues;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IRoom> room;
        Mock<IMoneyToCoins> moneyToCoins;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            world = new Mock<IWorld>();
            settings = new Mock<ISettings>();
            defaultValues = new Mock<IDefaultValues>();
            tagWrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();
            moneyToCoins = new Mock<IMoneyToCoins>();

            world.Setup(e => e.Precipitation).Returns(100);
            world.Setup(e => e.WindSpeed).Returns(100);
            settings.Setup(e => e.MaxLevel).Returns(100);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            moneyToCoins.Setup(e => e.FormatedAsCoins(0)).Returns("0 coins");

            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;

            elemental = new Elemental(ElementType.Air,room.Object);
            roundTickCounter = elemental.GetType().GetProperty("RoundTickCounter", BindingFlags.NonPublic | BindingFlags.Instance);

            roundTickCounter.SetValue(elemental, -1);
        }

        [TestMethod]
        public void Elemental_Constructor_Air()
        {
            elemental = new Elemental(ElementType.Air, room.Object);

            Assert.AreEqual("air", elemental.KeyWords[0]);
        }

        [TestMethod]
        public void Elemental_Constructor_Earth()
        {
            elemental = new Elemental(ElementType.Earth, room.Object);

            Assert.AreEqual("earth", elemental.KeyWords[0]);
        }

        [TestMethod]
        public void Elemental_Constructor_Fire()
        {
            elemental = new Elemental(ElementType.Fire, room.Object);

            Assert.AreEqual("fire", elemental.KeyWords[0]);
        }

        [TestMethod]
        public void Elemental_Constructor_Water()
        {
            elemental = new Elemental(ElementType.Water, room.Object);

            Assert.AreEqual("water", elemental.KeyWords[0]);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_SkipRound()
        {
            roundTickCounter.SetValue(elemental, 0);

            elemental.ProcessElementalTick();

            world.Verify(e => e.Precipitation, Times.Never);
            world.Verify(e => e.WindSpeed, Times.Never);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Air_GainLevel()
        {
            elemental.Level = 1;
            elemental.FinishLoad();
            Assert.AreEqual(1, elemental.EquipedArmor.Count());
            elemental.ProcessElementalTick();

            Assert.AreEqual(2, elemental.Level);
            Assert.AreEqual(2, elemental.EquipedArmor.Count());
            world.Verify(e => e.WindSpeed, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "The air elemental grows stronger."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Air_LooseLevel()
        {
            elemental.Level = 5;
            elemental.FinishLoad();
            Assert.AreEqual(5, elemental.EquipedArmor.Count());
            world.Setup(e => e.WindSpeed).Returns(0);

            elemental.ProcessElementalTick();

            Assert.AreEqual(4, elemental.Level);
            Assert.AreEqual(4, elemental.EquipedArmor.Count());
            world.Verify(e => e.WindSpeed, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "The air elemental grows weaker."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Air_NoChange()
        {
            world.Setup(e => e.WindSpeed).Returns(60);

            elemental.ProcessElementalTick();

            Assert.AreEqual(1, elemental.Level);
            world.Verify(e => e.WindSpeed, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false), Times.Never);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Earth_LooseLevel()
        {
            elemental = new Elemental(ElementType.Earth, room.Object);
            elemental.Room = room.Object;
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { elemental });
            roundTickCounter.SetValue(elemental, -1);
            elemental.Level = 1;
            elemental.FinishLoad();
            Assert.AreEqual(1, elemental.EquipedArmor.Count());
            elemental.ProcessElementalTick();

            Assert.AreEqual(1, elemental.Level);
            Assert.AreEqual(1, elemental.EquipedArmor.Count());
            world.Verify(e => e.Precipitation, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "The earth elemental has grown so weak it can no longer hold its form in this realm and slowly fades away."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Earth_GainLevel()
        {
            elemental = new Elemental(ElementType.Earth, room.Object);
            roundTickCounter.SetValue(elemental, -1);
            elemental.Level = 5;
            elemental.FinishLoad();
            Assert.AreEqual(5, elemental.EquipedArmor.Count());
            world.Setup(e => e.Precipitation).Returns(0);

            elemental.ProcessElementalTick();

            Assert.AreEqual(6, elemental.Level);
            Assert.AreEqual(6, elemental.EquipedArmor.Count());
            world.Verify(e => e.Precipitation, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "The earth elemental grows stronger."), null, true, false), Times.Once);
        }

        [TestMethod]
        public void Elemental_ProcessElementalTick_Earth_NoChange()
        {
            elemental = new Elemental(ElementType.Earth, room.Object);
            roundTickCounter.SetValue(elemental, -1);
            world.Setup(e => e.Precipitation).Returns(40);

            elemental.ProcessElementalTick();

            Assert.AreEqual(1, elemental.Level);
            world.Verify(e => e.Precipitation, Times.AtLeastOnce);
            notify.Verify(e => e.Room(elemental, null, room.Object, It.IsAny<ITranslationMessage>(), null, true, false), Times.Never);
        }
    }
}
