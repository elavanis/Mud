using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Magic;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Zone;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Damage;
using Objects.Global.DefaultValues;
using Objects.Item.Items;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Material.Materials;
using Objects.Personality.Personalities;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Magic.Interface;
using Objects.Magic.Spell;
using Objects.Effect;
using Objects.Die;
using Objects.Magic.Spell.Generic;
using Objects.Language;
using static Shared.TagWrapper.TagWrapper;
using Objects.Language.Interface;
using Objects.Global.Language;
using Objects.Personality.Personalities.Interface;
using Objects.Guild.Guilds;
using Objects.Guild;
using Objects.Personality.Personalities.ResponderMisc;
using static Objects.Mob.NonPlayerCharacter;

namespace GenerateZones.Zones
{
    public class Test : BaseZone, IZoneCode
    {
        public Test() : base(-1)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(Test);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "What can I say, its a room for testing.";
            room.LookDescription = "This room looks very much like a test.";
            room.ShortDescription = "Test Room";

            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = CreateRoom();

            room.AddMobileObjectToRoom(NPC());

            room.Attributes.Add(Room.RoomAttribute.Vault);
            return room;
        }

        private INonPlayerCharacter NPC()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 1);

            npc.KeyWords.Add("npc");

            npc.ExamineDescription = "A test mob dressed in its finished lab coat.";
            npc.LookDescription = "A test mob performing tests.";
            npc.ShortDescription = "A test mob.";
            npc.SentenceDescription = "test";

            npc.Health = int.MaxValue;

            IMagicUser magicUser = new MagicUser();
            magicUser.AddSpells(npc, new Wizard());
            //npc.Personalities.Add(magicUser);


            IResponder responder = new Responder();
            Response response = new Response();
            response.RequiredWordSets.Add(new OptionalWords() { TriggerWords = new List<string>() { "a" } });
            response.Message = "aa";
            responder.Responses.Add(response);

            response = new Response();
            response.RequiredWordSets.Add(new OptionalWords() { TriggerWords = new List<string>() { "b" } });
            response.Message = "bb";
            responder.Responses.Add(response);

            response = new Response();
            response.RequiredWordSets.Add(new OptionalWords() { TriggerWords = new List<string>() { "1", "2" } });
            response.Message = "1/2";
            responder.Responses.Add(response);

            response = new Response();
            response.RequiredWordSets.Add(new OptionalWords() { TriggerWords = new List<string>() { "x" } });
            response.RequiredWordSets.Add(new OptionalWords() { TriggerWords = new List<string>() { "y" } });
            response.Message = "xy";
            responder.Responses.Add(response);

            npc.Personalities.Add(responder);

            return npc;
        }
        #endregion Rooms

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();
        }
    }
}
