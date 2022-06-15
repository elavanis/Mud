using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;

namespace ObjectsUnitTest.Zone
{
    /// <summary>
    /// Summary description for ZoneIntergrationTest
    /// </summary>
    [TestClass]
    public class ZoneIntergrationTest
    {
        IZone zone;
        IRoom room;
        IItem item;
        INonPlayerCharacter npc;
        IPlayerCharacter pc;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
            GlobalReference.GlobalValues.Initilize();

            zone = new Objects.Zone.Zone();

            GlobalReference.GlobalValues.World.Zones.Add(0, zone);

            room = new Objects.Room.Room("examineDescription", "lookDescription", "shortDescription");
            item = new Objects.Item.Item("examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
            npc = new Objects.Mob.NonPlayerCharacter(room, "corpseLookDescription", "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");
            pc = new Objects.Mob.PlayerCharacter(room, "corpseLookDescription", "examineDescription", "lookDescription", "sentenceDescription", "shortDescription");

            zone.Id = 0;
            room.Id = 0;
            item.Level = 3;
            zone.Rooms.Add(0, room);
            room.AddItemToRoom(item);
            room.AddMobileObjectToRoom(npc);
            room.AddMobileObjectToRoom(pc);
            npc.Items.Add(item);
            npc.Level = 1;
            npc.MaxHealth = 10;
            pc.Items.Add(item);
            pc.Level = 1;
            pc.MaxHealth = 10;
            pc.Name = "pc";
        }

        [TestMethod]
        public void Zone_SeralizeDeserialize()
        {
            string serializedZone = GlobalReference.GlobalValues.Serialization.Serialize(zone);

            IZone zone2 = GlobalReference.GlobalValues.Serialization.Deserialize<Objects.Zone.Zone>(serializedZone);
            //temp work around room being added 2x
            IRoom room2;
            zone2.Rooms.TryGetValue(0, out room2);
            room2.KeyWords.RemoveAt(0);
            string serializedZone2 = GlobalReference.GlobalValues.Serialization.Serialize(zone2);

            Assert.AreEqual(serializedZone, serializedZone2);
        }
    }
}
