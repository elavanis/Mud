using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Engine.Engines;
using Objects.Global.Logging.Interface;
using Objects.Global.Map.Interface;
using Objects.Global.Notify.Interface;
using Objects.Item.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Trap.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Global.Direction.Directions;
using static Objects.Global.Logging.LogSettings;
using static Objects.Trap.Target;
using static Shared.TagWrapper.TagWrapper;

namespace ObjectsUnitTest.Global.Engine
{
    [TestClass]
    public class EventUnitTest
    {
        Event evnt;
        Mock<ITagWrapper> tagWrapper;
        Mock<ILogger> logger;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IItem> item;
        Mock<IRoom> room;
        Mock<IEnchantment> roomEnchantment;
        Mock<ITrap> trap;
        Mock<IEnchantment> trapEnchantment;
        Mock<IEnchantment> pcEnchantment;
        Mock<IEnchantment> npcEnchantment;
        Mock<IEnchantment> itemEnchantment;
        Mock<INotify> notify;
        Mock<IMap> map;

        [TestInitialize]
        public void Setup()
        {
            evnt = new Event();
            tagWrapper = new Mock<ITagWrapper>();
            logger = new Mock<ILogger>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            item = new Mock<IItem>();
            room = new Mock<IRoom>();
            roomEnchantment = new Mock<IEnchantment>();
            trap = new Mock<ITrap>();
            trapEnchantment = new Mock<IEnchantment>();
            pcEnchantment = new Mock<IEnchantment>();
            npcEnchantment = new Mock<IEnchantment>();
            itemEnchantment = new Mock<IEnchantment>();
            notify = new Mock<INotify>();
            map = new Mock<IMap>();

            npc.Setup(e => e.Room).Returns(room.Object);
            npc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { npcEnchantment.Object });
            npc.Setup(e => e.SentenceDescription).Returns("NpcSentence");
            pc.Setup(e => e.Room).Returns(room.Object);
            pc.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { pcEnchantment.Object });
            pc.Setup(e => e.SentenceDescription).Returns("PcSentence");
            item.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { itemEnchantment.Object });
            item.Setup(e => e.SentenceDescription).Returns("ItemSentence");
            trap.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { trapEnchantment.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { roomEnchantment.Object });
            room.Setup(e => e.Traps).Returns(new List<ITrap>() { trap.Object });
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter> { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter> { pc.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>() { item.Object });
            room.Setup(e => e.ToString()).Returns("1-1");
            room.Setup(e => e.SerializedSounds).Returns("SerializedSounds");
            trap.Setup(e => e.Trigger).Returns(TrapTrigger.All);

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.Map = map.Object;
        }

        #region Events
        [TestMethod]
        public void Event_HeartbeatBigTick()
        {
            evnt.HeartbeatBigTick(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.ALL, "Big Heartbeat Tick"), Times.Once);
            roomEnchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
        }


        [TestMethod]
        public void Event_OnDeath()
        {
            tagWrapper.Setup(e => e.WrapInTag("NpcSentence has died.", TagType.Info)).Returns("message");

            evnt.OnDeath(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "Died"), Times.Once);
            roomEnchantment.Verify(e => e.OnDeath(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.OnDeath(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.OnDeath(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.OnDeath(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.OnDeath(npc.Object), Times.Once);
            notify.Verify(e => e.Room(npc.Object, null, room.Object, It.IsAny<ITranslationMessage>(), new List<IMobileObject>() { npc.Object }, false, false), Times.Once);
        }

        [TestMethod]
        public void Event_DamageDealtBeforeDefense()
        {
            evnt.DamageDealtBeforeDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageDealtBeforeDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            npcEnchantment.Verify(e => e.DamageDealtBeforeDefense(npc.Object, pc.Object, 10), Times.Once);
        }

        [TestMethod]
        public void Event_DamageDealtAfterDefense()
        {
            tagWrapper.Setup(e => e.WrapInTag("You hit PcSentence for 10 damage.", TagType.DamageDelt)).Returns("attacker message");
            tagWrapper.Setup(e => e.WrapInTag("NpcSentence hit you for 10 damage.", TagType.DamageReceived)).Returns("defender message");
            tagWrapper.Setup(e => e.WrapInTag("NpcSentence attacked PcSentence for 10 damage.", TagType.Info)).Returns("message");

            evnt.DamageDealtAfterDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageDealtAfterDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            npcEnchantment.Verify(e => e.DamageDealtAfterDefense(npc.Object, pc.Object, 10), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, pc.Object, npc.Object, It.IsAny<ITranslationMessage>(), false, false));
            notify.Verify(e => e.Mob(npc.Object, pc.Object, pc.Object, It.IsAny<ITranslationMessage>(), false, false));
            notify.Verify(e => e.Room(npc.Object, pc.Object, room.Object, It.IsAny<ITranslationMessage>(), new List<IMobileObject>() { npc.Object, pc.Object }, false, false), Times.Once);
        }

        [TestMethod]
        public void Event_DamageReceivedBeforeDefense()
        {
            evnt.DamageReceivedBeforeDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageReceivedBeforeDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            pcEnchantment.Verify(e => e.DamageReceivedBeforeDefense(npc.Object, pc.Object, 10), Times.Once);
        }

        [TestMethod]
        public void Event_DamageReceivedAfterDefense()
        {
            evnt.DamageReceivedAfterDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageReceivedAfterDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            pcEnchantment.Verify(e => e.DamageReceivedAfterDefense(npc.Object, pc.Object, 10), Times.Once);
        }

        [TestMethod]
        public void Event_EnqueueMessage()
        {
            evnt.EnqueueMessage(npc.Object, "message");

            logger.Verify(e => e.Log(npc.Object, LogLevel.ALL, "message"), Times.Once);
        }

        [TestMethod]
        public void Event_ToDodge()
        {
            evnt.ToDodge(npc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence attempted to dodge and rolled 10."), Times.Once);
        }

        [TestMethod]
        public void Event_ToHit()
        {
            evnt.ToHit(npc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence attempted to hit and rolled 10."), Times.Once);
        }
        #endregion Events

        #region Room Enter/Leave
        [TestMethod]
        public void Event_EnterRoom()
        {
            tagWrapper.Setup(e => e.WrapInTag("SerializedSounds", TagType.Sound)).Returns("sounds");

            evnt.EnterRoom(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence entered room 1-1."), Times.Once);

            roomEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            map.Verify(e => e.SendMapPosition(pc.Object));
            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
        }

        [TestMethod]
        public void Event_LeaveRoom()
        {
            evnt.LeaveRoom(npc.Object, Direction.East);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence left room 1-1."), Times.Once);

            roomEnchantment.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
            trapEnchantment.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
            pcEnchantment.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
            npcEnchantment.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
            itemEnchantment.Verify(e => e.LeaveRoom(npc.Object, Direction.East), Times.Once);
        }

        [TestMethod]
        public void Event_AttemptToFollow()
        {
            evnt.AttemptToFollow(Direction.East, npc.Object, pc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence attempted to follow PcSentence East."), Times.Once);

            roomEnchantment.Verify(e => e.AttemptToFollow(Direction.East, npc.Object, pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.AttemptToFollow(Direction.East, npc.Object, pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.AttemptToFollow(Direction.East, npc.Object, pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.AttemptToFollow(Direction.East, npc.Object, pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.AttemptToFollow(Direction.East, npc.Object, pc.Object), Times.Once);
        }
        #endregion Room Enter/Leave

        #region Commands
        [TestMethod]
        public void Event_Cast()
        {
            evnt.Cast(npc.Object, "spell");

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence cast spell."), Times.Once);

            roomEnchantment.Verify(e => e.Cast(npc.Object, "spell"), Times.Once);
            trapEnchantment.Verify(e => e.Cast(npc.Object, "spell"), Times.Once);
            pcEnchantment.Verify(e => e.Cast(npc.Object, "spell"), Times.Once);
            npcEnchantment.Verify(e => e.Cast(npc.Object, "spell"), Times.Once);
            itemEnchantment.Verify(e => e.Cast(npc.Object, "spell"), Times.Once);
        }

        [TestMethod]
        public void Event_Perform()
        {
            evnt.Perform(npc.Object, "skill");

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence performed skill."), Times.Once);

            roomEnchantment.Verify(e => e.Perform(npc.Object, "skill"), Times.Once);
            trapEnchantment.Verify(e => e.Perform(npc.Object, "skill"), Times.Once);
            pcEnchantment.Verify(e => e.Perform(npc.Object, "skill"), Times.Once);
            npcEnchantment.Verify(e => e.Perform(npc.Object, "skill"), Times.Once);
            itemEnchantment.Verify(e => e.Perform(npc.Object, "skill"), Times.Once);
        }

        [TestMethod]
        public void Event_Drop()
        {
            evnt.Drop(npc.Object, item.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence dropped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Drop(npc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Drop(npc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Drop(npc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Drop(npc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Drop(npc.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Get()
        {
            evnt.Get(npc.Object, item.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence got ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Get(npc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Get(npc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Get(npc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Get(npc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Get(npc.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Relax()
        {
            evnt.Relax(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence relaxed."), Times.Once);

            roomEnchantment.Verify(e => e.Relax(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Relax(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Relax(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Relax(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Relax(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Sat()
        {
            evnt.Sit(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence sat."), Times.Once);

            roomEnchantment.Verify(e => e.Sit(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Sit(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Sit(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Sit(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Sit(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Sleep()
        {
            evnt.Sleep(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence slept."), Times.Once);

            roomEnchantment.Verify(e => e.Sleep(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Sleep(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Sleep(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Sleep(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Sleep(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Stand()
        {
            evnt.Stand(npc.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence stood."), Times.Once);

            roomEnchantment.Verify(e => e.Stand(npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Stand(npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Stand(npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Stand(npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Stand(npc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Equip()
        {
            evnt.Equip(npc.Object, item.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence equipped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Equip(npc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Equip(npc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Equip(npc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Equip(npc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Equip(npc.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Unequip()
        {
            evnt.Unequip(npc.Object, item.Object);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUG, "NpcSentence unequipped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Unequip(npc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Unequip(npc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Unequip(npc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Unequip(npc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Unequip(npc.Object, item.Object), Times.Once);
        }
        #endregion Commands

        #region Input/Output
        [TestMethod]
        public void Event_ProcessedCommand()
        {
            evnt.ProcessedCommand(npc.Object, "command");

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence processed command command."), Times.Once);

            roomEnchantment.Verify(e => e.ProcessedCommand(npc.Object, "command"), Times.Once);
            trapEnchantment.Verify(e => e.ProcessedCommand(npc.Object, "command"), Times.Once);
            pcEnchantment.Verify(e => e.ProcessedCommand(npc.Object, "command"), Times.Once);
            npcEnchantment.Verify(e => e.ProcessedCommand(npc.Object, "command"), Times.Once);
            itemEnchantment.Verify(e => e.ProcessedCommand(npc.Object, "command"), Times.Once);
        }

        [TestMethod]
        public void Event_ProcessedCommunication()
        {
            evnt.ProcessedCommunication(npc.Object, "communication");

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence processed communication communication."), Times.Once);

            roomEnchantment.Verify(e => e.ProcessedCommunication(npc.Object, "communication"), Times.Once);
            trapEnchantment.Verify(e => e.ProcessedCommunication(npc.Object, "communication"), Times.Once);
            pcEnchantment.Verify(e => e.ProcessedCommunication(npc.Object, "communication"), Times.Once);
            npcEnchantment.Verify(e => e.ProcessedCommunication(npc.Object, "communication"), Times.Once);
            itemEnchantment.Verify(e => e.ProcessedCommunication(npc.Object, "communication"), Times.Once);
        }

        [TestMethod]
        public void Event_ReturnedMessage()
        {
            evnt.ReturnedMessage(npc.Object, "message");

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "NpcSentence returned message message."), Times.Once);

            roomEnchantment.Verify(e => e.ReturnedMessage(npc.Object, "message"), Times.Once);
            trapEnchantment.Verify(e => e.ReturnedMessage(npc.Object, "message"), Times.Once);
            pcEnchantment.Verify(e => e.ReturnedMessage(npc.Object, "message"), Times.Once);
            npcEnchantment.Verify(e => e.ReturnedMessage(npc.Object, "message"), Times.Once);
            itemEnchantment.Verify(e => e.ReturnedMessage(npc.Object, "message"), Times.Once);
        }
        #endregion Input/Output
    }
}
