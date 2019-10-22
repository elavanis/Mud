using System;
using System.Collections.Generic;
using System.Text;
using MiscShared;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.UnderGrandView
{
    public class UnderGrandViewCastle : BaseZone, IZoneCode
    {
        public UnderGrandViewCastle() : base(25)
        {
        }

        //        public IZone Generate()
        //        {
        //            Zone.Name = nameof(UnderGrandViewCastle);

        //            BuildRoomsViaReflection(this.GetType());

        //            ConnectRooms();

        //            return Zone;
        //        }



        //        #region Rooms
        //        private IRoom GenerateRoom1()
        //        {
        //            IRoom room = IndoorRoomNoLight();

        //            room.ExamineDescription = "Dirt from above still occasionally falls down and one day may fill the tunnel.";
        //            room.LookDescription = "The tunnel was originally part of an underground sink hole network.";
        //            room.ShortDescription = "Underground cavern";

        //            return room;
        //        }

        //        private IRoom GenerateRoom2()
        //        {
        //            IRoom room = IndoorRoomNoLight();

        //            room.ExamineDescription = "The dirt from above has filled most of this cavern.  Several small tunnels lead off but most are impassable beyond a few feet.";
        //            room.LookDescription = "A large mound of dirt fills the cavern.";
        //            room.ShortDescription = "Underground cavern";

        //            return room;
        //        }

        //        private IRoom GenerateRoom3()
        //        {
        //            IRoom room = IndoorRoomNoLight();

        //            room.ExamineDescription = "The tunnel to the west appears to be natural or not finished while the tunnel to the east is lined with bricks.";
        //            room.LookDescription = "This point in the tunnel transitions between a rough natural tunnel and a brick lined tunnel with torches.";
        //            room.ShortDescription = "Underground cavern";

        //            return room;
        //        }

        //        #endregion Rooms

        //        private void ConnectRooms()
        //        {
        //            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
        //            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
        //        }
        //    }
        //}


        public IZone Generate()
        {
            Zone.Name = nameof(UnderGrandViewCastle);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom2()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom3()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom4()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom5()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom6()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom7()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom8()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom9()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom10()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom11()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom12()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom13()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom14()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom15()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom16()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom17()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom18()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom19()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom20()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom21()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom22()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom23()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom24()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom25()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom26()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom27()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom28()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom29()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom30()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom31()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom32()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom33()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom34()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom35()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom36()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom37()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom38()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom39()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom40()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom41()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom42()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom43()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom44()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "Aside immediate damage from the cave ins from the north the hallway seems in good shape.";
            room.LookDescription = "The hallway to the north ends a mere foot after it begins in a a collapsed ceiling.  The path to the east is also collapsing.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom45()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "The one statue that still stands has an inscription. \"Sir Malculms \"The Wall\"  Lost in the battle of widows while defiantly holding back a waves of advancing orcs so his troops could escape.";
            room.LookDescription = "Part of the north wall has caved in, held up only by one statue that did not break and has held the line.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom46()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "The faint sound of dirt can be heard falling into water through the crack in the floor.";
            room.LookDescription = "Small avalanches of dirt continue to slide down the mound before disappearing in a crack in the floor.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom47()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "Dirt from above still occasionally falls down and one day may fill the hallway.";
            room.LookDescription = "The ceiling to the hallway has collapsed and dirt from above has fallen in.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom48()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "The dirt from above has filled most of this cavern.  Several small tunnels lead off but most are impassable beyond a few feet.";
            room.LookDescription = "A large mound of dirt fills the cavern.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom49()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "Each statue is carved from stone and in a regal pose.";
            room.LookDescription = "The hallway is lined with statues of knights in their armor.";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom50()
        {
            IRoom room = IndoorRoomNoLight();
            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom51()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom52()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom53()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom54()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom55()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom56()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom57()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom58()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom59()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom60()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom61()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom62()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom63()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom64()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom65()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom66()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom67()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom68()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom69()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom70()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom71()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom72()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom73()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom74()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom75()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom76()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom77()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom78()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom79()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom80()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom81()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom82()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom83()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom84()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom85()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom86()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom87()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom88()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom89()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom90()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom91()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom92()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom93()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom94()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom95()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom96()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom97()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom98()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom99()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom100()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom101()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom102()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom103()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom104()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom105()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom106()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom107()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom108()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom109()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom110()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom111()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom112()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom113()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom114()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom115()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom116()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom117()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom118()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom119()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom120()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom121()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom122()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom123()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom124()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom125()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom126()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom127()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom128()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom129()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom130()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom131()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        private IRoom GenerateRoom132()
        {
            IRoom room = IndoorRoomNoLight();

            room.ExamineDescription = "";
            room.LookDescription = "";
            room.ShortDescription = "Underground cavern";

            return room;
        }
        #endregion Rooms

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.South, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.South, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.South, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.East, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.South, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.South, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.East, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[13], Direction.South, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.East, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.South, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.East, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.East, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.East, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.South, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[19], Direction.East, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.East, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.East, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.East, Zone.Rooms[24]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.South, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.South, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.East, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.South, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.East, Zone.Rooms[29]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.South, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[29], Direction.South, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.East, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.South, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.East, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.South, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.East, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.East, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.East, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.East, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[35], Direction.South, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.East, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.South, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.South, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.East, Zone.Rooms[39]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.South, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.East, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[39], Direction.South, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.South, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.East, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.South, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.East, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.South, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.South, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.East, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.South, Zone.Rooms[60]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.East, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[46], Direction.East, Zone.Rooms[47]);
            ZoneHelper.ConnectRoom(Zone.Rooms[47], Direction.East, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.East, Zone.Rooms[49]);
            ZoneHelper.ConnectRoom(Zone.Rooms[49], Direction.East, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.East, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.South, Zone.Rooms[61]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.East, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.South, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.East, Zone.Rooms[53]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.South, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.East, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[54], Direction.East, Zone.Rooms[55]);
            ZoneHelper.ConnectRoom(Zone.Rooms[55], Direction.East, Zone.Rooms[56]);
            ZoneHelper.ConnectRoom(Zone.Rooms[56], Direction.East, Zone.Rooms[57]);
            ZoneHelper.ConnectRoom(Zone.Rooms[57], Direction.East, Zone.Rooms[58]);
            ZoneHelper.ConnectRoom(Zone.Rooms[58], Direction.East, Zone.Rooms[59]);
            ZoneHelper.ConnectRoom(Zone.Rooms[59], Direction.South, Zone.Rooms[64]);
            ZoneHelper.ConnectRoom(Zone.Rooms[60], Direction.South, Zone.Rooms[66]);
            ZoneHelper.ConnectRoom(Zone.Rooms[61], Direction.East, Zone.Rooms[62]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.East, Zone.Rooms[63]);
            ZoneHelper.ConnectRoom(Zone.Rooms[62], Direction.South, Zone.Rooms[68]);
            ZoneHelper.ConnectRoom(Zone.Rooms[64], Direction.South, Zone.Rooms[69]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.East, Zone.Rooms[66]);
            ZoneHelper.ConnectRoom(Zone.Rooms[65], Direction.South, Zone.Rooms[70]);
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.East, Zone.Rooms[67]);
            ZoneHelper.ConnectRoom(Zone.Rooms[66], Direction.South, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[67], Direction.South, Zone.Rooms[72]);
            ZoneHelper.ConnectRoom(Zone.Rooms[68], Direction.South, Zone.Rooms[76]);
            ZoneHelper.ConnectRoom(Zone.Rooms[69], Direction.South, Zone.Rooms[80]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.East, Zone.Rooms[71]);
            ZoneHelper.ConnectRoom(Zone.Rooms[70], Direction.South, Zone.Rooms[81]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.East, Zone.Rooms[72]);
            ZoneHelper.ConnectRoom(Zone.Rooms[71], Direction.South, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[72], Direction.South, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.East, Zone.Rooms[74]);
            ZoneHelper.ConnectRoom(Zone.Rooms[73], Direction.South, Zone.Rooms[84]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.East, Zone.Rooms[75]);
            ZoneHelper.ConnectRoom(Zone.Rooms[74], Direction.South, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[75], Direction.South, Zone.Rooms[86]);
            ZoneHelper.ConnectRoom(Zone.Rooms[76], Direction.South, Zone.Rooms[88]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.East, Zone.Rooms[78]);
            ZoneHelper.ConnectRoom(Zone.Rooms[77], Direction.South, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.East, Zone.Rooms[79]);
            ZoneHelper.ConnectRoom(Zone.Rooms[78], Direction.South, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[79], Direction.South, Zone.Rooms[93]);
            ZoneHelper.ConnectRoom(Zone.Rooms[80], Direction.South, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[81], Direction.East, Zone.Rooms[82]);
            ZoneHelper.ConnectRoom(Zone.Rooms[82], Direction.East, Zone.Rooms[83]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.East, Zone.Rooms[85]);
            ZoneHelper.ConnectRoom(Zone.Rooms[84], Direction.South, Zone.Rooms[97]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.East, Zone.Rooms[86]);
            ZoneHelper.ConnectRoom(Zone.Rooms[85], Direction.South, Zone.Rooms[98]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.East, Zone.Rooms[87]);
            ZoneHelper.ConnectRoom(Zone.Rooms[86], Direction.South, Zone.Rooms[99]);
            ZoneHelper.ConnectRoom(Zone.Rooms[87], Direction.East, Zone.Rooms[88]);
            ZoneHelper.ConnectRoom(Zone.Rooms[88], Direction.East, Zone.Rooms[89]);
            ZoneHelper.ConnectRoom(Zone.Rooms[88], Direction.South, Zone.Rooms[100]);
            ZoneHelper.ConnectRoom(Zone.Rooms[89], Direction.East, Zone.Rooms[90]);
            ZoneHelper.ConnectRoom(Zone.Rooms[90], Direction.East, Zone.Rooms[91]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.East, Zone.Rooms[92]);
            ZoneHelper.ConnectRoom(Zone.Rooms[91], Direction.South, Zone.Rooms[101]);
            ZoneHelper.ConnectRoom(Zone.Rooms[92], Direction.East, Zone.Rooms[93]);
            ZoneHelper.ConnectRoom(Zone.Rooms[92], Direction.South, Zone.Rooms[102]);
            ZoneHelper.ConnectRoom(Zone.Rooms[93], Direction.South, Zone.Rooms[103]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.East, Zone.Rooms[95]);
            ZoneHelper.ConnectRoom(Zone.Rooms[94], Direction.South, Zone.Rooms[104]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.East, Zone.Rooms[96]);
            ZoneHelper.ConnectRoom(Zone.Rooms[95], Direction.South, Zone.Rooms[105]);
            ZoneHelper.ConnectRoom(Zone.Rooms[96], Direction.South, Zone.Rooms[106]);
            ZoneHelper.ConnectRoom(Zone.Rooms[97], Direction.East, Zone.Rooms[98]);
            ZoneHelper.ConnectRoom(Zone.Rooms[98], Direction.East, Zone.Rooms[99]);
            ZoneHelper.ConnectRoom(Zone.Rooms[99], Direction.South, Zone.Rooms[107]);
            ZoneHelper.ConnectRoom(Zone.Rooms[100], Direction.South, Zone.Rooms[108]);
            ZoneHelper.ConnectRoom(Zone.Rooms[101], Direction.East, Zone.Rooms[102]);
            ZoneHelper.ConnectRoom(Zone.Rooms[102], Direction.East, Zone.Rooms[103]);
            ZoneHelper.ConnectRoom(Zone.Rooms[104], Direction.East, Zone.Rooms[105]);
            ZoneHelper.ConnectRoom(Zone.Rooms[104], Direction.South, Zone.Rooms[109]);
            ZoneHelper.ConnectRoom(Zone.Rooms[105], Direction.East, Zone.Rooms[106]);
            ZoneHelper.ConnectRoom(Zone.Rooms[105], Direction.South, Zone.Rooms[110]);
            ZoneHelper.ConnectRoom(Zone.Rooms[106], Direction.South, Zone.Rooms[111]);
            ZoneHelper.ConnectRoom(Zone.Rooms[107], Direction.South, Zone.Rooms[113]);
            ZoneHelper.ConnectRoom(Zone.Rooms[108], Direction.South, Zone.Rooms[115]);
            ZoneHelper.ConnectRoom(Zone.Rooms[109], Direction.East, Zone.Rooms[110]);
            ZoneHelper.ConnectRoom(Zone.Rooms[110], Direction.East, Zone.Rooms[111]);
            ZoneHelper.ConnectRoom(Zone.Rooms[112], Direction.East, Zone.Rooms[113]);
            ZoneHelper.ConnectRoom(Zone.Rooms[112], Direction.South, Zone.Rooms[116]);
            ZoneHelper.ConnectRoom(Zone.Rooms[113], Direction.East, Zone.Rooms[114]);
            ZoneHelper.ConnectRoom(Zone.Rooms[113], Direction.South, Zone.Rooms[117]);
            ZoneHelper.ConnectRoom(Zone.Rooms[114], Direction.East, Zone.Rooms[115]);
            ZoneHelper.ConnectRoom(Zone.Rooms[114], Direction.South, Zone.Rooms[118]);
            ZoneHelper.ConnectRoom(Zone.Rooms[115], Direction.South, Zone.Rooms[119]);
            ZoneHelper.ConnectRoom(Zone.Rooms[116], Direction.East, Zone.Rooms[117]);
            ZoneHelper.ConnectRoom(Zone.Rooms[116], Direction.South, Zone.Rooms[123]);
            ZoneHelper.ConnectRoom(Zone.Rooms[117], Direction.East, Zone.Rooms[118]);
            ZoneHelper.ConnectRoom(Zone.Rooms[117], Direction.South, Zone.Rooms[124]);
            ZoneHelper.ConnectRoom(Zone.Rooms[118], Direction.East, Zone.Rooms[119]);
            ZoneHelper.ConnectRoom(Zone.Rooms[118], Direction.South, Zone.Rooms[125]);
            ZoneHelper.ConnectRoom(Zone.Rooms[119], Direction.East, Zone.Rooms[120]);
            ZoneHelper.ConnectRoom(Zone.Rooms[119], Direction.South, Zone.Rooms[126]);
            ZoneHelper.ConnectRoom(Zone.Rooms[120], Direction.East, Zone.Rooms[121]);
            ZoneHelper.ConnectRoom(Zone.Rooms[120], Direction.South, Zone.Rooms[127]);
            ZoneHelper.ConnectRoom(Zone.Rooms[121], Direction.East, Zone.Rooms[122]);
            ZoneHelper.ConnectRoom(Zone.Rooms[121], Direction.South, Zone.Rooms[128]);
            ZoneHelper.ConnectRoom(Zone.Rooms[122], Direction.South, Zone.Rooms[129]);
            ZoneHelper.ConnectRoom(Zone.Rooms[123], Direction.East, Zone.Rooms[124]);
            ZoneHelper.ConnectRoom(Zone.Rooms[124], Direction.East, Zone.Rooms[125]);
            ZoneHelper.ConnectRoom(Zone.Rooms[125], Direction.East, Zone.Rooms[126]);
            ZoneHelper.ConnectRoom(Zone.Rooms[126], Direction.East, Zone.Rooms[127]);
            ZoneHelper.ConnectRoom(Zone.Rooms[127], Direction.East, Zone.Rooms[128]);
            ZoneHelper.ConnectRoom(Zone.Rooms[127], Direction.South, Zone.Rooms[130]);
            ZoneHelper.ConnectRoom(Zone.Rooms[128], Direction.East, Zone.Rooms[129]);
            ZoneHelper.ConnectRoom(Zone.Rooms[128], Direction.South, Zone.Rooms[131]);
            ZoneHelper.ConnectRoom(Zone.Rooms[129], Direction.South, Zone.Rooms[132]);
            ZoneHelper.ConnectRoom(Zone.Rooms[130], Direction.East, Zone.Rooms[131]);
            ZoneHelper.ConnectRoom(Zone.Rooms[131], Direction.East, Zone.Rooms[132]);
        }
    }
}