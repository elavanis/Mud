using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.Engine.Engines.AdditionalParty
{
    public class Group : IGroup
    {
        private List<IMobileObject> groupMembers = new List<IMobileObject>();

        public IMobileObject GroupLeader
        {
            get
            {
                lock (groupMembers)
                {
                    if (MemberCount > 0)
                    {
                        return groupMembers[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public int MemberCount
        {
            get
            {
                return groupMembers.Count;
            }
        }

        public IReadOnlyList<IMobileObject> GroupMembers
        {
            get
            {
                lock (groupMembers)
                {
                    return new List<IMobileObject>(groupMembers).AsReadOnly();
                }
            }
        }

        public void AddMember(IMobileObject mobileObject)
        {
            lock (groupMembers)
            {
                groupMembers.Add(mobileObject);
            }
        }

        public void RemoveMember(IMobileObject mobileObject)
        {
            lock (groupMembers)
            {
                groupMembers.Remove(mobileObject);
            }
        }

        public bool IsMember(IMobileObject mobileObject)
        {
            lock (groupMembers)
            {
                return groupMembers.Contains(mobileObject);
            };
        }
    }
}
