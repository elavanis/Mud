using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ObjectsUnitTest.Global.Engine.AdditionalParty
{
    [TestClass]
    public class GroupUnitTest
    {
        Objects.Global.Engine.Engines.AdditionalParty.Group group;
        List<IMobileObject> groupMembers;
        Mock<IMobileObject> mob1;
        Mock<IMobileObject> mob2;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            group = new Objects.Global.Engine.Engines.AdditionalParty.Group();
            FieldInfo groupMembersFieldInfo = group.GetType().GetField("groupMembers", BindingFlags.NonPublic | BindingFlags.Instance);
            groupMembers = (List<IMobileObject>)groupMembersFieldInfo.GetValue(group);
            mob1 = new Mock<IMobileObject>();
            mob2 = new Mock<IMobileObject>();

            groupMembers.Add(mob1.Object);
            groupMembers.Add(mob2.Object);
        }

        [TestMethod]
        public void Group_GroupLeader()
        {
            Assert.AreEqual(mob1.Object, group.GroupLeader);
        }

        [TestMethod]
        public void Group_MemberCount()
        {
            Assert.AreEqual(2, group.MemberCount);
        }

        [TestMethod]
        public void Group_AddMember()
        {
            Mock<IMobileObject> mob3 = new Mock<IMobileObject>();

            group.AddMember(mob3.Object);

            Assert.AreEqual(3, groupMembers.Count);
        }

        [TestMethod]
        public void Group_RemoveMember()
        {
            group.RemoveMember(mob2.Object);

            Assert.AreEqual(1, groupMembers.Count);
        }

        [TestMethod]
        public void Group_NoMembersPresent()
        {
            groupMembers.Clear();

            Assert.AreEqual(null, group.GroupLeader);
        }

        [TestMethod]
        public void Group_GroupMembers()
        {
            Assert.AreEqual(2, group.GroupMembers.Count);
            foreach (var item in group.GroupMembers)
            {
                Assert.IsTrue(mob1.Object == item
                    || mob2.Object == item);
            }
        }

        [TestMethod]
        public void Group_IsMember()
        {
            groupMembers.Remove(mob1.Object);
            Assert.IsFalse(group.IsMember(mob1.Object));
            Assert.IsTrue(group.IsMember(mob2.Object));
        }
    }
}
