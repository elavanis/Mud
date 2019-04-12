using Objects.Mob.Interface;
using System.Collections.Generic;

namespace Objects.Global.Engine.Engines.AdditionalParty.Interface
{
    public interface IGroup
    {
        IMobileObject GroupLeader { get; }
        int MemberCount { get; }
        IReadOnlyList<IMobileObject> GroupMembers { get; }

        void AddMember(IMobileObject mobileObject);
        void RemoveMember(IMobileObject mobileObject);
        bool IsMember(IMobileObject mobileObject);
    }
}