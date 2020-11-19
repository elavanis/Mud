using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Command.God
{
    public class GameStats : BaseMobileObjectComand, IMobileObjectCommand
    {
        public GameStats() : base(nameof(GameStats), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("GameStats", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "GameStats" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            GlobalReference.GlobalValues.World.WorldCommands.Enqueue("GameStats");
            performer.EnqueueCommand("RetrieveGameStats");


            return new Result("Calculating Stats", true);
        }
    }
}

