using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global.Direction;
using Objects.Interface;
using Objects.Language;
using Objects.Magic.Enchantment;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;
using Shared.Sound;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.Ash
{
    public class AshMaze : IZoneCode
    {
        MethodInfo baseAddExit;
        IZone zone;
        private int zoneId = 18;
        private int roomId = 1;
        private int itemId = 1;
        private int npcId = 1;
        private int roomCount = 30;
        public IZone Generate()
        {
            zone = new Zone();
            zone.Id = zoneId;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(AshMaze);

            baseAddExit = typeof(ZoneHelper).GetMethod("AddExitToRoom", BindingFlags.Static | BindingFlags.NonPublic);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 0; i < roomCount; i++)
            {
                ZoneHelper.AddRoom(zone, GetRoom());
            }

            zone.RecursivelySetZone();
            zone.Rooms[1].AddMobileObjectToRoom(LZoir());
            zone.Rooms[10].AddMobileObjectToRoom(LZoir());
            zone.Rooms[20].AddMobileObjectToRoom(LZoir());
            zone.Rooms[30].AddMobileObjectToRoom(LZoir());

            zone.Rooms[15].AddMobileObjectToRoom(AshWitch());

            ConnectRooms();



            return zone;
        }

        private IMobileObject AshWitch()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 85;
            npc.KeyWords.Add("Ash");
            npc.KeyWords.Add("Witch");
            npc.God = true;

            IPhase phase = new Phase();
            foreach (IRoom room in zone.Rooms.Values)
            {
                phase.RoomsToPhaseTo.Add(new BaseObjectId(room));
            }
            npc.Personalities.Add(phase);

            npc.ShortDescription = "Ash Witch";
            npc.LongDescription = "An ash witch goes around trying to sweep up all the ash in vain.";
            npc.ExamineDescription = "The witch stands eight feet tall ans is slender.  It is made entirely of gray ash and has no face.";
            npc.SentenceDescription = "Ash Witch";

            return npc;
        }

        private IMobileObject LZoir()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 80;
            npc.KeyWords.Add("LZoir");
            npc.God = true;

            npc.Personalities.Add(new Aggressive());
            IPhase phase = new Phase();
            foreach (IRoom room in zone.Rooms.Values)
            {
                phase.RoomsToPhaseTo.Add(new BaseObjectId(room));
            }
            npc.Personalities.Add(phase);

            npc.ShortDescription = "LZoir";
            npc.LongDescription = "A large insect that seems drawn to heat.";
            npc.ExamineDescription = "A large insect resembling a mosquito with two stingers and spots of red on its wings.";
            npc.SentenceDescription = "LZoir";

            return npc;
        }

        private IRoom GetRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = 1;
            room.ShortDescription = "A world of ash.";
            room.ExamineDescription = "Flakes of ash fall like gray snow on the ground from some unseen fire.";
            room.LongDescription = "Ash floats through the air making it hard to see and breath.";

            return room;
        }

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            int roomPos = 1;
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.East, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.East, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.East, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.West, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.East, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.North, zone.Rooms[++roomPos]);
            ConnectRooms(zone.Rooms[roomPos], Direction.East, zone.Rooms[++roomPos]);
            //ConnectRooms(zone.Rooms[roomPos], Direction.South, zone.Rooms[roomPos + 1]);  //Use North
        }

        private void ConnectRooms(IRoom room1, Direction direction, IRoom room2)
        {
            baseAddExit.Invoke(null, new object[] { room1, direction, room2, null });

            int successfulPutbackPercent = 5;

            IExit exit = new Exit();
            exit.Room = 1;
            exit.Zone = room1.Zone;

            if (room1.North == null)
            {
                room1.North = exit;
                if (room1.Id != 1)
                {
                    room1.Enchantments.Add(GenerateMobEnchantment(Direction.North, 100));
                }
            }
            else
            {
                room1.Enchantments.Add(GenerateMobEnchantment(Direction.North, successfulPutbackPercent));
            }

            if (room1.East == null)
            {
                room1.East = exit;
                if (room1.Id != 1)
                {
                    room1.Enchantments.Add(GenerateMobEnchantment(Direction.East, 100));
                }
            }
            else
            {
                room1.Enchantments.Add(GenerateMobEnchantment(Direction.East, successfulPutbackPercent));
            }

            if (room1.South == null)
            {
                room1.South = exit;
                if (room1.Id != 1)
                {
                    room1.Enchantments.Add(GenerateMobEnchantment(Direction.South, 100));
                }
            }
            else
            {
                room1.Enchantments.Add(GenerateMobEnchantment(Direction.South, successfulPutbackPercent));
            }

            if (room1.West == null)
            {
                room1.West = exit;
                if (room1.Id != 1)
                {
                    room1.Enchantments.Add(GenerateMobEnchantment(Direction.West, 100));
                }
            }
            else
            {
                room1.Enchantments.Add(GenerateMobEnchantment(Direction.West, successfulPutbackPercent));
            }

            Message message = new Message();
            message.Sound = new Sound();
            for (int i = 1; i < 9; i++)
            {
                message.Sound.RandomSounds.Add($"{zone.Name}\\Thunder{i.ToString()}.mp3");
            }

            string castleSilhouette = $"Lightning illuminates a castle to the {direction.ToString()}.";
            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.RoomId = new BaseObjectId(room1);
            effectParameter.RoomMessage = new TranslationMessage(castleSilhouette);
            HeartbeatBigTickEnchantment heartbeatBigTickEnchantment = new HeartbeatBigTickEnchantment();
            heartbeatBigTickEnchantment.ActivationPercent = 2;
            heartbeatBigTickEnchantment.Effect = message;
            heartbeatBigTickEnchantment.Parameter = effectParameter;


            room1.Enchantments.Add(heartbeatBigTickEnchantment);
        }

        private static LeaveRoomMovePcEnchantment GenerateMobEnchantment(Direction direction, int percent)
        {
            LeaveRoomMovePcEnchantment leaveRoomEnchantment = new LeaveRoomMovePcEnchantment();
            leaveRoomEnchantment.Direction = direction;
            leaveRoomEnchantment.ActivationPercent = percent;
            leaveRoomEnchantment.RoomId = new BaseObjectId(18, 1);
            leaveRoomEnchantment.Effect = new MoveMob();
            return leaveRoomEnchantment;
        }
    }
}
