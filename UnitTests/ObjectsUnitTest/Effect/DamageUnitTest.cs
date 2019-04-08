using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Global;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Shared.Sound.Interface;
using System.Collections.Generic;
using Objects.Language.Interface;
using Objects.Global.Serialization.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class DamageUnitTest
    {
        Objects.Effect.Damage damage;
        Mock<IPlayerCharacter> pc;
        Mock<IEffectParameter> parameter;
        Mock<ISound> sound;
        Mock<ITranslationMessage> translationMessage;
        Mock<ITagWrapper> tagWrapper;

        [TestInitialize]
        public void Setup()
        {
            sound = new Mock<ISound>();
            damage = new Objects.Effect.Damage(sound.Object);
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            translationMessage = new Mock<ITranslationMessage>();

            parameter.Setup(e => e.TargetMessage).Returns(translationMessage.Object);
            translationMessage.Setup(e => e.GetTranslatedMessage(null)).Returns("test message");
            tagWrapper = new Mock<ITagWrapper>();

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        [TestMethod]
        public void Damage_Constructor()
        {
            Assert.AreSame(sound.Object, damage.Sound);
        }

        [TestMethod]
        public void Damage_ProcessEffect()
        {
            Mock<IDamage> mockDamage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();
            Mock<ISerialization> serialization = new Mock<ISerialization>();

            mockDamage.Setup(e => e.Dice).Returns(dice.Object);
            dice.Setup(e => e.RollDice()).Returns(1);
            parameter.Setup(e => e.Damage).Returns(mockDamage.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
            serialization.Setup(e => e.Serialize(It.IsAny<List<ISound>>())).Returns("abc");

            GlobalReference.GlobalValues.Serialization = serialization.Object;

            damage.ProcessEffect(parameter.Object);

            pc.Verify(e => e.TakeDamage(1, mockDamage.Object, null), Times.Once, null);
            tagWrapper.Verify(e => e.WrapInTag("abc", TagType.Sound));
        }
    }
}
