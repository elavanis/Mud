using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Objects.Item.Items;

namespace ObjectsUnitTest.Item.Items
{
    [TestClass]
    public class RecallBeaconUnitTest
    {
        RecallBeacon beacon;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            beacon = new RecallBeacon();
        }

        [TestMethod]
        public void RecallBeacon_Constructor()
        {
            Assert.IsTrue(beacon.Attributes.Contains(Objects.Item.Item.ItemAttribute.NoGet));
            Assert.AreEqual("A blue recall beacon hovers in the air.", beacon.ShortDescription);
            Assert.AreEqual("The recall beacon's soft blue color of a noon day sky is contrasted by the extreme hardness of the recall beacon itself.", beacon.LookDescription);
            Assert.AreEqual("The beacon is shaped like a giant nine foot crystal.  It hovers in the air and with effort it can be made to spin but no amount of force has been able to move it. While being mostly translucent if you look closely you can see yourself faintly reflected in its smooth finish.", beacon.ExamineDescription);
            Assert.AreEqual("recall beacon", beacon.SentenceDescription);
            Assert.IsTrue(beacon.KeyWords.Contains("recall"));
            Assert.IsTrue(beacon.KeyWords.Contains("beacon"));
        }
    }
}
