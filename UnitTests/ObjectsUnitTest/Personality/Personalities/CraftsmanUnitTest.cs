using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Crafting.Interface;
using Objects.Damage.Interface;
using Objects.Die;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class CraftsmanUnitTest
    {
        Craftsman craftsman;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        List<ICraftsmanObject> craftsmanObjects;
        Mock<IGameDateTime> gameDateTime;

        [TestInitialize]
        public void Setup()
        {
            craftsman = new Craftsman();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            Mock<IDefaultValues> defaultValues = new Mock<IDefaultValues>();
            Mock<IMoneyToCoins> moneyToCoins = new Mock<IMoneyToCoins>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            craftsmanObjects = new List<ICraftsmanObject>();
            gameDateTime = new Mock<IGameDateTime>();

            defaultValues.Setup(e => e.MoneyForNpcLevel(10)).Returns(1000);
            defaultValues.Setup(e => e.DiceForWeaponLevel(1)).Returns(new Dice(1, 2));
            pc.Setup(e => e.CharismaEffective).Returns(1);
            pc.Setup(e => e.KeyWords).Returns(new List<string>() { "pc" });
            npc.Setup(e => e.CharismaEffective).Returns(2);
            npc.Setup(e => e.Zone).Returns(1);
            npc.Setup(e => e.Id).Returns(2);
            pc.Setup(e => e.Money).Returns(1000);
            pc.Setup(e => e.CraftsmanObjects).Returns(craftsmanObjects);
            moneyToCoins.Setup(e => e.FormatedAsCoins(20000)).Returns("2 gold");
            tagWrapper.Setup(e => e.WrapInTag("You need 2 gold to have the item made for you.", TagType.Info)).Returns("not enough money");
            tagWrapper.Setup(e => e.WrapInTag("", TagType.Info)).Returns("");
            gameDateTime.Setup(e => e.BuildFormatedDateTime(It.IsAny<DateTime>())).Returns("future date");

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.MoneyToCoins = moneyToCoins.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.GameDateTime = gameDateTime.Object;
        }

        [TestMethod]
        public void Craftsman_Build_NotEnoughMoney()
        {
            IResult result = craftsman.Build(npc.Object, pc.Object, AvalableItemPosition.Arms, 10, "keyword", "sentenceDescription", "shortDescription", "longDescription", "examineDescription");

            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("not enough money", result.ResultMessage);
        }

        [TestMethod]
        public void Craftsman_Build_Armor()
        {
            DateTime start = DateTime.Now;
            IResult result = craftsman.Build(npc.Object, pc.Object, AvalableItemPosition.Feet, 1, "keyword", "sentenceDescription", "shortDescription", "longDescription", "examineDescription");

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("", result.ResultMessage);
            Assert.AreEqual(1, craftsmanObjects.Count);
            ICraftsmanObject craftsmanObject = craftsmanObjects[0];
            IArmor item = craftsmanObject.Item as IArmor;
            Assert.IsNotNull(item);
            Assert.AreEqual(AvalableItemPosition.Feet, item.ItemPosition);
            Assert.AreEqual(1, item.Level);
            Assert.AreEqual(1, item.KeyWords.Count);
            Assert.AreEqual("keyword", item.KeyWords[0]);
            Assert.AreEqual("sentenceDescription", item.SentenceDescription);
            Assert.AreEqual("shortDescription", item.ShortDescription);
            Assert.AreEqual("longDescription", item.LongDescription);
            Assert.AreEqual("examineDescription", item.ExamineDescription);
            Assert.AreEqual(1, craftsmanObject.CraftsmanId.Zone);
            Assert.AreEqual(2, craftsmanObject.CraftsmanId.Id);

            DateTime end = DateTime.Now;
            Assert.IsTrue(start.AddMinutes(1) <= craftsmanObject.Completion);
            Assert.IsTrue(end.AddMinutes(1) >= craftsmanObject.Completion);
        }

        [TestMethod]
        public void Craftsman_Build_Held()
        {
            DateTime start = DateTime.Now;
            IResult result = craftsman.Build(npc.Object, pc.Object, AvalableItemPosition.Held, 1, "keyword", "sentenceDescription", "shortDescription", "longDescription", "examineDescription");

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("", result.ResultMessage);
            Assert.AreEqual(1, craftsmanObjects.Count);
            ICraftsmanObject craftsmanObject = craftsmanObjects[0];
            IEquipment item = craftsmanObject.Item as IEquipment;
            Assert.IsNotNull(item);
            Assert.AreEqual(AvalableItemPosition.Held, item.ItemPosition);
            Assert.AreEqual(1, item.Level);
            Assert.AreEqual(1, item.KeyWords.Count);
            Assert.AreEqual("keyword", item.KeyWords[0]);
            Assert.AreEqual("sentenceDescription", item.SentenceDescription);
            Assert.AreEqual("shortDescription", item.ShortDescription);
            Assert.AreEqual("longDescription", item.LongDescription);
            Assert.AreEqual("examineDescription", item.ExamineDescription);
            Assert.AreEqual(1, craftsmanObject.CraftsmanId.Zone);
            Assert.AreEqual(2, craftsmanObject.CraftsmanId.Id);

            DateTime end = DateTime.Now;
            Assert.IsTrue(start.AddMinutes(1) <= craftsmanObject.Completion);
            Assert.IsTrue(end.AddMinutes(1) >= craftsmanObject.Completion);
        }

        [TestMethod]
        public void Craftsman_Build_Weapon()
        {
            DateTime start = DateTime.Now;
            IResult result = craftsman.Build(npc.Object, pc.Object, AvalableItemPosition.Wield, 1, "keyword", "sentenceDescription", "shortDescription", "longDescription", "examineDescription", DamageType.Pierce);

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("", result.ResultMessage);
            Assert.AreEqual(1, craftsmanObjects.Count);
            ICraftsmanObject craftsmanObject = craftsmanObjects[0];
            IWeapon item = craftsmanObject.Item as IWeapon;
            Assert.IsNotNull(item);
            Assert.AreEqual(AvalableItemPosition.Wield, item.ItemPosition);
            Assert.AreEqual(1, item.Level);
            Assert.AreEqual(1, item.KeyWords.Count);
            Assert.AreEqual("keyword", item.KeyWords[0]);
            Assert.AreEqual("sentenceDescription", item.SentenceDescription);
            Assert.AreEqual("shortDescription", item.ShortDescription);
            Assert.AreEqual("longDescription", item.LongDescription);
            Assert.AreEqual("examineDescription", item.ExamineDescription);
            Assert.AreEqual(1, craftsmanObject.CraftsmanId.Zone);
            Assert.AreEqual(2, craftsmanObject.CraftsmanId.Id);

            DateTime end = DateTime.Now;
            Assert.IsTrue(start.AddMinutes(1) <= craftsmanObject.Completion);
            Assert.IsTrue(end.AddMinutes(1) >= craftsmanObject.Completion);

            IDamage damage = new Objects.Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(item.Level));
            Assert.AreEqual(1, item.DamageList.Count);
            IDamage itemDamge = item.DamageList[0];
            Assert.AreEqual(damage.BonusDamageStat, itemDamge.BonusDamageStat);
            Assert.AreEqual(damage.BonusDefenseStat, itemDamge.BonusDefenseStat);
            Assert.AreEqual(damage.Dice.Die, itemDamge.Dice.Die);
            Assert.AreEqual(damage.Dice.Sides, itemDamge.Dice.Sides);
            Assert.AreEqual(DamageType.Pierce, itemDamge.Type);
        }

        [TestMethod]
        public void Craftsman_Build_NotWorn()
        {
            DateTime start = DateTime.Now;
            IResult result = craftsman.Build(npc.Object, pc.Object, AvalableItemPosition.NotWorn, 1, "keyword", "sentenceDescription", "shortDescription", "longDescription", "examineDescription", DamageType.Pierce);

            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("", result.ResultMessage);
        }

        [TestMethod]
        public void CraftsMan_Process()
        {
            Assert.AreEqual("abc", craftsman.Process(npc.Object, "abc"));
        }
    }
}