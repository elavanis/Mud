using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;

namespace Objects.Global.Engine.Engines.AdditionalParty.Interface
{
    public interface IPartyInvite
    {
        IGroup Group { get; set; }
        IMobileObject Invited { get; set; }
        DateTime InviteTime { get; set; }
        IMobileObject PartyLeader { get; set; }
    }
}