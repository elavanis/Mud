using Objects.Effect.Zone.EndlessDesert;
using Objects.Interface;
using Objects.LoadPercentage;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static GenerateZones.RandomZoneGeneration;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.EndLessDesert
{
    public class EndLessDesert : IZoneCode
    {
        //private int npcId = 1;
        private int zoneId = 14;

        IZone IZoneCode.Generate()
        {
            RandomZoneGeneration randZoneGen = new RandomZoneGeneration(10, 10, zoneId);
            RoomDescription description = new RoomDescription();
            description.LongDescription = "Dunes gently roll off into the horizon like waves on the ocean.";
            description.ExamineDescription = "The sand is a soft almost powdery substance that lets you sink up to your ankles.";
            description.ShortDescription = "Desert";
            randZoneGen.RoomDescriptions.Add(description);

            IZone zone = randZoneGen.Generate();
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(EndLessDesert);


            Random random = new Random(zoneId);
            IRoom room = zone.Rooms[random.Next(zone.Rooms.Count) + 1];
            room.LongDescription = "Lush trees grow around the small lake forming every desert travelers dream, an oasis.";
            room.ExamineDescription = "A small lake is a pale cool blue color inviting you to take a drink and cool off from the hot desert air.";
            room.ShortDescription = "Oasis";

            IEnchantment enchantment = new HeartbeatBigTickEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new DoorwayToUnderworld();
            room.Enchantments.Add(enchantment);

            ZoneHelper.ConnectZone(zone.Rooms[6], Direction.North, 9, 93);

            return zone;
        }
    }
}
