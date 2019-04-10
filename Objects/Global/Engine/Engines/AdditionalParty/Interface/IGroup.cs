using Objects.Mob.Interface;

namespace Objects.Global.Engine.Engines.AdditionalParty.Interface
{
    public interface IGroup
    {
        IMobileObject GroupLeader { get; }
        int MemberCount { get; }

        void AddMember(IMobileObject mobileObject);
        void RemoveMember(IMobileObject mobileObject);
    }
}