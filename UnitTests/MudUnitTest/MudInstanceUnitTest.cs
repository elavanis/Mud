using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global.Interface;
using Mud;
using Objects.Global;
using Objects.Global.Logging.Interface;
using static Objects.Global.Logging.LogSettings;
using Objects.World.Interface;
using System.Reflection;
using Mud.Interface;
using Objects.Global.TickTimes.Interface;
using System.Threading;

namespace MudUnitTest
{
    [TestClass]
    public class MudInstanceUnitTest
    {
        MudInstance mud;

        [TestInitialize]
        public void Setup()
        {
            mud = new MudInstance();
        }

        [TestMethod]
        public void MudInstance_StartMud()
        {
            Mock<IGlobalValues> globalValues = new Mock<IGlobalValues>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            globalValues.Setup(e => e.Logger).Returns(logger.Object);
            globalValues.Setup(e => e.World).Returns(world.Object);
            globalValues.Setup(e => e.TickTimes).Returns(tickTimes.Object);

            GlobalReference.GlobalValues = globalValues.Object;

            mud.StartMud();

            logger.Verify(e => e.Log(LogLevel.INFO, "Loading World"), Times.Once);
            world.Verify(e => e.LoadWorld(), Times.Once);
            logger.Verify(e => e.Log(LogLevel.INFO, "Starting Heartbeat"), Times.Once);
        }

        [TestMethod]
        public void MudInstance_StartMud_NonCommunicationTick()
        {
            Mock<IGlobalValues> globalValues = new Mock<IGlobalValues>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            globalValues.Setup(e => e.Logger).Returns(logger.Object);
            globalValues.Setup(e => e.World).Returns(world.Object);
            globalValues.Setup(e => e.TickTimes).Returns(tickTimes.Object);

            GlobalReference.GlobalValues = globalValues.Object;
            GlobalReference.GlobalValues.TickCounter = 1;

            mud.StartMud();

            Thread.Sleep(200); //allow time for heartbeat code to finish

            mud.StopMud(null);

            logger.Verify(e => e.Log(LogLevel.INFO, "Loading World"), Times.Once);
            world.Verify(e => e.LoadWorld(), Times.Once);
            logger.Verify(e => e.Log(LogLevel.INFO, "Starting Heartbeat"), Times.Once);
        }

        [TestMethod]
        public void MudInstance_StopMud()
        {
            Thread thread = new Thread(() =>
            {

            });
            thread.Start();

            PropertyInfo pi = mud.GetType().GetProperty("_heartBeat", BindingFlags.Instance | BindingFlags.NonPublic);
            Mock<IHeartBeat> heartBeat = new Mock<IHeartBeat>();
            pi.SetValue(mud, heartBeat.Object);

            pi = mud.GetType().GetProperty("_heartBeatThread", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(mud, thread);

            Mock<IGlobalValues> globalValues = new Mock<IGlobalValues>();
            Mock<ILogger> logger = new Mock<ILogger>();

            globalValues.Setup(e => e.Logger).Returns(logger.Object);

            GlobalReference.GlobalValues = globalValues.Object;

            mud.StopMud(null);

            heartBeat.Verify(e => e.StopHeartBeat(), Times.Once);
            logger.Verify(e => e.FlushLogs(), Times.Once);
        }
    }
}
