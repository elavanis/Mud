﻿using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Command.NPC
{
    public class Wait : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Wait", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Wait" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (performer is IPlayerCharacter)
            {
                //player character shouldn't be able to run this so allow them to do another command so it looks like they didn't execute this
                return new Result(null, true);
            }
            else
            {
                //make the npc wait turn
                return new Result(null, false);
            }
        }
    }
}
