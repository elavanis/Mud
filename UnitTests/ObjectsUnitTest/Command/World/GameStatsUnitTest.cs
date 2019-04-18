using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.World;
using Objects.Global;
using Objects.Global.UpTime.Interface;
using Moq;
using Objects.Global.TickTimes.Interface;
using Objects.World.Interface;
using System.Collections.Generic;
using Objects.Zone.Interface;
using Objects.Room.Interface;
using Objects.Mob.Interface;
using Objects.Item.Interface;

namespace ObjectsUnitTest.Command.World
{
    [TestClass]
    public class GameStatsUnitTest
    {

        GameStats gameStats;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            gameStats = new GameStats();
        }

        [TestMethod]
        public void GameStats_GenerateGameStats()
        {
            Mock<IUpTime> upTime = new Mock<IUpTime>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();
            Mock<IWorld> world = new Mock<IWorld>();
            Mock<IZone> zone1 = new Mock<IZone>();
            Mock<IZone> zone2 = new Mock<IZone>();
            Mock<IRoom> room = new Mock<IRoom>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            Mock<IItem> item = new Mock<IItem>();
            Dictionary<int, IZone> dictionaryZones = new Dictionary<int, IZone>();
            Dictionary<int, IRoom> dictionaryRoom1 = new Dictionary<int, IRoom>();
            Dictionary<int, IRoom> dictionaryRoom2 = new Dictionary<int, IRoom>();

            upTime.Setup(e => e.FormatedUpTime(It.IsAny<DateTime>())).Returns("uptime");
            tickTimes.Setup(e => e.Times).Returns("tickTimes");
            world.Setup(e => e.Zones).Returns(dictionaryZones);
            dictionaryZones.Add(1, zone1.Object);
            dictionaryZones.Add(2, zone2.Object);
            zone1.Setup(e => e.Id).Returns(1);
            zone1.Setup(e => e.Name).Returns("zone1");
            zone1.Setup(e => e.Rooms).Returns(dictionaryRoom1);
            dictionaryRoom1.Add(1, room.Object);
            zone2.Setup(e => e.Id).Returns(20);
            zone2.Setup(e => e.Name).Returns("z2");
            zone2.Setup(e => e.Rooms).Returns(dictionaryRoom2);
            for (int i = 0; i < 100; i++)
            {
                dictionaryRoom2.Add(i, room.Object);
            }
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });

            GlobalReference.GlobalValues.UpTime = upTime.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;
            GlobalReference.GlobalValues.World = world.Object;

            string result = gameStats.GenerateGameStats();
            string expected = "\r\nUpTime: uptime\r\ntickTimes\r\nId:1  - zone1 - Rooms:1   - Mobs:1   - Items:1   - Players:1  \r\nId:20 - z2    - Rooms:100 - Mobs:100 - Items:100 - Players:100";
            Assert.IsTrue(result.Contains(expected));
        }
    }
}
