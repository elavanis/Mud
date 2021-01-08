using Objects.Command.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Command.PC
{
    public class Party : BaseMobileObjectComand, IMobileObjectCommand
    {
        public Party() : base(nameof(Party), ShortCutCharPositions.Any) { }


        public IResult Instructions { get; } = new Result(@"Party Invite {Person To Invite}
Party Decline
Party {Message To Send To Party}
Party Start
Party Leave", true);


        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Party" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            IResult result = base.PerfomCommand(performer, command);
            if (result != null)
            {
                return result;
            }

            if (command.Parameters.Count == 0)
            {
                return Instructions;
            }
            else
            {
                string firstParameter = command.Parameters[0].ParameterValue;
                switch (firstParameter.ToUpper())
                {
                    case "INVITE":
                        return Invite(performer, command);
                    case "DECLINE":
                        return Decline(performer, command);
                    case "START":
                        return Start(performer, command);
                    case "LEAVE":
                        return Leave(performer, command);
                    default:
                        return Chat(performer, command);
                }
            }
        }




        private IResult Invite(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 2)
            {
                //oops we have a chat command inside starting with invite
                return Chat(performer, command);
            }
            else
            {
                string personToInvite = command.Parameters[1].ParameterValue.ToUpper();
                foreach (IPlayerCharacter pc in GlobalReference.GlobalValues.World.CurrentPlayers)
                {
                    if (pc.KeyWords[0].ToUpper() == personToInvite)
                    {
                        return GlobalReference.GlobalValues.Engine.Party.Invite(performer, pc);
                    }
                }

                return new Result($"Unable to find player {command.Parameters[1].ParameterValue}.", true);
            }

        }

        private IResult Decline(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 1)
            {
                //oops we have a chat command inside starting with decline
                return Chat(performer, command);
            }

            return GlobalReference.GlobalValues.Engine.Party.DeclinePartyInvite(performer);
        }

        private IResult Start(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 1)
            {
                //oops we have a chat command inside starting with decline
                return Chat(performer, command);
            }

            return GlobalReference.GlobalValues.Engine.Party.Start(performer);
        }

        private IResult Leave(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count > 1)
            {
                //oops we have a chat command inside starting with decline
                return Chat(performer, command);
            }

            return GlobalReference.GlobalValues.Engine.Party.Leave(performer);
        }

        private IResult Chat(IMobileObject performer, ICommand command)
        {
            IReadOnlyList<IMobileObject> partyMembers = GlobalReference.GlobalValues.Engine.Party.CurrentPartyMembers(performer);

            if (partyMembers == null)
            {
                return new Result("You are not in a party so you can't not chat with them.", true);
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (IParameter parameter in command.Parameters)
            {
                stringBuilder.Append(parameter.ParameterValue + " ");
            }

            string message = $"{performer.KeyWords[0]} party chats: {stringBuilder.ToString().Trim()}";
            ITranslationMessage translationMessage = new TranslationMessage(message, TagType.Communication);
            foreach (IMobileObject mob in partyMembers)
            {
                GlobalReference.GlobalValues.Notify.Mob(mob, translationMessage);
            }

            return null;
        }
    }
}