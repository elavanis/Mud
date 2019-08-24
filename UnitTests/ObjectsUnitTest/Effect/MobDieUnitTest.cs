using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Effect;
using Objects.Mob.Interface;
using Moq;
using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language.Interface;
using Objects.Global.Notify.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class MobDieUnitTest
    {
        MobDie mobDie;
        Mock<IPlayerCharacter> pc;
        Mock<IEffectParameter> parameter;
        Mock<ITranslationMessage> translationMessage;
        Mock<INotify> notify;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            mobDie = new MobDie();
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            translationMessage = new Mock<ITranslationMessage>();
            notify = new Mock<INotify>();

            parameter.Setup(e => e.TargetMessage).Returns(translationMessage.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
            translationMessage.Setup(e => e.GetTranslatedMessage(null)).Returns("test message");

            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void MobDie_ProcessEffect_NormalMob()
        {
            mobDie.ProcessEffect(parameter.Object);

            pc.Verify(e => e.Die(null), Times.Once);
            notify.Verify(e => e.Mob(pc.Object, translationMessage.Object));
        }

        [TestMethod]
        public void MobDie_ProcessEffect_God()
        {
            pc.Setup(e => e.God).Returns(true);

            mobDie.ProcessEffect(parameter.Object);

            pc.Verify(e => e.Die(null), Times.Never);
        }
    }
}
