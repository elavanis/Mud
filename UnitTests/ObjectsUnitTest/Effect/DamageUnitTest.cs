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
using Objects.Global.Notify.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class DamageUnitTest
    {
        Objects.Effect.Damage damage;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IEffectParameter> parameter;
        Mock<ISound> sound;
        Mock<ITranslationMessage> translationMessage;
        Mock<ITagWrapper> tagWrapper;
        Mock<INotify> notify;
        Mock<IDamage> mockDamage;
        Mock<IDice> dice;
        Mock<ISerialization> serialization;
        Mock<ICombat> combat;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        string damageDescription;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            sound = new Mock<ISound>();
            damage = new Objects.Effect.Damage(sound.Object);
            performer = new Mock<IMobileObject>();
            target = new Mock<IMobileObject>();
            parameter = new Mock<IEffectParameter>();
            translationMessage = new Mock<ITranslationMessage>();
            notify = new Mock<INotify>();
            mockDamage = new Mock<IDamage>();
            dice = new Mock<IDice>();
            serialization = new Mock<ISerialization>();
            combat = new Mock<ICombat>();
            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            tagWrapper = new Mock<ITagWrapper>();
            damageDescription = "miscDamage";

            parameter.Setup(e => e.TargetMessage).Returns(translationMessage.Object);
            parameter.Setup(e => e.Damage).Returns(mockDamage.Object);
            parameter.Setup(e => e.Target).Returns(target.Object);
            parameter.Setup(e => e.Performer).Returns(performer.Object);
            parameter.Setup(e => e.Description).Returns(damageDescription);
            translationMessage.Setup(e => e.GetTranslatedMessage(null)).Returns("test message");
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Sound)).Returns((string x, TagType y) => (x));
            mockDamage.Setup(e => e.Dice).Returns(dice.Object);
            dice.Setup(e => e.RollDice()).Returns(1);
            engine.Setup(e => e.Combat).Returns(combat.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            serialization.Setup(e => e.Serialize(It.IsAny<List<ISound>>())).Returns("abc");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.Serialization = serialization.Object;
            GlobalReference.GlobalValues.Engine = engine.Object;
        }

        [TestMethod]
        public void Damage_Constructor()
        {
            Assert.AreSame(sound.Object, damage.Sound);
        }

        [TestMethod]
        public void Damage_ProcessEffect()
        {
            damage.ProcessEffect(parameter.Object);

            target.Verify(e => e.TakeDamage(1, mockDamage.Object, damageDescription), Times.Once, null);
            tagWrapper.Verify(e => e.WrapInTag("abc", TagType.Sound));
            notify.Verify(e => e.Mob(performer.Object, target.Object, target.Object, translationMessage.Object, false, false));
            serialization.Verify(e => e.Serialize(new List<ISound>() { sound.Object }));
            notify.Verify(e => e.Mob(target.Object, It.Is<ITranslationMessage>(f => f.Message == "abc")));
            combat.Verify(e => e.AddCombatPair(performer.Object, target.Object));
            evnt.Verify(e => e.DamageAfterDefense(performer.Object, target.Object, 0, damageDescription));
        }
    }
}