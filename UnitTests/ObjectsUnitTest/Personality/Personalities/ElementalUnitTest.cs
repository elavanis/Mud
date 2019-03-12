﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Mob.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Personality.Personalities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class ElementalUnitTest
    {
        Elemental elemental;
        Mock<IElemental> elementalMob;
        [TestInitialize]
        public void Setup()
        {
            elementalMob = new Mock<IElemental>();
            elemental = new Elemental();
        }

        [TestMethod]
        public void Elemental_Process()
        {
            elemental.Process(elementalMob.Object, null);

            elementalMob.Verify(e => e.ProcessElementalTick(), Times.Once);
        }
    }
}