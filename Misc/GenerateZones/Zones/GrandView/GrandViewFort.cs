using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Room.Room;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewFort : BaseZone, IZoneCode
    {
        public GrandViewFort() : base(24)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewFort);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            //ConnectRooms();

            return Zone;
        }

        #region Rooms

        private IRoom GenerateRoom1()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "The stone walls were carved in place from the side of the mountain.  This leads to their strength as it is on solid piece of stone.";
            room.LookDescription = "The original fort's stone gate still stands strong.";
            room.ShortDescription = "Front Gate";

            room.AddMobileObjectToRoom(Guard());
            room.AddMobileObjectToRoom(Guard());

            return room;
        }

        private IRoom OutSideRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(RoomAttribute.Outdoor);
            room.Attributes.Add(RoomAttribute.Weather);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "Standing in the center of the barbican you get a sense of dread for anyone who get trapped here attacking the fort.";
            room.LookDescription = "Walls of stone rise up on all sides with places for guards to fire arrows as well as dump fire down on you if you were an attacker.";
            room.ShortDescription = "Inside the barbican.";


            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "The inside of the fort court yard you begin to realize the amount of work that went into creating this fort.  Tons of raw stone was removed from the mountain side just to clear the area  ";
            room.LookDescription = "The court yard extends a ways to the north before disappearing into the mountain.  The blacksmith and enchanter is to west.  The captains quarters, and stables are to the east. ";
            room.ShortDescription = "The courtyard.";


            return room;
        }
        #endregion Rooms

        private INonPlayerCharacter Guard()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(Objects.Mob.NonPlayerCharacter.MobType.Humanoid, 21);
            npc.ShortDescription = "A motionless guard.";
            npc.LookDescription = "The guard stands motionless while watching people move in and out of the fort.";
            npc.ExamineDescription = "The guard's face is blank but you almost detect a hint of boredom.";
            npc.SentenceDescription = "guard";
            npc.KeyWords.Add("guard");

            npc.Personalities.Add(new Guardian());

            return npc;
        }

    }
}
