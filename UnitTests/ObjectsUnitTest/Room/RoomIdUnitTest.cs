using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Objects.Room;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Room
{
    [TestClass]
    public class RoomIdUnitTest
    {
        RoomId roomId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            roomId = new RoomId();
        }

        [TestMethod]
        public void Room_ToString()
        {
            string result = roomId.ToString();

            Assert.AreEqual("0-0", result);
        }

        [TestMethod]
        public void RoomId_Constructor_Ints()
        {
            roomId = new RoomId(1, 2);
            Assert.AreEqual(1, roomId.Zone);
            Assert.AreEqual(2, roomId.Id);
        }

        [TestMethod]
        public void RoomId_Constructor_Room()
        {
            IRoom room = new Objects.Room.Room() { Zone = 1, Id = 2 };
            roomId = new RoomId(room);

            Assert.AreEqual(1, roomId.Zone);
            Assert.AreEqual(2, roomId.Id);
        }
    }
}
