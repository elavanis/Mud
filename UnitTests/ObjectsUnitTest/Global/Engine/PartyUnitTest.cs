using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Engine.Engines;
using Objects.Global.Engine.Engines.AdditionalParty.Interface;
using Objects.Mob.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Global.Engine
{
    [TestClass]
    public class PartyUnitTest
    {
        Party party;
        Dictionary<IMobileObject, IGroup> groups;
        List<IPartyInvite> invites;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> invited;
        Mock<ITagWrapper> tagWrapper;
        Mock<IGroup> group;


        [TestInitialize]
        public void Setup()
        {
            party = new Party();
            performer = new Mock<IMobileObject>();
            invited = new Mock<IMobileObject>();
            tagWrapper = new Mock<ITagWrapper>();
            group = new Mock<IGroup>();

            PropertyInfo propertyInfo = party.GetType().GetProperty("Groups", BindingFlags.NonPublic | BindingFlags.Instance);
            groups = (Dictionary<IMobileObject, IGroup>)propertyInfo.GetValue(party);
            propertyInfo = party.GetType().GetProperty("Invites", BindingFlags.NonPublic | BindingFlags.Instance);
            invites = (List<IPartyInvite>)propertyInfo.GetValue(party);

            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            group.Setup(e => e.GroupLeader).Returns(performer.Object);
            invited.Setup(e => e.KeyWords).Returns(new List<string> { "invited" });
            performer.Setup(e => e.KeyWords).Returns(new List<string> { "performer" });

            groups.Add(performer.Object, group.Object);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.TickCounter = 100;
        }

        [TestMethod]
        public void Party_WriteUnitTests()
        {
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void Party_Invite_Success()
        {
            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("invited has been invited to the party.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
            invited.Verify(e => e.EnqueueMessage("performer has invited you to their party."), Times.Once);
            Assert.AreEqual(1, invites.Count);
            Assert.AreEqual(group.Object, invites[0].Group);
            Assert.AreEqual(invited.Object, invites[0].Invited);
            Assert.AreEqual(100ul, invites[0].InviteTurn);
            Assert.AreEqual(performer.Object, invites[0].PartyLeader);
        }

        [TestMethod]
        public void Party_Invite_NoParty()
        {
            groups.Clear();

            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("You need to start a party before you can invite someone.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
        }
    }
}
