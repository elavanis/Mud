﻿using Objects.Command.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Tell : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Tell() : base(nameof(Tell), ShortCutCharPositions.Any) { }

        public IResult Instructions { get; } = new Result("Tell [Player Name] [Message]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Tell" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult? result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count > 0)
            {
                if (command.Parameters.Count > 1)
                {
                    IMobileObject mob = FindPlayer(command.Parameters[0].ParameterValue);

                    if (mob == null)
                    {
                        mob = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(performer.Room, command.Parameters[0].ParameterValue).FirstOrDefault();
                    }

                    if (mob != null)
                    {
                        StringBuilder strBldr = new StringBuilder();
                        for (int i = 1; i < command.Parameters.Count; i++)
                        {
                            strBldr.Append(command.Parameters[i].ParameterValue + " ");
                        }

                        string message = string.Format("{0} tells you -- {1}", GlobalReference.GlobalValues.StringManipulator.CapitalizeFirstLetter(performer.SentenceDescription), strBldr.ToString()).Trim();
                        GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(message, TagType.Communication));

                        return new Result("", false);
                    }
                    else
                    {
                        string message = string.Format("Unable to find {0} to tell them.", command.Parameters[0].ParameterValue);
                        return new Result(message, true);
                    }
                }
                else
                {
                    return new Result("What would you like to tell them?", true);
                }
            }
            else
            {
                return new Result("Who would you like to tell what?", true);
            }
        }

        private IPlayerCharacter FindPlayer(string parameterValue)
        {
            return GlobalReference.GlobalValues.World.CurrentPlayers.FirstOrDefault(e => e.KeyWords.Any(f => f.Equals(parameterValue, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}
