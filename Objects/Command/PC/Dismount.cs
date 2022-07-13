using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.PC
{
    public class Dismount : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Dismount() : base(nameof(Dismount), ShortCutCharPositions.Awake) { }

        public IResult Instructions { get; } = new Result("Dismount", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Dismount" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            return GlobalReference.GlobalValues.World.Dismount(performer);
        }
    }
}
