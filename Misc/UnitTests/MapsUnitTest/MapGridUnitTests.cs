//using Maps;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Objects.Room.Interface;
//using Objects.Zone.Interface;
//using System.Collections.Generic;

//namespace MapsUnitTest
//{
//    [TestClass]
//    public class MapGridUnitTests
//    {
//        [TestMethod]
//        public void BuildMapMigrateEast()
//        {
//            #region TestSetup
//            Mock<IZone> zone = new Mock<IZone>();
//            Mock<IRoom> room1 = new Mock<IRoom>();
//            Mock<IRoom> room2 = new Mock<IRoom>();
//            Mock<IRoom> room3 = new Mock<IRoom>();
//            Mock<IRoom> room4 = new Mock<IRoom>();
//            Mock<IRoom> room5 = new Mock<IRoom>();

//            Mock<IExit> exit1 = new Mock<IExit>();
//            Mock<IExit> exit2 = new Mock<IExit>();
//            Mock<IExit> exit3 = new Mock<IExit>();
//            Mock<IExit> exit4 = new Mock<IExit>();
//            Mock<IExit> exit5 = new Mock<IExit>();

//            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

//            rooms.Add(1, room1.Object);
//            rooms.Add(2, room2.Object);
//            rooms.Add(3, room3.Object);
//            rooms.Add(4, room4.Object);
//            rooms.Add(5, room5.Object);


//            zone.Setup(e => e.Id).Returns(1);
//            zone.Setup(e => e.Rooms).Returns(rooms);

//            room1.Setup(e => e.North).Returns(exit1.Object);
//            room2.Setup(e => e.East).Returns(exit2.Object);
//            room3.Setup(e => e.South).Returns(exit3.Object);
//            room4.Setup(e => e.West).Returns(exit4.Object);
//            room5.Setup(e => e.West).Returns(exit5.Object);

//            room1.Setup(e => e.Zone).Returns(1);
//            room2.Setup(e => e.Zone).Returns(1);
//            room3.Setup(e => e.Zone).Returns(1);
//            room4.Setup(e => e.Zone).Returns(1);
//            room5.Setup(e => e.Zone).Returns(1);

//            exit1.Setup(e => e.Room).Returns(2);
//            exit2.Setup(e => e.Room).Returns(3);
//            exit3.Setup(e => e.Room).Returns(4);
//            exit4.Setup(e => e.Room).Returns(5);
//            exit5.Setup(e => e.Room).Returns(1);

//            exit1.Setup(e => e.Zone).Returns(1);
//            exit2.Setup(e => e.Zone).Returns(1);
//            exit3.Setup(e => e.Zone).Returns(1);
//            exit4.Setup(e => e.Zone).Returns(1);
//            exit5.Setup(e => e.Zone).Returns(1);



//            #endregion TestSetup

//            //MapGrid grid = new MapGrid();
//            //grid.BuildRooms(zone.Object);
//        }
//    }
//}
