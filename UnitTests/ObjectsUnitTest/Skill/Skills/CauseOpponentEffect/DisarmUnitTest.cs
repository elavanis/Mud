using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Skill.Skills.CauseOpponentEffect;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Skill.Skills.CauseOpponentEffect
{
    [TestClass]

    public class DisarmUnitTest
    {
        Disarm disarm;
        Mock<IDefaultValues> defaultValue;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IMobileObject> target;
        Mock<IWeapon> weapon;
        Mock<IRandom> random;
        List<IItem> heldItems;
        Mock<ICommand> command;
        Mock<IParameter> parameterCommandName;
        Mock<IParameter> parameterTarget;
        Mock<IFindObjects> findObjects;
        Mock<INotify> notify;
        Mock<IRoom> room;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            defaultValue = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();
            weapon = new Mock<IWeapon>();
            target = new Mock<IMobileObject>();
            performer = new Mock<IMobileObject>();
            random = new Mock<IRandom>();
            heldItems = new List<IItem>();
            command = new Mock<ICommand>();
            parameterCommandName = new Mock<IParameter>();
            parameterTarget = new Mock<IParameter>();
            findObjects = new Mock<IFindObjects>();
            notify = new Mock<INotify>();
            room = new Mock<IRoom>();
            stringManipulator = new Mock<IStringManipulator>();

            defaultValue.Setup(e => e.DiceForSkillLevel(80)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            target.Setup(e => e.EquipedWeapon).Returns(new List<IWeapon>() { weapon.Object });
            target.Setup(e => e.Items).Returns(heldItems);
            target.Setup(e => e.StrengthEffective).Returns(5);
            target.Setup(e => e.SentenceDescription).Returns("target");
            performer.Setup(e => e.Stamina).Returns(200);
            performer.Setup(e => e.StrengthEffective).Returns(10);
            performer.Setup(e => e.Room).Returns(room.Object);
            performer.Setup(e => e.SentenceDescription).Returns("performer");
            weapon.Setup(e => e.KeyWords).Returns(new List<string>() { "Sword" });
            random.SetupSequence(e => e.Next(0)).Returns(1).Returns(2);
            random.Setup(e => e.Next(It.IsAny<int>())).Returns((int x) => (x));
            command.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameterCommandName.Object, parameterTarget.Object });
            parameterCommandName.Setup(e => e.ParameterValue).Returns("commandName");
            parameterTarget.Setup(e => e.ParameterValue).Returns("target");
            findObjects.Setup(e => e.FindObjectOnPersonOrInRoom(performer.Object, parameterTarget.Object.ParameterValue, 0, true, true, true, true, true)).Returns(target.Object);
            stringManipulator.Setup(e => e.UpdateTargetPerformer("performer", "target", "You try to stun {target} causing them to drop their weapon but fail.")).Returns("message");

            GlobalReference.GlobalValues.DefaultValues = defaultValue.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.FindObjects = findObjects.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;

            disarm = new Disarm();
        }

        [TestMethod]
        public void Disarm_TeachMessage()
        {
            string expected = "If your opponent has no weapon then they can't hurt you.";
            Assert.AreEqual(expected, disarm.TeachMessage);
        }

        [TestMethod]
        public void Disarm_RequirementsFailureMessage()
        {
            IResult result = disarm.RequirementsFailureMessage;
            string expected = "You can not disarm an unarmed opponent.";
            Assert.AreEqual(expected, result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
        }


        [TestMethod]
        public void Disarm_ProcessSkill_DoesNotMeetRequirments()
        {
            weapon.Setup(e => e.KeyWords).Returns(new List<string>() { "BareHands" });

            IResult result = disarm.ProcessSkill(performer.Object, command.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You can not disarm an unarmed opponent.", result.ResultMessage);
            performer.VerifySet(e => e.Stamina = It.IsAny<int>(), Times.Never);
        }

        [TestMethod]
        public void Disarm_ProcessSkill_NotEnoughStamina()
        {
            performer.Setup(e => e.Stamina).Returns(199);

            IResult result = disarm.ProcessSkill(performer.Object, command.Object);

            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual("You need 200 stamina to use the skill commandName.", result.ResultMessage);
            performer.VerifySet(e => e.Stamina = It.IsAny<int>(), Times.Never);
        }

        [TestMethod]
        public void Disarm_ProcessSkill_IsNotSuccessful()
        {
            target.Setup(e => e.StrengthEffective).Returns(20);

            IResult result = disarm.ProcessSkill(performer.Object, command.Object);

            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual("message", result.ResultMessage);
            performer.VerifySet(e => e.Stamina = 0);
            notify.Verify(e => e.Mob(target.Object, disarm.TargetNotificationFailure), Times.Once);
            notify.Verify(e => e.Room(performer.Object, target.Object, room.Object, disarm.RoomNotificationFailure, new List<IMobileObject>() { performer.Object, target.Object }, false, false));
        }

        [TestMethod]
        public void Disarm_WriteTests()
        {
            Assert.AreEqual(1, 2);
        }

        //[TestMethod]
        //public void Disarm_MeetRequirments_True()
        //{
        //    Assert.IsTrue(disarm.MeetRequirments(performer.Object, target.Object));
        //}




        //[TestMethod]
        //public void Disarm_IsSuccessful_True()
        //{
        //    random.SetupSequence(e => e.Next(0)).Returns(2).Returns(1);
        //    Assert.IsTrue(disarm.IsSuccessful(performer.Object, target.Object));
        //}

        //[TestMethod]
        //public void Disarm_IsSuccessful_False()
        //{
        //    Assert.IsFalse(disarm.IsSuccessful(performer.Object, target.Object));
        //}

        //[TestMethod]
        //public void Disarm_AdditionalEffect()
        //{
        //    disarm.AdditionalEffect(performer.Object, target.Object);

        //    target.Verify(e => e.RemoveEquipment(weapon.Object), Times.Once);
        //}
    }
}