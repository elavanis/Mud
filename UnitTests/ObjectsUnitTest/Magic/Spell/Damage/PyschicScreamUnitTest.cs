﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Die.Interface;
using Objects.Global;
using Objects.Global.DefaultValues.Interface;
using Objects.Magic.Spell.Damage;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Magic.Spell.Damage
{
    [TestClass]
    public class PyschicScreamUnitTest
    {
        PyschicScream pyschicScream;
        Mock<IDefaultValues> defaultValues;
        Mock<IDice> dice;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            defaultValues = new Mock<IDefaultValues>();
            dice = new Mock<IDice>();
            tagWrapper = new Mock<ITagWrapper>();

            defaultValues.Setup(e => e.DiceForSpellLevel(70)).Returns(dice.Object);
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));

            GlobalReference.GlobalValues.DefaultValues = defaultValues.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            pyschicScream = new PyschicScream();
        }

        [TestMethod]
        public void PoisonBreath()
        {
            Assert.AreEqual("finish me.", pyschicScream.PerformerNotificationSuccess.Message);
            Assert.AreEqual("finish me", pyschicScream.RoomNotificationSuccess.Message);
            Assert.AreEqual("finish me", pyschicScream.TargetNotificationSuccess.Message);
        }
    }
}