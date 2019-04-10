using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;

namespace Objects.Global.Engine.Engines.AdditionalParty.Interface
{
    public interface IPartyInvite
    {
        IGroup Group { get; set; }
        IMobileObject Invited { get; set; }
        ulong InviteTurn { get; set; }
        IMobileObject PartyLeader { get; set; }
    }
}