using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Engine.Engines;
using Objects.Global.Logging.Interface;
using Objects.Global.Map.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.StringManuplation.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Trap.Interface;
using Shared.TagWrapper.Interface;
using System.Collections.Generic;
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
        Mock<IEnchantment> itemContainerEnchantment;
        Mock<INotify> notify;
        Mock<IMap> map;
        Mock<IContainer> container;
        Mock<IBaseObject> baseObjectContainer;
        Mock<IItem> itemContainer;
        Mock<IStringManipulator> stringManipulator;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

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
            itemContainerEnchantment = new Mock<IEnchantment>();
            notify = new Mock<INotify>();
            map = new Mock<IMap>();
            container = new Mock<IContainer>();
            baseObjectContainer = container.As<IBaseObject>();
            itemContainer = baseObjectContainer.As<IItem>();
            stringManipulator = new Mock<IStringManipulator>();

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
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Info)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.DamageDelt)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.DamageReceived)).Returns((string x, TagType y) => (x));
            tagWrapper.Setup(e => e.WrapInTag(It.IsAny<string>(), TagType.Sound)).Returns((string x, TagType y) => (x));
            baseObjectContainer.Setup(e => e.SentenceDescription).Returns("ContainerSentence");
            itemContainer.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { itemContainerEnchantment.Object });
            stringManipulator.Setup(e => e.CapitalizeFirstLetter("PcSentence")).Returns("PcSentence");

            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.Map = map.Object;
            GlobalReference.GlobalValues.StringManipulator = stringManipulator.Object;
        }

        #region Events
        [TestMethod]
        public void Event_HeartbeatBigTick()
        {
            evnt.HeartbeatBigTick(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.ALL, "Big Heartbeat Tick"), Times.Once);
            roomEnchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.HeartbeatBigTick(pc.Object), Times.Once);
        }


        [TestMethod]
        public void Event_OnDeath()
        {
            evnt.OnDeath(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "Died"), Times.Once);
            roomEnchantment.Verify(e => e.OnDeath(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.OnDeath(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.OnDeath(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.OnDeath(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.OnDeath(pc.Object), Times.Once);
            notify.Verify(e => e.Room(pc.Object, null, room.Object, It.Is<ITranslationMessage>(f => f.Message == "PcSentence has died."), new List<IMobileObject>() { pc.Object }, false, false), Times.Once);
        }

        [TestMethod]
        public void Event_DamageDealtBeforeDefense()
        {
            evnt.DamageBeforeDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageDealtBeforeDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            npcEnchantment.Verify(e => e.DamageBeforeDefense(npc.Object, pc.Object, 10), Times.Once);
        }

        [TestMethod]
        public void Event_DamageDealtAfterDefense()
        {
            evnt.DamageAfterDefense(npc.Object, pc.Object, 10);

            logger.Verify(e => e.Log(npc.Object, LogLevel.DEBUGVERBOSE, "DamageDealtAfterDefense: Attacker-NpcSentence Defender-PcSentence DamageAmount-10."), Times.Once);
            npcEnchantment.Verify(e => e.DamageAfterDefense(npc.Object, pc.Object, 10), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, pc.Object, npc.Object, It.Is<ITranslationMessage>(f => f.Message == "You hit {target} for 10 damage."), false, false));
            notify.Verify(e => e.Mob(npc.Object, pc.Object, pc.Object, It.Is<ITranslationMessage>(f => f.Message == "{performer} hit you for 10 damage."), false, false));
            notify.Verify(e => e.Room(npc.Object, pc.Object, room.Object, It.Is<ITranslationMessage>(f => f.Message == "NpcSentence attacked PcSentence for 10 damage."), new List<IMobileObject>() { npc.Object, pc.Object }, false, false), Times.Once);
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
            evnt.EnterRoom(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence entered room 1-1."), Times.Once);

            roomEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.EnterRoom(pc.Object), Times.Once);
            map.Verify(e => e.SendMapPosition(pc.Object));
            notify.Verify(e => e.Mob(pc.Object, It.Is<ITranslationMessage>(f => f.Message == "SerializedSounds")), Times.Once);
        }

        [TestMethod]
        public void Event_LeaveRoom()
        {
            evnt.LeaveRoom(pc.Object, Direction.East);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence left room 1-1."), Times.Once);

            roomEnchantment.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            trapEnchantment.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            pcEnchantment.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            npcEnchantment.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
            itemEnchantment.Verify(e => e.LeaveRoom(pc.Object, Direction.East), Times.Once);
        }

        [TestMethod]
        public void Event_AttemptToFollow()
        {
            evnt.AttemptToFollow(Direction.East, pc.Object, npc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence attempted to follow NpcSentence East."), Times.Once);

            roomEnchantment.Verify(e => e.AttemptToFollow(Direction.East, pc.Object, npc.Object), Times.Once);
            trapEnchantment.Verify(e => e.AttemptToFollow(Direction.East, pc.Object, npc.Object), Times.Once);
            pcEnchantment.Verify(e => e.AttemptToFollow(Direction.East, pc.Object, npc.Object), Times.Once);
            npcEnchantment.Verify(e => e.AttemptToFollow(Direction.East, pc.Object, npc.Object), Times.Once);
            itemEnchantment.Verify(e => e.AttemptToFollow(Direction.East, pc.Object, npc.Object), Times.Once);
        }
        #endregion Room Enter/Leave

        #region Commands
        [TestMethod]
        public void Event_Cast()
        {
            evnt.Cast(pc.Object, "spell");

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence cast spell."), Times.Once);

            roomEnchantment.Verify(e => e.Cast(pc.Object, "spell"), Times.Once);
            trapEnchantment.Verify(e => e.Cast(pc.Object, "spell"), Times.Once);
            pcEnchantment.Verify(e => e.Cast(pc.Object, "spell"), Times.Once);
            npcEnchantment.Verify(e => e.Cast(pc.Object, "spell"), Times.Once);
            itemEnchantment.Verify(e => e.Cast(pc.Object, "spell"), Times.Once);
        }

        [TestMethod]
        public void Event_Perform()
        {
            evnt.Perform(pc.Object, "skill");

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence performed skill."), Times.Once);

            roomEnchantment.Verify(e => e.Perform(pc.Object, "skill"), Times.Once);
            trapEnchantment.Verify(e => e.Perform(pc.Object, "skill"), Times.Once);
            pcEnchantment.Verify(e => e.Perform(pc.Object, "skill"), Times.Once);
            npcEnchantment.Verify(e => e.Perform(pc.Object, "skill"), Times.Once);
            itemEnchantment.Verify(e => e.Perform(pc.Object, "skill"), Times.Once);
        }

        [TestMethod]
        public void Event_Drop()
        {
            evnt.Drop(pc.Object, item.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence dropped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Drop(pc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Drop(pc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Drop(pc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Drop(pc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Drop(pc.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Get()
        {
            evnt.Get(pc.Object, item.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence got ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Get(pc.Object, item.Object, null), Times.Once);
            trapEnchantment.Verify(e => e.Get(pc.Object, item.Object, null), Times.Once);
            pcEnchantment.Verify(e => e.Get(pc.Object, item.Object, null), Times.Once);
            npcEnchantment.Verify(e => e.Get(pc.Object, item.Object, null), Times.Once);
            itemEnchantment.Verify(e => e.Get(pc.Object, item.Object, null), Times.Once);
        }

        [TestMethod]
        public void Event_Open()
        {
            evnt.Open(pc.Object, itemContainer.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence opened ContainerSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
            trapEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
            pcEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
            npcEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
            itemEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
            itemContainerEnchantment.Verify(e => e.Open(pc.Object, itemContainer.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Put()
        {
            evnt.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence put ItemSentence in ContainerSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object), Times.Once);
            trapEnchantment.Verify(e => e.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object), Times.Once);
            pcEnchantment.Verify(e => e.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object), Times.Once);
            npcEnchantment.Verify(e => e.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object), Times.Once);
            itemEnchantment.Verify(e => e.Put(pc.Object, item.Object, (IContainer)baseObjectContainer.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Relax()
        {
            evnt.Relax(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence relaxed."), Times.Once);

            roomEnchantment.Verify(e => e.Relax(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Relax(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Relax(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Relax(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Relax(pc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Sat()
        {
            evnt.Sit(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence sat."), Times.Once);

            roomEnchantment.Verify(e => e.Sit(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Sit(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Sit(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Sit(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Sit(pc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Sleep()
        {
            evnt.Sleep(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence slept."), Times.Once);

            roomEnchantment.Verify(e => e.Sleep(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Sleep(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Sleep(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Sleep(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Sleep(pc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Stand()
        {
            evnt.Stand(pc.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence stood."), Times.Once);

            roomEnchantment.Verify(e => e.Stand(pc.Object), Times.Once);
            trapEnchantment.Verify(e => e.Stand(pc.Object), Times.Once);
            pcEnchantment.Verify(e => e.Stand(pc.Object), Times.Once);
            npcEnchantment.Verify(e => e.Stand(pc.Object), Times.Once);
            itemEnchantment.Verify(e => e.Stand(pc.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Equip()
        {
            evnt.Equip(pc.Object, item.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence equipped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Equip(pc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Equip(pc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Equip(pc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Equip(pc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Equip(pc.Object, item.Object), Times.Once);
        }

        [TestMethod]
        public void Event_Unequip()
        {
            evnt.Unequip(pc.Object, item.Object);

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUG, "PcSentence unequipped ItemSentence."), Times.Once);

            roomEnchantment.Verify(e => e.Unequip(pc.Object, item.Object), Times.Once);
            trapEnchantment.Verify(e => e.Unequip(pc.Object, item.Object), Times.Once);
            pcEnchantment.Verify(e => e.Unequip(pc.Object, item.Object), Times.Once);
            npcEnchantment.Verify(e => e.Unequip(pc.Object, item.Object), Times.Once);
            itemEnchantment.Verify(e => e.Unequip(pc.Object, item.Object), Times.Once);
        }
        #endregion Commands

        #region Input/Output
        [TestMethod]
        public void Event_ProcessedCommand()
        {
            evnt.ProcessedCommand(pc.Object, "command");

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence processed command command."), Times.Once);

            roomEnchantment.Verify(e => e.ProcessedCommand(pc.Object, "command"), Times.Once);
            trapEnchantment.Verify(e => e.ProcessedCommand(pc.Object, "command"), Times.Once);
            pcEnchantment.Verify(e => e.ProcessedCommand(pc.Object, "command"), Times.Once);
            npcEnchantment.Verify(e => e.ProcessedCommand(pc.Object, "command"), Times.Once);
            itemEnchantment.Verify(e => e.ProcessedCommand(pc.Object, "command"), Times.Once);
        }

        [TestMethod]
        public void Event_ProcessedCommunication()
        {
            evnt.ProcessedCommunication(pc.Object, "communication");

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence processed communication communication."), Times.Once);

            roomEnchantment.Verify(e => e.ProcessedCommunication(pc.Object, "communication"), Times.Once);
            trapEnchantment.Verify(e => e.ProcessedCommunication(pc.Object, "communication"), Times.Once);
            pcEnchantment.Verify(e => e.ProcessedCommunication(pc.Object, "communication"), Times.Once);
            npcEnchantment.Verify(e => e.ProcessedCommunication(pc.Object, "communication"), Times.Once);
            itemEnchantment.Verify(e => e.ProcessedCommunication(pc.Object, "communication"), Times.Once);
        }

        [TestMethod]
        public void Event_ReturnedMessage()
        {
            evnt.ReturnedMessage(pc.Object, "message");

            logger.Verify(e => e.Log(pc.Object, LogLevel.DEBUGVERBOSE, "PcSentence returned message message."), Times.Once);

            roomEnchantment.Verify(e => e.ReturnedMessage(pc.Object, "message"), Times.Once);
            trapEnchantment.Verify(e => e.ReturnedMessage(pc.Object, "message"), Times.Once);
            pcEnchantment.Verify(e => e.ReturnedMessage(pc.Object, "message"), Times.Once);
            npcEnchantment.Verify(e => e.ReturnedMessage(pc.Object, "message"), Times.Once);
            itemEnchantment.Verify(e => e.ReturnedMessage(pc.Object, "message"), Times.Once);
        }
        #endregion Input/Output
    }
}
