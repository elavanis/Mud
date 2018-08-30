using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Zone.Interface;
using static GenerateZones.RandomZoneGeneration;
using Objects.Mob.Interface;
using Objects.Mob;
using Objects.Personality.Personalities;
using Objects.Global;
using Objects.Room.Interface;
using Objects.LevelRange;
using System.Reflection;
using Objects.Interface;
using Objects.LoadPercentage;
using static Objects.Mob.NonPlayerCharacter;

namespace GenerateZones.Zones.ConnectingZones
{
    public class GrandViewDeepForest : BaseZone, IZoneCode
    {
        public GrandViewDeepForest() : base(9)
        {
        }

        IZone IZoneCode.Generate()
        {
            RandomZoneGeneration randZoneGen = new RandomZoneGeneration(10, 10, Zone.Id);
            RoomDescription description = new RoomDescription();
            description.LookDescription = "This part of the field is tilled and ready to be planted.";
            description.ExamineDescription = "The dirt is rich and will support a good crop.";
            description.ShortDescription = "Farmland";
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "While the {crop} looks healthy it is still to young to eat.";
            description.ExamineDescription = "A tall crop of {crop} is growing here.";
            description.ShortDescription = "Farmland";
            FlavorOption option = new FlavorOption();
            option.FlavorValues.Add("{crop}", new List<string>() { "corn", "wheat", "grapes" });
            description.FlavorOption.Add(option);
            randZoneGen.RoomDescriptions.Add(description);

            description = new RoomDescription();
            description.LookDescription = "The field is full of tall grass.";
            description.ExamineDescription = "The field is full of tall grass that seems to flow around you as you walk through it.";
            description.ShortDescription = "Farmland";
            randZoneGen.RoomDescriptions.Add(description);

            option = new FlavorOption();
            option.FlavorText = "A {type} fence runs parallel to you a {distance} away.";
            option.FlavorValues.Add("{type}", new List<string>() { "wooden", "stone" });
            option.FlavorValues.Add("{distance}", new List<string>() { "short", "long" });
            randZoneGen.RoomFlavorText.Add(option);

            option = new FlavorOption();
            option.FlavorText = "A rusted horse shoe has been lost and lies rusting away.";
            randZoneGen.RoomFlavorText.Add(option);

            option = new FlavorOption();
            option.FlavorText = "A small hill rises to the {direction} in the distance.";
            option.FlavorValues.Add("{direction}", new List<string>() { "north", "east", "south", "west" });
            randZoneGen.RoomFlavorText.Add(option);

            Zone = randZoneGen.Generate();
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(GrandViewDeepForest);


            description = new RoomDescription();
            description.LookDescription = "A road runs through the farm lands.";
            description.ExamineDescription = "Two wagon ruts cut into the soil.";
            description.ShortDescription = "Road";
            randZoneGen.RoadDescription = description;
            randZoneGen.AddRoad(Zone, null, new ZoneConnection() { ZoneId = 8, RoomId = 1 }, new ZoneConnection() { ZoneId = 14, RoomId = 6 }, new ZoneConnection() { ZoneId = 4, RoomId = 6 });

            int animalChoices = 0;

            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo info in methods)
            {
                if (info.ReturnType == typeof(INonPlayerCharacter) && info.Name != "BuildNpc")
                {
                    animalChoices++;
                }
            }


            int percent = 20 / animalChoices;
            foreach (IRoom room in Zone.Rooms.Values)
            {
                ILoadableItems loadable = (ILoadableItems)room;
                loadable.LoadableItems.Add(new LoadPercentage() { PercentageLoad = percent, Object = Cow() });
                loadable.LoadableItems.Add(new LoadPercentage() { PercentageLoad = percent, Object = Horse() });
                loadable.LoadableItems.Add(new LoadPercentage() { PercentageLoad = percent, Object = Chicken() });

            }
            return Zone;
        }

        private INonPlayerCharacter Cow()
        {
            INonPlayerCharacter npc = BuildNpc();
            npc.LevelRange = new LevelRange() { LowerLevel = 8, UpperLevel = 10 };
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("cow");
            npc.SentenceDescription = "cow";
            npc.ShortDescription = "A dairy cow.";
            npc.LookDescription = "A dairy cow lazily eats grass.";
            npc.ExamineDescription = "The cow looks to be about five feet tall and could easily push you out of the way if it wanted the grass you were standing on.";

            //TODO add hide drop, some type of leather material
            //TODO make drop only appear on death

            return npc;
        }

        private INonPlayerCharacter Horse()
        {
            INonPlayerCharacter npc = BuildNpc();
            npc.LevelRange = new LevelRange() { LowerLevel = 9, UpperLevel = 11 };
            npc.KeyWords.Add("horse");
            npc.SentenceDescription = "horse";
            npc.ShortDescription = "A fine horse stands here.";
            npc.LookDescription = "A beautiful {color} horse stands looking at you.";
            npc.ExamineDescription = "The {color} horse.";
            npc.FlavorOptions.Add("{color}", new List<string>() { "black", "brown" });

            return npc;
        }

        private INonPlayerCharacter Chicken()
        {
            INonPlayerCharacter npc = BuildNpc();
            npc.LevelRange = new LevelRange() { LowerLevel = 5, UpperLevel = 8 };
            npc.KeyWords.Add("chicken");
            npc.SentenceDescription = "chicken";
            npc.ShortDescription = "A chicken";
            npc.LookDescription = "The chicken struts around pecking the ground looking for something to eat.";
            npc.ExamineDescription = "The chicken looks to be just the right size for some good eating.";

            return npc;
        }

        private INonPlayerCharacter BuildNpc()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other);
            npc.Personalities.Add(new Wanderer());
            return npc;
        }
    }
}
