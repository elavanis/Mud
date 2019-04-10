using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Global.Engine.Engines.AdditionalParty
{
    [ExcludeFromCodeCoverage]
    public class PartyInvite : IPartyInvite
    {
        public IMobileObject PartyLeader { get; set; }
        public IGroup Group { get; set; }
        public IMobileObject Invited { get; set; }
        public ulong InviteTurn { get; set; }
    }
}
