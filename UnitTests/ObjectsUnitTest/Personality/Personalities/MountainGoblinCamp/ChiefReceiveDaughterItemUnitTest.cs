using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Personalities.MountainGoblinCamp;
using System.Collections.Generic;

namespace ObjectsUnitTest.Personality.Personalities.MountainGoblinCamp
{
    [TestClass]
    public class ChiefReceiveDaughterItemUnitTest
    {
        Mock<IMobileObject> performer;
        Mock<IMobileObject> receiver;
        Mock<IItem> item;
        IReceiver receiverPersonality;
        List<IItem> items;
        Mock<IBaseObjectId> baseObjectId;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            receiverPersonality = new ChiefReceiveDaughterItem();
            performer = new Mock<IMobileObject>();
            receiver = new Mock<IMobileObject>();
            item = new Mock<IItem>();
            items = new List<IItem>();
            baseObjectId = new Mock<IBaseObjectId>();

            performer.Setup(e => e.Items).Returns(items);
            baseObjectId.Setup(e => e.Id).Returns(1);
            baseObjectId.Setup(e => e.Zone).Returns(2);
            item.Setup(e => e.Id).Returns(1);
            item.Setup(e => e.Zone).Returns(2);
            receiverPersonality.TriggerObjectId = baseObjectId.Object;
            receiverPersonality.ResponseMessage = "response";
            receiverPersonality.Reward = item.Object;
        }

        [TestMethod]
        public void ChiefReceiveDaughter_RecievedItem_Matched()
        {
            IResult result = receiverPersonality.ReceivedItem(performer.Object, receiver.Object, item.Object);

            Assert.AreEqual(null, result.ResultMessage);
            Assert.IsFalse(result.AllowAnotherCommand);
            Assert.AreEqual(1, items.Count);
            receiver.Verify(e => e.EnqueueCommand("say response"), Times.Once);
        }

        [TestMethod]
        public void ChiefReceiveDaughter_RecievedItem_NotMatched()
        {
            item.Setup(e => e.Zone).Returns(3);

            IResult result = receiverPersonality.ReceivedItem(performer.Object, receiver.Object, item.Object);

            Assert.AreEqual(null, result.ResultMessage);
            Assert.IsTrue(result.AllowAnotherCommand);
            Assert.AreEqual(0, items.Count);
            receiver.Verify(e => e.EnqueueCommand("say Thank you for the gift."), Times.Once);
        }
    }
}
