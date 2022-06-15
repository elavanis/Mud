using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Room;
using Objects.Room.Interface;
using Objects.World.Interface;

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
            Mock<IRoom> room = new Mock<IRoom>();
            room.Setup(e => e.ExamineDescription).Returns("examineDescription");
            room.Setup(e => e.LookDescription).Returns("lookDescription");
            room.Setup(e => e.ShortDescription).Returns("shortDescription");
            room.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.Id).Returns(2);
            
            roomId = new RoomId(room.Object);

            Assert.AreEqual(1, roomId.Zone);
            Assert.AreEqual(2, roomId.Id);
            Assert.AreEqual("examineDescription", room.Object.ExamineDescription);
            Assert.AreEqual("lookDescription", room.Object.LookDescription);
            Assert.AreEqual("shortDescription", room.Object.ShortDescription);
        }
    }
}
