using System.Collections.Generic;
using Objects.Zone.Interface;
using static GenerateZones.RandomZoneGeneration;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Room.Interface;
using Objects.LevelRange;
using System.Reflection;
using Objects.Interface;
using Objects.LoadPercentage;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

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
            Zone.Name = nameof(GrandViewDeepForest);
            foreach (IRoom localRoom in Zone.Rooms.Values)
            {
                localRoom.Attributes.Add(RoomAttribute.Outdoor);
                localRoom.Attributes.Add(RoomAttribute.Weather);
            }

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
            string examineDescription = "The cow looks to be about five feet tall and could easily push you out of the way if it wanted the grass you were standing on.";
            string lookDescription = "A dairy cow lazily eats grass.";
            string sentenceDescription = "cow";
            string shortDescription = "A dairy cow.";

            INonPlayerCharacter npc = BuildNpc(examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 8, UpperLevel = 10 };
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("cow");

            //TODO add hide drop, some type of leather material
            //TODO make drop only appear on death

            return npc;
        }

        private INonPlayerCharacter Horse()
        {
            string examineDescription = "The {color} horse.";
            string lookDescription = "A beautiful {color} horse stands looking at you.";
            string sentenceDescription = "horse";
            string shortDescription = "A fine horse stands here.";

            INonPlayerCharacter npc = BuildNpc(examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 9, UpperLevel = 11 };
            npc.KeyWords.Add("horse");
            npc.FlavorOptions.Add("{color}", new List<string>() { "black", "brown" });

            return npc;
        }

        private INonPlayerCharacter Chicken()
        {
            string examineDescription = "The chicken looks to be just the right size for some good eating.";
            string lookDescription = "The chicken struts around pecking the ground looking for something to eat.";
            string sentenceDescription = "chicken";
            string shortDescription = "A chicken";

            INonPlayerCharacter npc = BuildNpc(examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.LevelRange = new LevelRange() { LowerLevel = 5, UpperLevel = 8 };
            npc.KeyWords.Add("chicken");

            return npc;
        }

        private INonPlayerCharacter BuildNpc(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            //these are template characters so don't add the room
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, null!, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.Personalities.Add(new Wanderer());
            return npc;
        }
    }
}
