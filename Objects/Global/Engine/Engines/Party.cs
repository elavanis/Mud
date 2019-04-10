using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
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

    }
}
