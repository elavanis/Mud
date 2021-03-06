﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            GlobalReference.GlobalValues = new GlobalValues();

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
        public void Party_RemoveOldPartyInvites()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.PartyLeader).Returns(performer.Object);
            partyInvite.Setup(e => e.Invited).Returns(performer.Object);
            partyInvite.Setup(e => e.InviteTime).Returns(new DateTime());
            invites.Add(partyInvite.Object);

            partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.PartyLeader).Returns(invited.Object);
            partyInvite.Setup(e => e.Invited).Returns(invited.Object);
            partyInvite.Setup(e => e.InviteTime).Returns(DateTime.UtcNow);
            invites.Add(partyInvite.Object);

            party.RemoveOldPartyInvites();

            Assert.AreEqual(1, invites.Count);
            Assert.AreEqual(invited.Object, invites[0].Invited);
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
            Assert.IsTrue(DateTime.Now.Subtract(invites[0].InviteTime).TotalSeconds <= 1);
            Assert.AreEqual(performer.Object, invites[0].PartyLeader);
        }

        [TestMethod]
        public void Party_Invite_AlreadyInvited()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.Invited).Returns(invited.Object);
            invites.Add(partyInvite.Object);

            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("invited already has an invite and will need to either accept or deny first.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_Invite_AlreadyInAParty()
        {
            group.Setup(e => e.IsMember(invited.Object)).Returns(true);
            groups.Add(invited.Object, group.Object);

            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("invited is already in a party and will need to leave before you can invite them.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_Invite_NotPartyLeader()
        {
            group.Setup(e => e.GroupLeader).Returns(invited.Object);

            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("You need to be the party leader to invite people.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_Invite_NoParty()
        {
            groups.Clear();

            IResult result = party.Invite(performer.Object, invited.Object);

            Assert.AreEqual("You need to start a party before you can invite someone.", result.ResultMessage);
            Assert.AreEqual(true, result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_AcceptInvite_Accepted()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.Group).Returns(group.Object);
            partyInvite.Setup(e => e.PartyLeader).Returns(performer.Object);
            partyInvite.Setup(e => e.Invited).Returns(invited.Object);
            invites.Add(partyInvite.Object);

            IResult result = party.AcceptPartyInvite(invited.Object);

            Assert.AreEqual(0, invites.Count);
            Assert.AreEqual(group.Object, groups[invited.Object]);
            group.Verify(e => e.AddMember(invited.Object), Times.Once);
            Assert.AreEqual("You join performer's party.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_AcceptInvite_NoInvite()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.Group).Returns(group.Object);
            partyInvite.Setup(e => e.PartyLeader).Returns(performer.Object);
            partyInvite.Setup(e => e.Invited).Returns(performer.Object);
            invites.Add(partyInvite.Object);

            IResult result = party.AcceptPartyInvite(invited.Object);

            Assert.AreEqual(1, invites.Count);
            Assert.IsFalse(groups.ContainsKey(invited.Object));
            group.Verify(e => e.AddMember(invited.Object), Times.Never);
            Assert.AreEqual("You do not have any current party invites.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_DeclineInvite_Success()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.Group).Returns(group.Object);
            partyInvite.Setup(e => e.PartyLeader).Returns(performer.Object);
            partyInvite.Setup(e => e.Invited).Returns(invited.Object);
            invites.Add(partyInvite.Object);

            IResult result = party.DeclinePartyInvite(invited.Object);

            Assert.AreEqual(0, invites.Count);
            Assert.AreEqual("You declined performer's party invite.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_DeclineInvite_NoInvite()
        {
            Mock<IPartyInvite> partyInvite = new Mock<IPartyInvite>();
            partyInvite.Setup(e => e.Group).Returns(group.Object);
            partyInvite.Setup(e => e.PartyLeader).Returns(performer.Object);
            partyInvite.Setup(e => e.Invited).Returns(performer.Object);
            invites.Add(partyInvite.Object);

            IResult result = party.DeclinePartyInvite(invited.Object);

            Assert.AreEqual(1, invites.Count);
            Assert.AreEqual("You do not have any current party invites.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }

        [TestMethod]
        public void Party_CurrentPartyMembers_InParty()
        {
            List<IMobileObject> expectedList = new List<IMobileObject>() { performer.Object, invited.Object };
            group.Setup(e => e.GroupMembers).Returns(expectedList);

            IReadOnlyList<IMobileObject> partyMembers = party.CurrentPartyMembers(performer.Object);
            Assert.AreSame(expectedList, partyMembers);
        }

        [TestMethod]
        public void Party_CurrentPartyMembers_NotInParty()
        {
            IReadOnlyList<IMobileObject> partyMembers = party.CurrentPartyMembers(invited.Object);
            Assert.IsNull(partyMembers);
        }

        [TestMethod]
        public void Party_Start_NotInParty()
        {
            groups.Clear();

            IResult result = party.Start(performer.Object);
            Assert.AreEqual("You started a party and are the leader, now invite other members.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.IsTrue(groups.ContainsKey(performer.Object));
        }

        [TestMethod]
        public void Party_Start_InParty()
        {
            IResult result = party.Start(performer.Object);
            Assert.AreEqual("You can not start a party as you are already in one.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.IsTrue(groups.ContainsKey(performer.Object));
        }

        [TestMethod]
        public void Party_Leave_NotInParty()
        {
            groups.Clear();

            IResult result = party.Leave(performer.Object);
            Assert.AreEqual("You are not in a party.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.IsFalse(groups.ContainsKey(performer.Object));
        }

        [TestMethod]
        public void Party_Leave_InParty()
        {
            IResult result = party.Leave(performer.Object);
            Assert.AreEqual("You left the party.", result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.IsFalse(groups.ContainsKey(performer.Object));
        }
    }
}
