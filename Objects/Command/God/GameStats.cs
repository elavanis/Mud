using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.God
{
    public class GameStats : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("GameStats", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "GameStats" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            GlobalReference.GlobalValues.World.WorldCommands.Enqueue("GameStats");
            performer.EnqueueCommand("RetrieveGameStats");


            return new Result("Calculating Stats", true);
        }
    }
}

