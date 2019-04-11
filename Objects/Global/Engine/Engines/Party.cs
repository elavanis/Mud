using Objects.Command;
using Objects.Command.Interface;
using Objects.Global.Engine.Engines.AdditionalParty;
using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects.Global.Engine.Engines
{
    public class Party
    {
        private Dictionary<IMobileObject, IGroup> Groups { get; } = new Dictionary<IMobileObject, IGroup>();
        private List<IPartyInvite> Invites { get; } = new List<IPartyInvite>();


        public void ProcessPartyInvites()
        {

        }

        public IResult Invite(IMobileObject performer, IMobileObject invitedMob)
        {
            lock (Groups)
            {
                if (Groups.TryGetValue(performer, out IGroup group))
                {
                    //this person is in a group
                    if (group.GroupLeader == performer)
                    {
                        if (Groups.TryGetValue(invitedMob, out IGroup invitedMobGroup))
                        {
                            return new Result($"{invitedMob.KeyWords[0]} is already in a party and will need to leave before you can invite them.", true);
                        }
                        else
                        {
                            if (Invites.Where(e => e.Invited == invitedMob).Any())
                            {
                                return new Result($"{invitedMob.KeyWords[0]} is already has an invite and will need to either accept or deny first.", true);
                            }
                            else
                            {
                                lock (Invites)
                                {
                                    Invites.Add(new PartyInvite() { Group = group, PartyLeader = performer, Invited = invitedMob, InviteTurn = GlobalReference.GlobalValues.TickCounter });
                                    invitedMob.EnqueueMessage($"{performer.KeyWords[0]} has invited you to their party.");
                                    return new Result($"{invitedMob.KeyWords[0]} has been invited to the party.", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        return new Result($"You need to be the party leader to invite people.", true);
                    }
                }
                else
                {
                    return new Result("You need to start a party before you can invite someone.", true);
                }
            }
        }
    }
}
