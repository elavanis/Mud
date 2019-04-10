using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.Engine.Engines.AdditionalParty
{
    public class Group : IGroup
    {
        private List<IMobileObject> GroupMembers { get; } = new List<IMobileObject>();

        public IMobileObject GroupLeader
        {
            get
            {
                if (MemberCount > 0)
                {
                    return GroupMembers[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public int MemberCount
        {
            get
            {
                return GroupMembers.Count;
            }
        }

        public void AddMember(IMobileObject mobileObject)
        {
            lock (GroupMembers)
            {
                GroupMembers.Add(mobileObject);
            }
        }

        public void RemoveMember(IMobileObject mobileObject)
        {
            lock (GroupMembers)
            {
                GroupMembers.Remove(mobileObject);
            }
        }

    }
}
