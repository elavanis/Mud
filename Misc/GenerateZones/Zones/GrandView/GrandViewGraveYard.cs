﻿using System;
using System.Collections.Generic;
using System.Reflection;
using MiscShared;
using Objects;
using Objects.Effect;
using Objects.Interface;
using Objects.Language;
using Objects.LevelRange;
using Objects.LoadPercentage;
using Objects.Magic.Enchantment;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Personality.Interface;
using Objects.Personality.Custom.GrandViewGraveYard;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static GenerateZones.RandomZoneGeneration;
using static Objects.Global.Direction.Directions;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewGraveYard : BaseZone, IZoneCode
    {
        public GrandViewGraveYard() : base(21)
        {
        }

        public IZone Generate()
        {
            List<string> names = new List<string>() { "Falim Nasha", "Bushem Dinon", "Stavelm Eaglelash", "Giu Thunderbash", "Marif Hlisk", "Fim Grirgav", "Strarcar Marshgem", "Storth Shadowless", "Tohkue-zid Lendikrafk", "Vozif Jikrehd", "Dranrovelm Igenomze", "Zathis Vedergi", "Mieng Chiao", "Thuiy Chim", "Sielbonron Canderger", "Craldu Gacevi",
            "Rumeim Shennud","Nilen Cahrom","Bei Ashspark","Hii Clanbraid","Sodif Vatsk","Por Rorduz","Grorcerth Forestsoar","Gath Distantthorne","Duhvat-keuf Faltrueltrim","Ham-kaoz Juhpafk","Rolvoumvald Gibenira","Rondit Vumregi","Foy Sheiy","Fiop Tei","Fruenrucu Jalbese","Fhanun Guldendal"};

            RandomZoneGeneration randZoneGen = new RandomZoneGeneration(5, 5, Zone.Id);
            RoomDescription description = new RoomDescription();
            description.LookDescription = "The dirt has been freshly disturbed where a body has been recently placed in the ground.";
            description.ExamineDescription = "Some flowers have been placed on the headstone that belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "The headstone has been here a while and is starting to show its age.";
            description.ExamineDescription = "The headstone name has worn off and is impossible to read.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "A grand tower of marble rises to the sky.  This person must have been important or rich in life.";
            description.ExamineDescription = "The tombstone belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "A small flat stone marker is all shows where this person is buried.";
            description.ExamineDescription = "The grave marker belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "There is a small bench for resting as one walks among the tombstones.";
            description.ExamineDescription = "A pair of angles are carved into the sides of the feet on the bench.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "Crosses give hint that the owner might have been religions in life.";
            description.ExamineDescription = "Here lies {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "The statue a weeping angle stands watch over the deceased.";
            description.ExamineDescription = "The grave belongs to {name}.";
            description.ShortDescription = "Graveyard";
            randZoneGen.RoomDescriptions.Add(description);

            FlavorOption option = new FlavorOption();
            option.FlavorValues.Add("{name}", names);
            description.FlavorOption.Add(option);
            randZoneGen.RoomDescriptions.Add(description);

            Zone = randZoneGen.Generate();
            Zone.Name = nameof(GrandViewGraveYard);

            int creatueChoices = 0;

            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo info in methods)
            {
                if (info.ReturnType == typeof(INonPlayerCharacter) && info.Name != "BuildNpc")
                {
                    creatueChoices++;
                }
            }

            int percent = (int)Math.Round(5d / creatueChoices, 0);
            List<int> hoursToSpawnUndead = new List<int>();
            for (int i = 12; i < 24; i++)
            {
                hoursToSpawnUndead.Add(i);
            }

            HeartbeatBigTickEnchantment enchantmentSkeleton = new HeartbeatBigTickEnchantment();
            enchantmentSkeleton.ActivationPercent = .2;
            enchantmentSkeleton.Effect = new LoadMob() { HoursToLoad = hoursToSpawnUndead };
            enchantmentSkeleton.Parameter = new EffectParameter() { Performer = Skeleton(), RoomMessage = new TranslationMessage("The skeleton rises slowly out of its grave.") };

            HeartbeatBigTickEnchantment enchantmentZombie = new HeartbeatBigTickEnchantment();
            enchantmentSkeleton.ActivationPercent = .2;
            enchantmentZombie.Effect = new LoadMob() { HoursToLoad = hoursToSpawnUndead };
            enchantmentZombie.Parameter = new EffectParameter() { Performer = Zombie(), RoomMessage = new TranslationMessage("A zombie burst forth from it grave hungry for brains.") };

            foreach (IRoom room in Zone.Rooms.Values)
            {
                ILoadableItems loadable = (ILoadableItems)room;
                loadable.LoadableItems.Add(new LoadPercentage() { PercentageLoad = percent, Object = Crow() });
                room.Attributes.Add(RoomAttribute.Outdoor);
                room.Attributes.Add(RoomAttribute.Weather);

                room.Enchantments.Add(enchantmentSkeleton);
                room.Enchantments.Add(enchantmentZombie);
            }

            SetRoom13();

            Zone.Rooms.Add(26, Room26());

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
            ZoneHelper.ConnectZone(Zone.Rooms[13], Direction.Down, 20, 1);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.East, Zone.Rooms[26]);

            ZoneHelper.ConnectZone(Zone.Rooms[5], Direction.North, 5, 29);
        }

        #region Rooms
        private IRoom Room26()
        {
            string lookDescription = "You shouldn't see this but since you can some how before you is the groundskeeper house.  It is a simple house with a cooking stove, table and chair and single bed for sleeping.";
            string examineDescription = "Maybe you should report the bug that you got here.";
            string shortDescription = "GroundsKeeper House";

            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Owner = "the grounds keeper";
            room.Guests.Add("Groundskeeper");

            room.AddMobileObjectToRoom(GroundsKeeper(room));

            return room;
        }

        private void SetRoom13()
        {
            IRoom room = Zone.Rooms[12];
            room.LookDescription = "The temple of Charon is shaped like a large mausoleum made of white granite.";
            room.ExamineDescription = "Reliefs of Charon ferrying people to the underworld can be seen carved into the side of the mausoleum.";
            room.ShortDescription = "Temple";
        }
        #endregion Rooms

        #region Npcs
        private IMobileObject GroundsKeeper(IRoom room)
        {
            string examineDescription = "Heavy gray eyes stare at you. He seems intently aware that you're not part of the normal surroundings but unaware what to do about it.";
            string lookDescription = "The groundskeeper looks tired of his job and dreams of a better place.";
            string sentenceDescription = "groundskeeper";
            string shortDescription = "Groundskeeper";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 30);
            npc.KeyWords.Add("Groundskeeper");
            npc.KeyWords.Add("Grounds");
            npc.KeyWords.Add("ground");
            npc.KeyWords.Add("keeper");

            npc.Personalities.Add(new GroundsKeeper());
            npc.Personalities.Add(new Wanderer());
            return npc;
        }

        private INonPlayerCharacter Skeleton()
        {
            string examineDescription = "There air takes on a slight chill as the skeleton turns and looks at you.";
            string lookDescription = "Somewhere the skeleton lost part of its arm.";
            string sentenceDescription = "skeleton";
            string shortDescription = "A skeleton walks bones clatter as it walks around.";

            //we pass null for the room since this does not have a room and has one set when its cloned
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, null!, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 17, UpperLevel = 19 };
            npc.KeyWords.Add("skeleton");

            npc.Personalities.Add(new DeathDuringDay());
            npc.Personalities.Add(Wanderer());

            return npc;
        }

        private INonPlayerCharacter Zombie()
        {
            string examineDescription = "The smell of rotting flesh emanates from the zombie as you get close to it.";
            string lookDescription = "The zombie is wearing clothes or at least what used to be clothes.  A small red handkerchief can be seen sticking out of what is left of its suit.";
            string sentenceDescription = "zombie";
            string shortDescription = "A zombie stares off into the distance looking at nothing.";

            //we pass null for the room since this does not have a room and has one set when its cloned
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, null!, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 17, UpperLevel = 19 };
            npc.KeyWords.Add("zombie");

            npc.Personalities.Add(new DeathDuringDay());
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(Wanderer());

            return npc;
        }

        private INonPlayerCharacter Crow()
        {
            string examineDescription = "It seems to have been born of the night with black feathers, feet and beak. The small black beady eyes are the only thing to reflect any light.";
            string lookDescription = "As you and the crow stare at each other it starts crowing loudly as trying to win a staring contest by making you look away.";
            string sentenceDescription = "a crow";
            string shortDescription = "A black as night crow calls out a warning as you approach.";

            //we pass null for the room since this does not have a room and has one set when its cloned
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, null!, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 16, UpperLevel = 17 };
            npc.KeyWords.Add("Crow");

            npc.Personalities.Add(Wanderer());

            return npc;
        }      

        private IWanderer Wanderer()
        {
            Wanderer wanderer = new Wanderer();
            for (int i = 0; i < 25; i++)
            {
                wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, i + 1));
            }

            return wanderer;
        }
        #endregion Npcs

    }
}
