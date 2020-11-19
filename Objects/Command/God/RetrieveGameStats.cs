using Objects.Command.Interface;
using System.Collections.Generic;
using Objects.Global;
using Objects.Mob.Interface;

namespace Objects.Command.God
{
    public class RetrieveGameStats : BaseMobileObjectComand, IMobileObjectCommand
    {
        public RetrieveGameStats() : base(nameof(RetrieveGameStats), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("RetrieveGameStats", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "RetrieveGameStats" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            GlobalReference.GlobalValues.World.WorldResults.TryGetValue("GameStats", out string value);

            if (value != null)
            {
                return new Result(value, false);
            }
            else
            {
                return new Result("Unable to retrieve game stats.", true);
            }
        }
    }
}
