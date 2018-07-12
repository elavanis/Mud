using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects.Command.God
{
    public class Possess : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "Posses {Mob Keyword}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Possess" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count == 0)
            {
                if (performer.PossedMob != null)
                {
                    IMobileObject PossedMob = performer.PossedMob;
                    performer.PossedMob.PossingMob = null;
                    performer.PossedMob = null;
                    return new Result(true, $"You release control of {PossedMob.SentenceDescription}.");
                }
                else
                {
                    return new Result(false, "You were not possessing anyone.");
                }
            }
            else
            {
                IMobileObject possedMob = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(performer.Room, command.Parameters[0].ParameterValue).FirstOrDefault();

                if (possedMob == null)
                {
                    possedMob = GlobalReference.GlobalValues.FindObjects.FindPcInRoom(performer.Room, command.Parameters[0].ParameterValue).FirstOrDefault();
                }

                if (possedMob == null)
                {
                    return new Result(false, $"You were unable to find {command.Parameters[0].ParameterValue}.");
                }
                else
                {
                    //release any possessed mob as we are about to possess another
                    if (performer.PossedMob != null)
                    {
                        performer.PossedMob.PossingMob = null;
                    }

                    possedMob.PossingMob = performer;
                    performer.PossedMob = possedMob;
                    return new Result(false, $"You possessed {command.Parameters[0].ParameterValue}.");
                }
            }
        }
    }
}
