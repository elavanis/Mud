using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using System.Collections.Generic;
using Objects.Room.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Item.Items.Interface;
using Objects.Interface;
using Objects.Global.FindObjects;

namespace ObjectsUnitTest.Global.FindThings
{
    [TestClass]
    public class FindObjectsUnitTest
    {
        FindObjects findObjects;

        [TestInitialize]
        public void Setup()
        {
            findObjects = new FindObjects();
        }

        [TestMethod]
        public void FindObjects_FindHeldItemsOnMobSpecificNumber()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "target" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(items);

            IItem result = findObjects.FindHeldItemsOnMob(mob.Object, "target", 1);

            Assert.AreSame(item2.Object, result);
        }

        [TestMethod]
        public void FindObjects_FindHeldItemsOnMobSpecificNumberNotFound()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "target" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(items);

            IItem result = findObjects.FindHeldItemsOnMob(mob.Object, "target", 2);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindObjects_FindHeldItemsOnMob()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "notTarget" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Items).Returns(items);

            List<IItem> result = findObjects.FindHeldItemsOnMob(mob.Object, "target");

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(item.Object, result[0]);
        }

        [TestMethod]
        public void FindObjects_FindItemsInRoomSpecificNumber()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "target" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(items);

            IItem result = findObjects.FindItemsInRoom(room.Object, "target", 1);

            Assert.AreSame(item2.Object, result);
        }

        [TestMethod]
        public void FindObjects_FindItemsInRoomSpecificNumberNotFound()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "target" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(items);

            IItem result = findObjects.FindItemsInRoom(room.Object, "target", 2);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindObjects_FindItemsInRoom()
        {
            List<IItem> items = new List<IItem>();
            Mock<IItem> item = new Mock<IItem>();
            List<string> keywords = new List<string>() { "target" };
            item.Setup(e => e.KeyWords).Returns(keywords);
            items.Add(item.Object);

            Mock<IItem> item2 = new Mock<IItem>();
            List<string> keywords2 = new List<string>() { "notTarget" };
            item2.Setup(e => e.KeyWords).Returns(keywords2);
            items.Add(item2.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(items);

            List<IItem> result = findObjects.FindItemsInRoom(room.Object, "target");

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(item.Object, result[0]);
        }

        [TestMethod]
        public void FindObjects_FindNpcInRoom()
        {
            List<INonPlayerCharacter> npcs = new List<INonPlayerCharacter>();
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            List<string> keywords = new List<string>() { "target" };
            npc.Setup(e => e.KeyWords).Returns(keywords);
            npcs.Add(npc.Object);

            Mock<INonPlayerCharacter> npc2 = new Mock<INonPlayerCharacter>();
            List<string> keywords2 = new List<string>() { "notTarget" };
            npc2.Setup(e => e.KeyWords).Returns(keywords2);
            npcs.Add(npc2.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.NonPlayerCharacters).Returns(npcs);

            List<INonPlayerCharacter> result = findObjects.FindNpcInRoom(room.Object, "target");

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(npc.Object, result[0]);
        }

        [TestMethod]
        public void FindObjects_FindPcInRoom()
        {
            List<IPlayerCharacter> pcs = new List<IPlayerCharacter>();
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            List<string> keywords = new List<string>() { "target" };
            pc.Setup(e => e.KeyWords).Returns(keywords);
            pcs.Add(pc.Object);

            Mock<IPlayerCharacter> pcs2 = new Mock<IPlayerCharacter>();
            List<string> keywords2 = new List<string>() { "notTarget" };
            pcs2.Setup(e => e.KeyWords).Returns(keywords2);
            pcs.Add(pcs2.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.PlayerCharacters).Returns(pcs);

            List<IPlayerCharacter> result = findObjects.FindPcInRoom(room.Object, "target");

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(pc.Object, result[0]);
        }

        [TestMethod]
        public void FindObjects_DetermineFoundObjectTagType_IItem()
        {
            Mock<IItem> obj = new Mock<IItem>();
            Assert.AreEqual(TagType.Item, findObjects.DetermineFoundObjectTagType(obj.Object));
        }

        [TestMethod]
        public void FindObjects_DetermineFoundObjectTagType_INonplayerCharacter()
        {
            Mock<INonPlayerCharacter> obj = new Mock<INonPlayerCharacter>();
            Assert.AreEqual(TagType.NonPlayerCharacter, findObjects.DetermineFoundObjectTagType(obj.Object));
        }

        [TestMethod]
        public void FindObjects_DetermineFoundObjectTagType_IPlayerCharacter()
        {
            Mock<IPlayerCharacter> obj = new Mock<IPlayerCharacter>();
            Assert.AreEqual(TagType.PlayerCharacter, findObjects.DetermineFoundObjectTagType(obj.Object));
        }

        [TestMethod]
        public void FindObjects_DetermineFoundObjectTagType_Info()
        {
            Mock<IRoom> obj = new Mock<IRoom>();
            Assert.AreEqual(TagType.Info, findObjects.DetermineFoundObjectTagType(obj.Object));
        }

        [TestMethod]
        public void FindObjects_FindObjectOnPersonOrInRoom_ItemOnPerson()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IItem> onPerson = new Mock<IItem>();
            onPerson.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IItem> inRoom = new Mock<IItem>();
            inRoom.Setup(e => e.KeyWords).Returns(keywords);
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.KeyWords).Returns(keywords);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>() { inRoom.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { onPerson.Object });

            Assert.AreSame(onPerson.Object, findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 0));
        }

        [TestMethod]
        public void FindObjects_FindObjectOnPersonOrInRoom_ItemInRoom()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IItem> onPerson = new Mock<IItem>();
            onPerson.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IItem> inRoom = new Mock<IItem>();
            inRoom.Setup(e => e.KeyWords).Returns(keywords);
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.KeyWords).Returns(keywords);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>() { inRoom.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { onPerson.Object });

            Assert.AreSame(inRoom.Object, findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 1));
        }

        [TestMethod]
        public void FindObjects_FindObjectOnPersonOrInRoom_Npc()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IItem> onPerson = new Mock<IItem>();
            onPerson.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IItem> inRoom = new Mock<IItem>();
            inRoom.Setup(e => e.KeyWords).Returns(keywords);
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.KeyWords).Returns(keywords);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>() { inRoom.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { onPerson.Object });

            Assert.AreSame(npc.Object, findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 2));
        }

        [TestMethod]
        public void FindObjects_FindObjectOnPersonOrInRoom_Pc()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IItem> onPerson = new Mock<IItem>();
            onPerson.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IItem> inRoom = new Mock<IItem>();
            inRoom.Setup(e => e.KeyWords).Returns(keywords);
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.KeyWords).Returns(keywords);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>() { inRoom.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { onPerson.Object });

            Assert.AreSame(pc.Object, findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 3));
        }

        [TestMethod]
        public void FindObjects_FindObjectOnPersonOrInRoom_NotFound()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IDoor> door = new Mock<IDoor>();
            door.Setup(e => e.KeyWords).Returns(new List<string>() { "keywords" });
            Mock<IExit> exit = new Mock<IExit>();
            exit.Setup(e => e.Door).Returns(door.Object);
            Mock<IItem> onPerson = new Mock<IItem>();
            onPerson.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IItem> inRoom = new Mock<IItem>();
            inRoom.Setup(e => e.KeyWords).Returns(keywords);
            Mock<INonPlayerCharacter> npc = new Mock<INonPlayerCharacter>();
            npc.Setup(e => e.KeyWords).Returns(keywords);
            Mock<IPlayerCharacter> pc = new Mock<IPlayerCharacter>();
            pc.Setup(e => e.KeyWords).Returns(keywords);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>() { inRoom.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.North).Returns(exit.Object);
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>() { onPerson.Object });

            Assert.AreSame(null, findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 10));
        }

        [TestMethod]
        public void FindObjects_FindObjectsOnPersonOrInRoom_Doors()
        {
            List<string> keywords = new List<string>() { "keyword" };

            Mock<IDoor> door = new Mock<IDoor>();
            door.Setup(e => e.KeyWords).Returns(new List<string>() { "keyword" });
            Mock<IExit> exit = new Mock<IExit>();
            exit.Setup(e => e.Door).Returns(door.Object);

            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.Items).Returns(new List<IItem>());
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room.Setup(e => e.North).Returns(exit.Object);
            Mock<IMobileObject> mob = new Mock<IMobileObject>();
            mob.Setup(e => e.Room).Returns(room.Object);
            mob.Setup(e => e.Items).Returns(new List<IItem>());

            IBaseObject foundObject = findObjects.FindObjectOnPersonOrInRoom(mob.Object, "keyword", 0);
            Assert.AreSame(door.Object, foundObject);
        }


    }
}
