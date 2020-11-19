using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Mob.MobileObject;

namespace ObjectsUnitTest.Command
{
    [TestClass]
    public class BaseMobileObjectComandUnitTest
    {
        [TestMethod]
        public void BaseMobileObjectComand_Constructor_PassedHashSet()
        {
            string commandName = "command";
            HashSet<CharacterPosition> characterPositions = new HashSet<CharacterPosition>();
            BaseMobileObjectCommandHelper baseMobileObjectCommandHelper = new BaseMobileObjectCommandHelper(commandName, characterPositions);

            Assert.AreSame(commandName, baseMobileObjectCommandHelper.CommandName);
            Assert.AreSame(characterPositions, baseMobileObjectCommandHelper.AllowedCharacterPositions);
        }

        [TestMethod]
        public void BaseMobileObjectComand_Constructor_Any()
        {
            string commandName = "command";
            BaseMobileObjectCommandHelper baseMobileObjectCommandHelper = new BaseMobileObjectCommandHelper(commandName, BaseMobileObjectComand.ShortCutCharPositions.Any);

            Assert.AreSame(commandName, baseMobileObjectCommandHelper.CommandName);

            foreach (CharacterPosition position in Enum.GetValues(typeof(CharacterPosition)))
            {
                Assert.IsTrue(baseMobileObjectCommandHelper.AllowedCharacterPositions.Contains(position));
            }
        }

        [TestMethod]
        public void BaseMobileObjectComand_Constructor_Awake()
        {
            string commandName = "command";
            BaseMobileObjectCommandHelper baseMobileObjectCommandHelper = new BaseMobileObjectCommandHelper(commandName, BaseMobileObjectComand.ShortCutCharPositions.Awake);

            Assert.AreSame(commandName, baseMobileObjectCommandHelper.CommandName);

            foreach (CharacterPosition position in Enum.GetValues(typeof(CharacterPosition)))
            {
                switch (position)
                {
                    case CharacterPosition.Sleep:
                        Assert.IsFalse(baseMobileObjectCommandHelper.AllowedCharacterPositions.Contains(position));
                        break;
                    case CharacterPosition.Mounted:
                    case CharacterPosition.Relax:
                    case CharacterPosition.Sit:
                    case CharacterPosition.Stand:
                        Assert.IsTrue(baseMobileObjectCommandHelper.AllowedCharacterPositions.Contains(position));
                        break;
                    default:
                        throw new Exception("unknown character position");
                }
            }
        }

        [TestMethod]
        public void BaseMobileObjectComand_Constructor_Standinng()
        {
            string commandName = "command";
            BaseMobileObjectCommandHelper baseMobileObjectCommandHelper = new BaseMobileObjectCommandHelper(commandName, BaseMobileObjectComand.ShortCutCharPositions.Standing);

            Assert.AreSame(commandName, baseMobileObjectCommandHelper.CommandName);

            foreach (CharacterPosition position in Enum.GetValues(typeof(CharacterPosition)))
            {
                switch (position)
                {
                    case CharacterPosition.Sleep:
                    case CharacterPosition.Relax:
                    case CharacterPosition.Sit:
                        Assert.IsFalse(baseMobileObjectCommandHelper.AllowedCharacterPositions.Contains(position));
                        break;
                    case CharacterPosition.Mounted:
                    case CharacterPosition.Stand:
                        Assert.IsTrue(baseMobileObjectCommandHelper.AllowedCharacterPositions.Contains(position));
                        break;
                    default:
                        throw new Exception("unknown character position");
                }
            }
        }
    }

    public class BaseMobileObjectCommandHelper : BaseMobileObjectComand
    {
        public BaseMobileObjectCommandHelper(string commandName, HashSet<CharacterPosition> characterPositions) : base(commandName, characterPositions)
        {

        }

        public BaseMobileObjectCommandHelper(string commandName, ShortCutCharPositions shortCutCharPositions) : base(commandName, shortCutCharPositions)
        {

        }
    }
}
