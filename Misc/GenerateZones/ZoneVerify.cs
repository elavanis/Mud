﻿using Objects.Damage;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.LoadPercentage.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using Objects.Skill.Interface;
using Objects.Zone.Interface;
using System;
using static Objects.Item.Items.Weapon;
using static Objects.Room.Room;

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

            foreach (IEnchantment item in room.Enchantments)
            {
                VerifyEnchantment(item);
            }

            ILoadableItems loadableItems = room as ILoadableItems;
            foreach (ILoadPercentage loadPercentage in loadableItems.LoadableItems)
            {
                IItem item = loadPercentage.Object as IItem;
                if (item != null)
                {
                    VerifyItem(item);
                }

                INonPlayerCharacter nonPlayerCharacter = loadPercentage.Object as INonPlayerCharacter;
                if (nonPlayerCharacter != null)
                {
                    VerifyNpc(nonPlayerCharacter);
                }

                IMount mount = loadPercentage.Object as IMount;
                if (mount != null)
                {
                    VerifyMount(mount);
                }

            }

            foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
            {
                VerifyNpc(npc);
            }

            foreach (IMobileObject mobileObject in room.OtherMobs)
            {
                if (mobileObject is IMount)
                {
                    VerifyMount(mobileObject as IMount);
                }
                else
                {
                    VerifyMob(mobileObject);
                }
            }

            if (room.Attributes.Count == 0)
            {
                ThrowConfigException(room, type, "Room attributes blank.");
            }
            else if (room.Attributes.Contains(RoomAttribute.Outdoor))
            {
                if (!room.Attributes.Contains(RoomAttribute.Weather))
                {
                    Console.WriteLine($"Room {room.ZoneId} - {room.Id} has outdoors but not weather.");
                }
            }

            CheckRoomDoors(room);
        }

        private static void VerifyEnchantment(IEnchantment enchantment)
        {
            string type = "Enchantment";

            if (enchantment.Effect is Objects.Effect.Damage)
            {
                if (enchantment.Parameter.Description == null)
                {
                    ThrowConfigException(enchantment, type, $"Enchantments with effect damage must have a damage description.");
                }
            }
        }

        private static void VerifyMount(IMount mount)
        {
            VerifyMob(mount);

            string type = "Mount";
            if (mount.Movement == -1)
            {
                ThrowConfigException(mount, type, $"Mount has no movement {mount.SentenceDescription}.");
            }

            if (mount.StaminaMultiplier == -1)
            {
                ThrowConfigException(mount, type, $"Mount has no stamina multiplier {mount.SentenceDescription}.");
            }

            if (mount.MaxRiders == -1)
            {
                ThrowConfigException(mount, type, $"Mount has no max riders {mount.SentenceDescription}.");
            }
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

        private static void VerifyMob(IMobileObject mob)
        {
            string type = "MOB";
            VerifyIds(mob, type);
            VerifyDescriptions(mob, type);

            foreach (IEnchantment enchantment in mob.Enchantments)
            {
                VerifyEnchantment(enchantment);
            }


            foreach (IItem item in mob.Items)
            {
                VerifyItem(item);
            }

            foreach (IItem item in mob.EquipedEquipment)
            {
                VerifyItem(item);
            }

            foreach (ISpell spell in mob.SpellBook.Values)
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

            foreach (ISkill skill in mob.KnownSkills.Values)
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

        private static void VerifyNpc(INonPlayerCharacter npc)
        {
            VerifyMob(npc);

            string type = "NPC";
            VerifyNpcType(npc, type);
            VerifyNpcLevel(npc, type);

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
        }

        private static void VerifyNpcLevel(INonPlayerCharacter npc, string type)
        {
            if (npc.Level > 0)
            {
                return;
            }

            if (npc.LevelRange != null)
            {
                return;
            }

            ThrowConfigException(npc, type, $"Mob has no level {npc.SentenceDescription}.");
        }

        private static void VerifyNpcType(INonPlayerCharacter npc, string type)
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

            foreach (IEnchantment enchantment in item.Enchantments)
            {
                VerifyEnchantment(enchantment);
            }

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

                if (weapon.Type == WeaponType.NotSet)
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
                    if (damage != Damage.DamageType.NotSet) //skip not set
                    {
                        if (armor.GetTypeModifier(damage) == Decimal.MaxValue
                            && armor.Material == null)
                        {
                            ThrowConfigException(item, type, string.Format("Damage type {0} not set.", damage));
                        }
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
                ThrowConfigException(item, type, "LookDescription = null");
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
            if (item.ZoneId == 0)
            {
                ThrowConfigException(item, type, "Zone = 0");
            }

            if (item.Id == 0)
            {
                ThrowConfigException(item, type, "Id = 0");
            }
        }

        private static void ThrowConfigException(object item, string type, string message)
        {
            if (type == null)
            {
                type = item.GetType().ToString();
            }
            throw new Exception(string.Format("{0} {1}", type, message));
        }
    }
}