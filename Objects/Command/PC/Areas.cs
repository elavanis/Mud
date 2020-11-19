using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Areas : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Areas() : base(nameof(Areas), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Areas", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Areas" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            StringBuilder stringBuilder = new StringBuilder();
            int zoneNameLength = 0;

            foreach (IZone zone in GlobalReference.GlobalValues.World.Zones.Values)
            {
                zoneNameLength = Math.Max(zoneNameLength, zone.Name.Length);
            }

            foreach (IZone zone in GlobalReference.GlobalValues.World.Zones.Values)
            {
                stringBuilder.AppendLine($"{zone.Name.PadRight(zoneNameLength)} -- Level: {zone.Level}");
            }

            return new Result(stringBuilder.ToString(), true);
        }
    }
}
