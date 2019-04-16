using Objects.Command.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Global.Engine.Engines.Interface
{
    public interface IParty
    {
        IResult AcceptPartyInvite(IMobileObject performer);
        IResult DeclinePartyInvite(IMobileObject performer);
        IResult Invite(IMobileObject performer, IMobileObject invitedMob);
        void RemoveOldPartyInvites();
        IReadOnlyList<IMobileObject> CurrentPartyMembers(IMobileObject performer);
    }
}