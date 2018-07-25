using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;

namespace GenerateZones.Zones.Mountain
{
    public class DragonMountainFace : BaseZone, IZoneCode
    {
        public DragonMountainFace() : base(19)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(DragonMountainFace);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            return Zone;
        }

        #region Mountain Face
        #region Level 1
        private IRoom GenerateRoom1()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A rock face looks craggy and has plenty of hand holds.";
            room.LookDescription = "You stand at the base of the mountain side.  The ground is rocky here with little vegetation growing.";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The path up appears to be clear and able to be climbed.";
            room.LookDescription = "You stand at the base of the mountain side.  A few small clumps of grass grow in cracks in the rocky soil.";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "Several hand holds have been marked on the rock face showing a way up the mountain side.";
            room.LookDescription = "You stand at the base of the mountain side.  A small pool of water sits in a small divot in an otherwise smooth rock ground.";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face seems to smooth out the higher up you go.";
            room.LookDescription = "You stand at the base of the mountain side.  The ground is made of soil and has several sets of goat prints here.";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A few feet above you the rock face move back into the mountain making it hard to see a path up.";
            room.LookDescription = "You stand at the base of the mountain side.  There is a long path leading up the mountain to the east and a short solid rock wall going straight up.";

            return room;
        }
        #endregion Level 1

        #region Level 2
        private IRoom GenerateRoom6()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "There are scratches on the rock face indicating people have climbed on this rock face before you.";
            room.LookDescription = "The wall rock face is craggy and easy to climb.";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A small green bush protrudes from the rock ledge.";
            room.LookDescription = "A small ledge makes standing here much easier.";

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The hand holds continue on a path straight up.";
            room.LookDescription = "A tree branch brushes up against the rock face as if saying hello there.";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face is getting very hard to find hand holds.";
            room.LookDescription = "The rock face is harder to climb with less hand holds.";

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A small birds nest sits on a small ledge to a few feet to your right.";
            room.LookDescription = "The rock wall is smooth like water has flowed over it but is still craggy with plenty of holds.";

            return room;
        }
        #endregion Level 2

        #region Level 3
        private IRoom GenerateRoom11()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A small crack quickly grows into a crack large enough to crawl into.";
            room.LookDescription = "The rock face is starting to get craggy and brittle.";

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The soft stone has been worn away in spots and polished smooth.";
            room.LookDescription = "The rock face has become loose crumbly stone that breaks when weight it put on it.";

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face contains a vein of sparkling stones while pretty would be to hard to get while dangling from the rock face.";
            room.LookDescription = "The rock face looks to have broken off here reveling the innards of the mountain.";

            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A small birds nest gently sits on a branch growing out of the rock face.  Be careful if you use the branch as a hand hold.";
            room.LookDescription = "The hand holds are getting thin except a few branches growing out of the rock face.";

            return room;
        }

        private IRoom GenerateRoom15()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The stone resembles a eye of a dragon.  This is what gives this mountain its name. Dragon Mountain.";
            room.LookDescription = "There is a large round stone that some how is carved out of the mountain side.";

            return room;
        }
        #endregion Level 3

        #region Level 4
        private IRoom GenerateRoom16()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face is spotted with little hole as if a great volcanic blast blew holes in the mountain side.";
            room.LookDescription = "The wall here is a golden brown color.";

            return room;
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "A great divot has been worn away by the wind in the face of the mountain.";
            room.LookDescription = "The soft rock has not stood the test of time and worn away.";

            return room;
        }

        private IRoom GenerateRoom18()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The stone has sparkles slightly as if its made of granite.";
            room.LookDescription = "The rock face looks like it a cool gray color.";

            return room;
        }

        private IRoom GenerateRoom19()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The rock face looks like it has a grid pattern almost as if it was made of scales.";
            room.LookDescription = "The rock here has a grid pattern as if it was once made of something else.";

            return room;
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The stone has given way to a white chalky substance that crumbles in your hand.";
            room.LookDescription = "The rock has become a white chalky substance making it easy to climb but dangerous to stay.";

            return room;
        }
        #endregion Level 4

        #region Level 5
        private IRoom GenerateRoom21()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The top of the rock mountain ledge is covered in green grass.";
            room.LookDescription = "The sheer rock face is looks difficult to climb.";

            return room;
        }

        private IRoom GenerateRoom22()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "Unfortunately the ropes seem quite frail and do not look to be able to support weight.";
            room.LookDescription = "Several ropes have been hung over the side of the ledge allowing one to climb down the cliff.";

            return room;
        }

        private IRoom GenerateRoom23()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "It is hard to tell if the depression is natural or man made but the stones were put there by someone for sure.";
            room.LookDescription = "The earth has been shaped into a large bowl area with rocks forming a perimeter.";

            return room;
        }

        private IRoom GenerateRoom24()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "The remnants of what appear to be torches can be seen scattered around.";
            room.LookDescription = "Here the ground has been burned by fire.";

            return room;
        }

        private IRoom GenerateRoom25()
        {
            IRoom room = RockFace();
            room.ExamineDescription = "Large rocks five feet in diameter are scattered around for what appears like no good reason.";
            room.LookDescription = "The terrain and interesting mix of large rocks and small patches of grass.";

            return room;
        }
        #endregion Level 5
        #endregion Mountain Face

        #region Path
        private IRoom GenerateRoom26()
        {
            IRoom room = RockPath();
            room.ExamineDescription = "A small path leads up the side of the mountain.";
            room.LookDescription = "Several small flowers grow along side of the path here.";

            return room;
        }
        #endregion Path


        private IRoom RockFace()
        {
            IRoom room = CreateRoom(100);
            room.ShortDescription = "Mountain Face";
            return room;
        }

        private IRoom RockPath()
        {
            IRoom room = CreateRoom(1);
            room.ShortDescription = "Mountain Path";
            return room;
        }

        private IRoom OutSide()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);
            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = CreateRoom();
            return room;
        }
    }
}
