using Objects.Command;
using Objects.Command.Interface;
using Objects.Global.Engine.Engines.AdditionalParty;
using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Global.Engine.Engines
{
    public class Party : IParty
    {
        private object padLock = new object();
        private Dictionary<IMobileObject, IGroup> Groups { get; } = new Dictionary<IMobileObject, IGroup>();
        private List<IPartyInvite> Invites { get; } = new List<IPartyInvite>();

        public void RemoveOldPartyInvites()
        {
            for (int i = Invites.Count - 1; i >= 0; i--)
            {
                IPartyInvite invite = Invites[i];
                if (DateTime.UtcNow.Subtract(invite.InviteTime).TotalMinutes >= 5)
                {
                    Invites.RemoveAt(i);
                }
            }
        }

        public IResult Invite(IMobileObject performer, IMobileObject invitedMob)
        {
            lock (padLock)
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
                                return new Result($"{invitedMob.KeyWords[0]} already has an invite and will need to either accept or deny first.", true);
                            }
                            else
                            {
                                Invites.Add(new PartyInvite() { Group = group, PartyLeader = performer, Invited = invitedMob, InviteTime = DateTime.Now });
                                invitedMob.EnqueueMessage($"{performer.KeyWords[0]} has invited you to their party.");
                                return new Result($"{invitedMob.KeyWords[0]} has been invited to the party.", true);
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

        public IResult AcceptPartyInvite(IMobileObject performer)
        {
            lock (padLock)
            {
                for (int i = Invites.Count - 1; i >= 0; i--)
                {
                    IPartyInvite partyInvite = Invites[i];
                    if (partyInvite.Invited == performer)
                    {
                        partyInvite.Group.AddMember(performer);
                        Invites.RemoveAt(i);
                        Groups.Add(performer, partyInvite.Group);

                        return new Result($"You join {partyInvite.PartyLeader.KeyWords[0]}'s party.", true);
                    }
                }
            }

            return new Result($"You do not have any current party invites.", true);
        }

        public IResult DeclinePartyInvite(IMobileObject performer)
        {
            lock (padLock)
            {
                for (int i = Invites.Count - 1; i >= 0; i--)
                {
                    IPartyInvite partyInvite = Invites[i];
                    if (partyInvite.Invited == performer)
                    {
                        Invites.RemoveAt(i);

                        return new Result($"You declined {partyInvite.PartyLeader.KeyWords[0]}'s party invite.", true);
                    }
                }
            }

            return new Result($"You do not have any current party invites.", true);
        }

        public IReadOnlyList<IMobileObject> CurrentPartyMembers(IMobileObject performer)
        {
            lock (padLock)
            {
                if (Groups.TryGetValue(performer, out IGroup performerGroup))
                {
                    return performerGroup.GroupMembers;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
