using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using Objects.Skill.Interface;
using Objects.Trap.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateZones
{
    public static class ZoneVerify
    {
        public static void VerifyZone(IZone zone)
        {
            Console.WriteLine(string.Format("Starting basic verification for {0}.", zone.Name));
            foreach (IRoom room in zone.Rooms.Values)
            {
                VerifyRoom(room);
            }
            Console.WriteLine(string.Format("{0} passed basic verification.", zone.Name));
        }

        private static void VerifyRoom(IRoom room)
        {
            string type = "Room ";
            VerifyIds(room, type);
            VerifyDescriptions(room, type);

            foreach (IItem item in room.Items)
            {
                VerifyItem(item);
            }

            foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
            {
                VerifyNpc(npc);
            }

            CheckRoomDoors(room);
        }

        private static void CheckRoomDoors(IRoom room)
        {
            CheckDoor(room.North?.Door);
            CheckDoor(room.East?.Door);
            CheckDoor(room.South?.Door);
            CheckDoor(room.West?.Door);
            CheckDoor(room.Up?.Door);
            CheckDoor(room.Down?.Door);

        }

        private static void CheckDoor(IDoor door)
        {
            if (door == null)
            {
                return;
            }

            VerifyItem(door);

            if (door.OpenMessage == null)
            {
                ThrowConfigException(door, null, string.Format("Door is missing OpenMessage."));
            }

            if (door.Linked)
            {
                if (door.LinkedRoomId == null)
                {
                    ThrowConfigException(door, null, string.Format("Door is missing LinkedRoomId."));
                }

                if (door.LinkedRoomId == null)
                {
                    ThrowConfigException(door, null, string.Format("Door is missing LinkedRoomId."));
                }
            }
        }

        private static void VerifyNpc(INonPlayerCharacter npc)
        {
            string type = "NPC";
            VerifyIds(npc, type);
            VerifyDescriptions(npc, type);
            VerifyMobType(npc, type);

            foreach (IItem item in npc.Items)
            {
                VerifyItem(item);
            }

            foreach (IItem item in npc.EquipedEquipment)
            {
                VerifyItem(item);
            }

            foreach (IPersonality personality in npc.Personalities)
            {
                IMerchant merchant = personality as IMerchant;
                if (merchant != null)
                {
                    foreach (IItem item in merchant.Sellables)
                    {
                        VerifyItem(item);
                    }
                }

                IPhase phase = personality as IPhase;
                if (phase != null)
                {
                    if (!npc.God)
                    {
                        ThrowConfigException(npc, type, string.Format($"Npc {npc.ShortDescription} needs to have God mode turned on."));
                    }
                }
            }

            foreach (ISpell spell in npc.SpellBook.Values)
            {
                if (spell.PerformerNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Spell {0} performer success notification is null.", spell.SpellName));
                }

                if (spell.RoomNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Spell {0} room success notification is null.", spell.SpellName));
                }

                if (spell.TargetNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Spell {0} target success notification is null.", spell.SpellName));
                }
            }

            foreach (ISkill skill in npc.KnownSkills.Values)
            {
                if (skill.PerformerNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} performer success notification is null.", skill.SkillName));
                }

                if (skill.RoomNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} room success notification is null.", skill.SkillName));
                }

                if (skill.TargetNotificationSuccess == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} target success notification is null.", skill.SkillName));
                }

                if (skill.PerformerNotificationFailure == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} performer failure notification is null.", skill.SkillName));
                }

                if (skill.RoomNotificationFailure == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} room failure notification is null.", skill.SkillName));
                }

                if (skill.TargetNotificationFailure == null)
                {
                    ThrowConfigException(null, type, string.Format("Skill {0} target failure notification is null.", skill.SkillName));
                }
            }
        }

        private static void VerifyMobType(INonPlayerCharacter npc, string type)
        {
            if (npc.TypeOfMob == null)
            {
                ThrowConfigException(npc, type, $"No mob type set {npc.SentenceDescription}.");
            }
        }

        private static void VerifyItem(IItem item)
        {
            string type = "Item";
            VerifyIds(item, type);
            VerifyDescriptions(item, type);

            VerifyArmor(item);
            VerifyWeapon(item);

            IContainer container = item as IContainer;
            if (container != null)
            {
                foreach (IItem innerItem in container.Items)
                {
                    VerifyItem(innerItem);
                }
            }
        }

        private static void VerifyWeapon(IItem item)
        {
            IWeapon weapon = item as IWeapon;
            if (weapon != null)
            {
                string type = "Weapon";

                if (weapon.DamageList.Count == 0)
                {
                    ThrowConfigException(item, type, $"No damage set for weapon {item.SentenceDescription}.");
                }

                if (weapon.Type == null)
                {
                    ThrowConfigException(item, type, $"No weapon type set for weapon {item.SentenceDescription}.");
                }
            }
        }

        private static void VerifyArmor(IItem item)
        {
            IArmor armor = item as IArmor;
            if (armor != null)
            {
                string type = "Armor";

                foreach (Damage.DamageType damage in Enum.GetValues(typeof(Damage.DamageType)))
                {
                    if (armor.GetTypeModifier(damage) == Decimal.MaxValue
                        && armor.Material == null)
                    {
                        ThrowConfigException(item, type, string.Format("Damage type {0} not set.", damage));
                    }
                }

                if (armor.Dice == null)
                {
                    ThrowConfigException(item, type, string.Format("No dice set."));
                }
            }
        }

        private static void VerifyDescriptions(IBaseObject item, string type)
        {
            if (item.ExamineDescription == null)
            {
                ThrowConfigException(item, type, "ExamineDescription = null");
            }

            if (item.LookDescription == null)
            {
                ThrowConfigException(item, type, "LongDescription = null");
            }

            if (item.ShortDescription == null)
            {
                ThrowConfigException(item, type, "ShortDescription = null");
            }

            IRoom room = item as IRoom;
            if (room == null) //do not check this for rooms
            {
                if (item.SentenceDescription == null
                    || item.SentenceDescription == "")
                {
                    ThrowConfigException(item, type, "SentenceDescription = null");
                }
            }

            if (item.KeyWords.Count == 0)
            {
                if (!item.ZoneSyncOptions.ContainsKey("ZoneSyncKeywords"))
                {
                    ThrowConfigException(item, type, "No Keywords Found");
                }
            }
        }

        private static void VerifyIds(IBaseObject item, string type)
        {
            if (item.Zone == 0)
            {
                ThrowConfigException(item, type, "Zone = 0");
            }
        }

        private static void ThrowConfigException(IBaseObject item, string type, string message)
        {
            if (type == null)
            {
                type = item.GetType().ToString();
            }
            throw new Exception(string.Format("{0} {1}", type, message));
        }
    }
}