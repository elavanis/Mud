using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;

namespace ObjectsUnitTest.Global
{
    [TestClass]
    public class GlobalValuesUnitTest
    {
        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();
        }

        [TestMethod]
        public void GlobalValues_SetDefaultValues()
        {
            GlobalReference.GlobalValues.Initilize();

            Assert.IsInstanceOfType(GlobalReference.GlobalValues.DefaultValues, typeof(Objects.Global.DefaultValues.DefaultValues));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.Experience, typeof(Objects.Global.Exp.Experience));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.MoneyToCoins, typeof(Objects.Global.MoneyToCoins.MoneyToCoins));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.MultiClassBonus, typeof(Objects.Global.MultiClassBonus.MultiClassBonus));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.Random, typeof(Objects.Global.Random.Random));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.Settings, typeof(Objects.Global.Settings.Settings));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.TagWrapper, typeof(Shared.TagWrapper.TagWrapper));
            Assert.IsInstanceOfType(GlobalReference.GlobalValues.Translator, typeof(Objects.Global.Language.Translator));
        }
    }
}
