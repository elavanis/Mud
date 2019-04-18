using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Objects.Global.Logging.Interface;
using Objects.Global;
using static Objects.Global.Logging.LogSettings;
using Objects.Interface;

namespace ObjectsUnitTest.Effect
{
    [TestClass]
    public class SetRespawnPoint
    {
        Objects.Effect.SetRespawnPoint respawn;
        Mock<IPlayerCharacter> pc;
        Mock<IEffectParameter> parameter;
        Mock<IBaseObjectId> roomId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            respawn = new Objects.Effect.SetRespawnPoint();
            pc = new Mock<IPlayerCharacter>();
            parameter = new Mock<IEffectParameter>();
            roomId = new Mock<IBaseObjectId>();

            parameter.Setup(e => e.ObjectId).Returns(roomId.Object);
            parameter.Setup(e => e.Target).Returns(pc.Object);
        }

        [TestMethod]
        public void SetRespawnPoint_ProcessEffect()
        {
            Mock<ILogger> logger = new Mock<ILogger>();

            GlobalReference.GlobalValues.Logger = logger.Object;

            respawn.ProcessEffect(parameter.Object);

            pc.VerifySet(e => e.RespawnPoint = roomId.Object, Times.Once);
            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, " respawn point was reset to ."));
        }
    }
}
