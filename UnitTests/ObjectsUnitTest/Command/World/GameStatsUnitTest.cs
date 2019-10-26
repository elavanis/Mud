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
using Objects.GameDateTime.Interface;
using Objects.Global.GameDateTime.Interface;

namespace ObjectsUnitTest.Command.World
{
    [TestClass]
    public class GameStatsUnitTest
    {

        GameStats gameStats;
        Mock<IUpTime> upTime;
        Mock<ITickTimes> tickTimes;
        Mock<IWorld> world;
        Mock<IZone> zone1;
        Mock<IZone> zone2;
        Mock<IRoom> room;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IItem> item;
        Mock<IGameDateTime> gameDateTime;
        Mock<IInGameDateTime> inGameDateTime;
        Dictionary<int, IZone> dictionaryZones;
        Dictionary<int, IRoom> dictionaryRoom1;
        Dictionary<int, IRoom> dictionaryRoom2;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            gameStats = new GameStats();

            upTime = new Mock<IUpTime>();
            tickTimes = new Mock<ITickTimes>();
            world = new Mock<IWorld>();
            zone1 = new Mock<IZone>();
            zone2 = new Mock<IZone>();
            room = new Mock<IRoom>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            item = new Mock<IItem>();
            gameDateTime = new Mock<IGameDateTime>();
            inGameDateTime = new Mock<IInGameDateTime>();
            dictionaryZones = new Dictionary<int, IZone>();
            dictionaryRoom1 = new Dictionary<int, IRoom>();
            dictionaryRoom2 = new Dictionary<int, IRoom>();

            upTime.Setup(e => e.FormatedUpTime(It.IsAny<DateTime>())).Returns("uptime");
            tickTimes.Setup(e => e.Times).Returns("tickTimes");
            world.Setup(e => e.Zones).Returns(dictionaryZones);
            world.Setup(e => e.CurrentPlayers).Returns(new List<IPlayerCharacter>());
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
            inGameDateTime.Setup(e => e.GameDateTime).Returns(gameDateTime.Object);

            GlobalReference.GlobalValues.UpTime = upTime.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;
            GlobalReference.GlobalValues.World = world.Object;
            GlobalReference.GlobalValues.GameDateTime = inGameDateTime.Object;
        }

        [TestMethod]
        public void GameStats_GenerateGameStats()
        {
            string result = gameStats.GenerateGameStats();
            string expected = "\r\nUpTime: uptime\r\ntickTimes\r\nId:1  - zone1 - Rooms:1   - Mobs:1   - Items:1   - Players:1  \r\nId:20 - z2    - Rooms:100 - Mobs:100 - Items:100 - Players:100";
            Assert.IsTrue(result.Contains(expected));
        }
    }
}
