using System.Linq;
using System.Reflection;
using MiscShared;
using Objects;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Language;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Personality.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Shared.Sound;
using static Objects.Global.Direction.Directions;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

namespace GenerateZones.Zones.Ash
{
    public class AshMaze : BaseZone, IZoneCode
    {
        MethodInfo baseAddExit;
        private int roomCount = 30;

        public AshMaze() : base(18)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(AshMaze);

            baseAddExit = typeof(ZoneHelper).GetMethod("AddExitToRoom", BindingFlags.Static | BindingFlags.NonPublic);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 0; i < roomCount; i++)
            {
                ZoneHelper.AddRoom(Zone, GetRoom());
            }

            Zone.Rooms[1].AddMobileObjectToRoom(LZoir(Zone.Rooms[1]));
            Zone.Rooms[10].AddMobileObjectToRoom(LZoir(Zone.Rooms[10]));
            Zone.Rooms[20].AddMobileObjectToRoom(LZoir(Zone.Rooms[20]));
            Zone.Rooms[30].AddMobileObjectToRoom(LZoir(Zone.Rooms[30]));

            Zone.Rooms[15].AddMobileObjectToRoom(AshWitch(Zone.Rooms[15]));

            ConnectRooms();



            return Zone;
        }

        private IMobileObject AshWitch(IRoom room)
        {
            string corpseDescription = "A pile of wispy ash lies here.";
            string examineDescription = "The witch stands eight feet tall ans is slender.  It is made entirely of gray ash and has no face.";
            string lookDescription = "An ash witch goes around trying to sweep up all the ash in vain.";
            string shortDescription = "Ash Witch";
            string sentenceDescription = "Ash Witch";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 85, corpseDescription);
            npc.KeyWords.Add("Ash");
            npc.KeyWords.Add("Witch");
            npc.God = true;     //needed to phase

            IPhase phase = new Phase();

            foreach (IRoom localroom in Zone.Rooms.Values)
            {
                phase.RoomsToPhaseTo.Add(new BaseObjectId(localroom));
            }

            npc.Personalities.Add(phase);
            return npc;
        }

        private IMobileObject LZoir(IRoom room)
        {
            string corpseDescription = "A giant LZoir corpse slowly turns gray.";
            string examineDescription = "A large insect resembling a mosquito with two stingers and spots of red on its wings.";
            string lookDescription = "A large insect that seems drawn to heat.";
            string shortDescription = "LZoir";
            string sentenceDescription = "LZoir";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 80, corpseDescription);
            npc.KeyWords.Add("LZoir");
            npc.God = true;     //needed to phase

            npc.Personalities.Add(new Aggressive());
            IPhase phase = new Phase();

            foreach (IRoom localroom in Zone.Rooms.Values)
            {
                phase.RoomsToPhaseTo.Add(new BaseObjectId(localroom));
            }

            npc.Personalities.Add(phase);

            return npc;
        }

        private IRoom GetRoom()
        {

            string shortDescription = "A world of ash.";
            string examineDescription = "Flakes of ash fall like gray snow on the ground from some unseen fire.";
            string lookDescription = "Ash floats through the air making it hard to see and breath.";
            IRoom room = OutdoorRoom(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private void ConnectRooms()
        {
            int roomPos = 1;
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.East, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.East, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.East, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.West, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.East, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.North, Zone.Rooms[++roomPos]);
            ConnectRooms(Zone.Rooms[roomPos], Direction.East, Zone.Rooms[++roomPos]);
            //ConnectRooms(Zone.Rooms[roomPos], Direction.South, Zone.Rooms[roomPos + 1]);  //Use North
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
                message.Sound.RandomSounds.Add($"{Zone.Name}\\Thunder{i.ToString()}.mp3");
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
