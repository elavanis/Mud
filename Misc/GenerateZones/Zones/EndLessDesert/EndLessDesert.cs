using Objects.Effect.Zone.EndlessDesert;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using static GenerateZones.RandomZoneGeneration;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.EndLessDesert
{
    public class EndLessDesert : BaseZone, IZoneCode
    {

        public EndLessDesert() : base(14)
        {
        }

        IZone IZoneCode.Generate()
        {
            RandomZoneGeneration randZoneGen = new RandomZoneGeneration(10, 10, Zone.Id);
            RoomDescription description = new RoomDescription();
            description.LookDescription = "Dunes gently roll off into the horizon like waves on the ocean.";
            description.ExamineDescription = "The sand is a soft almost powdery substance that lets you sink up to your ankles.";
            description.ShortDescription = "Desert";
            randZoneGen.RoomDescriptions.Add(description);

            Zone = randZoneGen.Generate();
            Zone.Name = nameof(EndLessDesert);


            Random random = new Random(Zone.Id);
            IRoom room = Zone.Rooms[random.Next(Zone.Rooms.Count) + 1];
            room.LookDescription = "Lush trees grow around the small lake forming every desert travelers dream, an oasis.";
            room.ExamineDescription = "A small lake is a pale cool blue color inviting you to take a drink and cool off from the hot desert air.";
            room.ShortDescription = "Oasis";

            IEnchantment enchantment = new HeartbeatBigTickEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new DoorwayToUnderworld();
            room.Enchantments.Add(enchantment);

            ZoneHelper.ConnectZone(Zone.Rooms[6], Direction.North, 9, 93);

            return Zone;
        }
    }
}
