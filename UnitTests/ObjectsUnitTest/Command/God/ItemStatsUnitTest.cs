using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Command.God;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using System.Text.RegularExpressions;
using Objects.Item.Items.Interface;
using Objects.Die.Interface;
using static Objects.Item.Item;
using Objects.Magic.Interface;
using static Objects.Damage.Damage;
using Objects.Damage.Interface;
using static Objects.Global.Stats.Stats;
using System.Linq;
using Objects.Global.FindObjects.Interface;

namespace ObjectsUnitTest.Command.God
{
    [TestClass]
    public class ItemStatsUnitTest
    {
        Mock<IMobileObject> mob;
        Mock<ICommand> commandMock;
        IMobileObjectCommand command;
        Mock<IFindObjects> findThings;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            command = new ItemStats();
            mob = new Mock<IMobileObject>();
            findThings = new Mock<IFindObjects>();
            findThings.Setup(e => e.FindHeldItemsOnMob(mob.Object, "notFound", 0)).Returns<IItem>(null);
            GlobalReference.GlobalValues.FindObjects = findThings.Object;
            commandMock = new Mock<ICommand>();
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("item");
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });
        }

        [TestMethod]
        public void ItemStats_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("ItemStats [Item Keyword]", result.ResultMessage);
        }

        [TestMethod]
        public void ItemStats_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("ItemStats"));
        }

        [TestMethod]
        public void ItemStats_PerformCommand_NoParameter()
        {
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("What item would you like to get stats on?", result.ResultMessage);
        }

        [TestMethod]
        public void ItemStats_PerformCommand_ItemNotFound()
        {
            Mock<IParameter> parm = new Mock<IParameter>();
            parm.Setup(e => e.ParameterValue).Returns("notFound");
            parm.Setup(e => e.ParameterNumber).Returns(2);
            commandMock.Setup(e => e.Parameters).Returns(new List<IParameter>() { parm.Object });

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You do not seem to be holding notFound[2].", result.ResultMessage);
        }

        [TestMethod]
        public void ItemStats_PerformCommand_Item()
        {
            Mock<IItem> item = new Mock<IItem>();
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            item.Setup(e => e.ZoneId).Returns(1);
            item.Setup(e => e.Id).Returns(2);
            item.Setup(e => e.Level).Returns(3);
            item.Setup(e => e.Weight).Returns(4);
            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            item.Setup(e => e.LookDescription).Returns("LookDescription");
            item.Setup(e => e.ExamineDescription).Returns("ExamineDescription");
            findThings.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Regex regex = new Regex(@"SentenceDescription\r\nItem Type: ObjectProxy_\d+\r\nZone: 1\r\nId: 2\r\nLevel: 3\r\nKeywords: keyword\r\nSentenceDescription: SentenceDescription\r\nShortDescription: ShortDescription\r\nLookDescription: LookDescription\r\nExamineDescription: ExamineDescription\r\nWeight :4");
            Assert.IsTrue(regex.Match(result.ResultMessage).Success);
        }

        [TestMethod]
        public void ItemStats_PerformCommand_Armor()
        {
            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.Die).Returns(100);
            dice.Setup(e => e.Sides).Returns(101);

            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            enchantment.Setup(e => e.ToString()).Returns("enchantment");

            Mock<IShield> item = new Mock<IShield>();
            int uniqueNumber = 1;
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            item.Setup(e => e.ZoneId).Returns(uniqueNumber++);
            item.Setup(e => e.Id).Returns(uniqueNumber++);
            item.Setup(e => e.Level).Returns(uniqueNumber++);
            item.Setup(e => e.Weight).Returns(uniqueNumber++);
            item.Setup(e => e.NegateDamagePercent).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Slash)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Pierce)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Bludgeon)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Acid)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Fire)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Cold)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Poison)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Necrotic)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Radiant)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Lightning)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Psychic)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Thunder)).Returns(uniqueNumber++);
            item.Setup(e => e.GetTypeModifier(DamageType.Force)).Returns(uniqueNumber++);
            item.Setup(e => e.Strength).Returns(uniqueNumber++);
            item.Setup(e => e.Dexterity).Returns(uniqueNumber++);
            item.Setup(e => e.Constitution).Returns(uniqueNumber++);
            item.Setup(e => e.Intelligence).Returns(uniqueNumber++);
            item.Setup(e => e.Wisdom).Returns(uniqueNumber++);
            item.Setup(e => e.Charisma).Returns(uniqueNumber++);
            item.Setup(e => e.MaxHealth).Returns(uniqueNumber++);
            item.Setup(e => e.MaxMana).Returns(uniqueNumber++);
            item.Setup(e => e.MaxStamina).Returns(uniqueNumber++);

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            item.Setup(e => e.LookDescription).Returns("LookDescription");
            item.Setup(e => e.ExamineDescription).Returns("ExamineDescription");
            item.Setup(e => e.Dice).Returns(dice.Object);
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.Light });
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            findThings.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Regex regex = new Regex(@"SentenceDescription\r\nItem\ Type:\ ObjectProxy_\d+\r\nZone:\ 1\r\nId:\ 2\r\nLevel:\ 3\r\nKeywords:\ keyword\r\nSentenceDescription:\ SentenceDescription\r\nShortDescription:\ ShortDescription\r\nLookDescription:\ LookDescription\r\nExamineDescription:\ ExamineDescription\r\nWeight\ :4\r\nShieldNegateDamagePercent:\ 5\r\nDie:\ 100\r\nSides:\ 101\r\nNotSet:\ 0\r\nSlash:\ 6\r\nPierce:\ 7\r\nBludgeon:\ 8\r\nAcid:\ 9\r\nFire:\ 10\r\nCold:\ 11\r\nPoison:\ 12\r\nNecrotic:\ 13\r\nRadiant:\ 14\r\nLightning:\ 15\r\nPsychic:\ 16\r\nThunder:\ 17\r\nForce:\ 18\r\nStrength:\ 19\r\nDexterity:\ 20\r\nConstitution:\ 21\r\nIntelligence:\ 22\r\nWisdom:\ 23\r\nCharisma:\ 24\r\nMaxHealth:\ 25\r\nMaxMana:\ 26\r\nMaxStamina:\ 27\r\nItemPosition:\ NotSet\r\nAttributes:\ Light\r\nEnchantments:\ enchantment");
            Assert.IsTrue(regex.Match(result.ResultMessage).Success);
        }

        [TestMethod]
        public void ItemStats_PerformCommand_Weapon()
        {
            Mock<IDice> dice = new Mock<IDice>();
            dice.Setup(e => e.Die).Returns(100);
            dice.Setup(e => e.Sides).Returns(101);

            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();
            enchantment.Setup(e => e.ToString()).Returns("enchantment");

            Mock<IDamage> damage = new Mock<IDamage>();
            damage.Setup(e => e.Dice).Returns(dice.Object);
            damage.Setup(e => e.Type).Returns(DamageType.Fire);
            damage.Setup(e => e.BonusDamageStat).Returns(Stat.Strength);
            damage.Setup(e => e.BonusDefenseStat).Returns(Stat.Dexterity);

            Mock<IWeapon> item = new Mock<IWeapon>();
            int uniqueNumber = 1;
            item.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            item.Setup(e => e.ZoneId).Returns(uniqueNumber++);
            item.Setup(e => e.Id).Returns(uniqueNumber++);
            item.Setup(e => e.Level).Returns(uniqueNumber++);
            item.Setup(e => e.Weight).Returns(uniqueNumber++);
            item.Setup(e => e.Strength).Returns(uniqueNumber++);
            item.Setup(e => e.Dexterity).Returns(uniqueNumber++);
            item.Setup(e => e.Constitution).Returns(uniqueNumber++);
            item.Setup(e => e.Intelligence).Returns(uniqueNumber++);
            item.Setup(e => e.Wisdom).Returns(uniqueNumber++);
            item.Setup(e => e.Charisma).Returns(uniqueNumber++);
            item.Setup(e => e.MaxHealth).Returns(uniqueNumber++);
            item.Setup(e => e.MaxMana).Returns(uniqueNumber++);
            item.Setup(e => e.MaxStamina).Returns(uniqueNumber++);

            item.Setup(e => e.SentenceDescription).Returns("SentenceDescription");
            item.Setup(e => e.ShortDescription).Returns("ShortDescription");
            item.Setup(e => e.LookDescription).Returns("LookDescription");
            item.Setup(e => e.ExamineDescription).Returns("ExamineDescription");
            item.Setup(e => e.Attributes).Returns(new List<ItemAttribute>() { ItemAttribute.Light });
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            item.Setup(e => e.DamageList).Returns(new List<IDamage>() { damage.Object });

            findThings.Setup(e => e.FindHeldItemsOnMob(mob.Object, "item", 0)).Returns(item.Object);

            IResult result = command.PerformCommand(mob.Object, commandMock.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Regex regex = new Regex(@"SentenceDescription\r\nItem\ Type:\ ObjectProxy_\d+\r\nZone:\ 1\r\nId:\ 2\r\nLevel:\ 3\r\nKeywords:\ keyword\r\nSentenceDescription:\ SentenceDescription\r\nShortDescription:\ ShortDescription\r\nLookDescription:\ LookDescription\r\nExamineDescription:\ ExamineDescription\r\nWeight\ :4\r\nDamageType:\ Fire\r\nDie:\ 100\r\nSides:\ 101\r\nBonusDamageStat:\ Strength\r\nBonusDefenseStat:\ Dexterity\r\nStrength:\ 5\r\nDexterity:\ 6\r\nConstitution:\ 7\r\nIntelligence:\ 8\r\nWisdom:\ 9\r\nCharisma:\ 10\r\nMaxHealth:\ 11\r\nMaxMana:\ 12\r\nMaxStamina:\ 13\r\nItemPosition:\ NotSet\r\nAttributes:\ Light\r\nEnchantments:\ enchantment");
            Assert.IsTrue(regex.Match(result.ResultMessage).Success);
        }
    }
}
