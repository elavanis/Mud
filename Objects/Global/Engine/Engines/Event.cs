using System.Collections.Generic;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Interface;
using Objects.Global.Direction;
using Objects.Item.Interface;
using Objects.Global.Engine.Engines.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Magic.Interface;
using Objects.Room.Interface;
using static Objects.Global.Logging.LogSettings;
using static Shared.TagWrapper.TagWrapper;
using Objects.Trap.Interface;
using Objects.Language.Interface;
using Objects.Language;
using Objects.Item.Items.Interface;

namespace Objects.Global.Engine.Engines
{
    public class Event : IEvent
    {
        #region Events
        public void HeartbeatBigTick(IBaseObject performer)
        {
            if (performer is IMobileObject mob)
            {
                GlobalReference.GlobalValues.Logger.Log(mob, LogLevel.ALL, "Big Heartbeat Tick");

                RunEnchantments(mob, EventType.HeartbeatBigTick, new EventParamerters() { Performer = mob });
            }
        }

        public void OnDeath(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, "Died");
            RunEnchantments(performer, EventType.OnDeath, new EventParamerters() { Performer = performer });

            string message = string.Format($"{performer.SentenceDescription} has died.");
            ITranslationMessage translationMessage = new TranslationMessage(message);
            GlobalReference.GlobalValues.Notify.Room(performer, null, performer.Room, translationMessage, new List<IMobileObject>() { performer });
        }

        public void DamageDealtBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            string message = $"DamageDealtBeforeDefense: Attacker-{attacker?.SentenceDescription ?? "unknown"} Defender-{defender.SentenceDescription} DamageAmount-{damageAmount}.";
            GlobalReference.GlobalValues.Logger.Log(attacker, LogLevel.DEBUGVERBOSE, message);

            RunEnchantments(attacker, EventType.DamageDealtBeforeDefense, new EventParamerters() { Attacker = attacker, Defender = defender, DamageAmount = damageAmount });
        }

        public void DamageDealtAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            string message = $"DamageDealtAfterDefense: Attacker-{attacker?.SentenceDescription ?? "unknown"} Defender-{defender.SentenceDescription} DamageAmount-{damageAmount}.";
            GlobalReference.GlobalValues.Logger.Log(attacker, LogLevel.DEBUGVERBOSE, message);

            RunEnchantments(attacker, EventType.DamageDealtAfterDefense, new EventParamerters() { Attacker = attacker, Defender = defender, DamageAmount = damageAmount });

            #region damage messages
            //attacker?.EnqueueMessage(GlobalReference.GlobalValues.TagWrapper.WrapInTag($"You hit {defender.SentenceDescription} for {damageAmount} damage.", TagType.DamageDelt));
            if (attacker != null)
            {
                GlobalReference.GlobalValues.Notify.Mob(attacker, defender, attacker, new TranslationMessage("You hit {target} for {0} damage.".Replace("{0}", damageAmount.ToString()), TagType.DamageDelt));
            }
            if (defender != null)
            {
                GlobalReference.GlobalValues.Notify.Mob(attacker, defender, defender, new TranslationMessage("{performer} hit you for {0} damage.".Replace("{0}", damageAmount.ToString()), TagType.DamageReceived));
            }
            //defender?.EnqueueMessage(GlobalReference.GlobalValues.TagWrapper.WrapInTag($"{CapitializeFirstLetter(attacker?.SentenceDescription ?? "unknown")} hit you for {damageAmount} damage.", TagType.DamageReceived));

            message = $"{CapitializeFirstLetter(attacker?.SentenceDescription ?? "unknown")} attacked {defender.SentenceDescription} for {damageAmount} damage.";
            ITranslationMessage translationMessage = new TranslationMessage(message);
            GlobalReference.GlobalValues.Notify.Room(attacker, defender, attacker.Room, translationMessage, new List<IMobileObject>() { attacker, defender });
            #endregion damage messages
        }

        public void DamageReceivedBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            string message = $"DamageReceivedBeforeDefense: Attacker-{attacker?.SentenceDescription ?? "unknown"} Defender-{defender.SentenceDescription} DamageAmount-{damageAmount}.";
            GlobalReference.GlobalValues.Logger.Log(attacker, LogLevel.DEBUGVERBOSE, message);

            RunEnchantments(attacker, EventType.DamageReceivedBeforeDefense, new EventParamerters() { Attacker = attacker, Defender = defender, DamageAmount = damageAmount });
        }

        public void DamageReceivedAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            string message = $"DamageReceivedAfterDefense: Attacker-{attacker?.SentenceDescription ?? "unknown"} Defender-{defender.SentenceDescription} DamageAmount-{damageAmount}.";
            GlobalReference.GlobalValues.Logger.Log(attacker, LogLevel.DEBUGVERBOSE, message);

            RunEnchantments(attacker, EventType.DamageReceivedAfterDefense, new EventParamerters() { Attacker = attacker, Defender = defender, DamageAmount = damageAmount });
        }

        public string EnqueueMessage(IMobileObject performer, string message)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.ALL, message);
            return message;
        }
        public int ToDodge(IMobileObject performer, int rolledValue)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} attempted to dodge and rolled {rolledValue}.");
            return rolledValue;
        }

        public int ToHit(IMobileObject performer, int rolledValue)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} attempted to hit and rolled {rolledValue}.");
            return rolledValue;
        }
        #endregion Events

        #region Room Enter/Leave
        public void EnterRoom(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} entered room {performer.Room.ToString()}.");
            IRoom performerRoomTheyEntered = performer.Room;
            RunEnchantments(performer, EventType.EnterRoom, new EventParamerters() { Performer = performer });

            //now that all the enchantments have finished check once again to see if they are still in the same room
            //if so send the sound effects
            if (performer.Room == performerRoomTheyEntered)
            {
                if (performer.Room.SerializedSounds != null)
                {
                    GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage(performer.Room.SerializedSounds, TagType.Sound));
                }

                GlobalReference.GlobalValues.Map.SendMapPosition(performer);
            }
        }

        public void LeaveRoom(IMobileObject performer, Directions.Direction direction)
        {
            RunEnchantments(performer, EventType.LeaveRoom, new EventParamerters() { Performer = performer, Direction = direction });
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} left room {performer.Room.ToString()}.");
        }

        public void AttemptToFollow(Directions.Direction direction, IMobileObject performer, IMobileObject followedTarget)
        {
            RunEnchantments(performer, EventType.AttemptToFollow, new EventParamerters() { Direction = direction, Performer = performer, FollowedTarget = followedTarget });
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} attempted to follow {followedTarget.SentenceDescription} {direction.ToString()}.");
        }
        #endregion Room Enter/Leave

        #region Commands
        public void Cast(IMobileObject performer, string spellName)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} cast {spellName}.");
            RunEnchantments(performer, EventType.Cast, new EventParamerters() { Performer = performer, SpellName = spellName });
        }
        public void Perform(IMobileObject performer, string skillName)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} performed {skillName}.");
            RunEnchantments(performer, EventType.Perform, new EventParamerters() { Performer = performer, SkillName = skillName });
        }
        public void Drop(IMobileObject performer, IItem item)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} dropped {item.SentenceDescription}.");
            RunEnchantments(performer, EventType.Drop, new EventParamerters() { Performer = performer, Item = item });
        }
        public void Get(IMobileObject performer, IItem item, IContainer container = null)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} got {item.SentenceDescription}.");
            RunEnchantments(performer, EventType.Get, new EventParamerters() { Performer = performer, Item = item, Container = container });
        }
        public void Put(IMobileObject performer, IItem item, IContainer container)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} put {item.SentenceDescription}.");
            RunEnchantments(performer, EventType.Put, new EventParamerters() { Performer = performer, Item = item, Container = container });
        }

        public void Relax(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} relaxed.");
            RunEnchantments(performer, EventType.Relax, new EventParamerters() { Performer = performer });
        }
        public void Sit(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} sat.");
            RunEnchantments(performer, EventType.Sit, new EventParamerters() { Performer = performer });
        }
        public void Sleep(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} slept.");
            RunEnchantments(performer, EventType.Sleep, new EventParamerters() { Performer = performer });
        }
        public void Stand(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} stood.");
            RunEnchantments(performer, EventType.Stand, new EventParamerters() { Performer = performer });
        }
        public void Equip(IMobileObject performer, IItem item)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} equipped {item.SentenceDescription}.");
            RunEnchantments(performer, EventType.Equip, new EventParamerters() { Performer = performer, Item = item });
        }
        public void Unequip(IMobileObject performer, IItem item)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUG, $"{performer.SentenceDescription} unequipped {item.SentenceDescription}.");
            RunEnchantments(performer, EventType.Unequip, new EventParamerters() { Performer = performer, Item = item });
        }
        #endregion Commands

        #region Input/Output
        public void ProcessedCommand(IMobileObject performer, string command)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} processed command {command}.");
            RunEnchantments(performer, EventType.ProcessedCommand, new EventParamerters() { Performer = performer, Command = command });
        }

        public void ProcessedCommunication(IMobileObject performer, string communication)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} processed communication {communication}.");
            RunEnchantments(performer, EventType.ProcessedCommunication, new EventParamerters() { Performer = performer, Communication = communication });
        }

        public void ReturnedMessage(IMobileObject performer, string message)
        {
            GlobalReference.GlobalValues.Logger.Log(performer, LogLevel.DEBUGVERBOSE, $"{performer.SentenceDescription} returned message {message}.");
            RunEnchantments(performer, EventType.ReturnedMessage, new EventParamerters() { Performer = performer, Message = message });
        }
        #endregion Input/Output

        private static void RunEnchantments(IMobileObject performer, EventType eventType, EventParamerters paramerter)
        {
            if (eventType == EventType.DamageReceivedBeforeDefense
                || eventType == EventType.DamageReceivedAfterDefense)
            {
                foreach (IEnchantment enchantment in paramerter.Defender.Enchantments)
                {
                    RunEnchantment(enchantment, eventType, paramerter);
                }
            }
            else if (eventType == EventType.DamageDealtBeforeDefense
                    || eventType == EventType.DamageDealtAfterDefense)
            {
                if (performer != null)
                {
                    foreach (IEnchantment enchantment in performer.Enchantments)
                    {
                        RunEnchantment(enchantment, eventType, paramerter);
                    }
                }
            }
            else
            {
                IRoom performerRoom = performer.Room;
                if (performerRoom.Enchantments.Count > 0)
                {
                    foreach (IEnchantment enchantment in performerRoom.Enchantments)
                    {
                        RunEnchantment(enchantment, eventType, paramerter);
                    }
                }

                if (performerRoom.Traps.Count > 0)
                {
                    foreach (ITrap trap in performerRoom.Traps)
                    {
                        if (trap.Trigger == Trap.Target.TrapTrigger.All
                            || (performer is NonPlayerCharacter && trap.Trigger == Trap.Target.TrapTrigger.NPC)
                            || (performer is PlayerCharacter && trap.Trigger == Trap.Target.TrapTrigger.PC))
                        {
                            foreach (IEnchantment enchantment in trap.Enchantments)
                            {
                                RunEnchantment(enchantment, eventType, paramerter);
                            }
                        }
                    }
                }

                if (performerRoom.PlayerCharacters.Count > 0)
                {
                    foreach (IPlayerCharacter pc in performerRoom.PlayerCharacters)
                    {
                        if (pc.Enchantments.Count > 0)
                        {
                            if (pc == performer)
                            {
                                //verify the performer is still alive before they can trigger their enchantment
                                if (performer.Room == performerRoom)
                                {
                                    foreach (IEnchantment enchantment in performer.Enchantments)
                                    {
                                        RunEnchantment(enchantment, eventType, paramerter);
                                    }
                                }
                            }
                            else
                            {
                                //for only fire off when its the enchanted player turn, otherwise it will fire once for each player in the room
                                //keeping else statement  around in case we want to explain why we don't do this in case we want to try this again later
                            }
                        }
                    }
                }

                if (performerRoom.NonPlayerCharacters.Count > 0)
                {
                    foreach (INonPlayerCharacter npc in performerRoom.NonPlayerCharacters)
                    {
                        if (npc.Enchantments.Count > 0)
                        {
                            foreach (IEnchantment enchantment in npc.Enchantments)
                            {
                                RunEnchantment(enchantment, eventType, paramerter);
                            }
                        }
                    }
                }

                if (performerRoom.Items.Count > 0)
                {
                    foreach (IItem item in performerRoom.Items)
                    {
                        if (item.Enchantments.Count > 0)
                        {
                            foreach (IEnchantment enchantment in item.Enchantments)
                            {
                                RunEnchantment(enchantment, eventType, paramerter);
                            }
                        }
                    }
                }
            }
        }

        private static void RunEnchantment(IEnchantment enchantment, EventType eventType, EventParamerters paramerter)
        {
            switch (eventType)
            {
                case EventType.AttemptToFollow:
                    enchantment.AttemptToFollow(paramerter.Direction, paramerter.Performer, paramerter.FollowedTarget);
                    break;
                case EventType.Cast:
                    enchantment.Cast(paramerter.Performer, paramerter.SpellName);
                    break;
                case EventType.DamageDealtAfterDefense:
                    enchantment.DamageDealtAfterDefense(paramerter.Attacker, paramerter.Defender, paramerter.DamageAmount);
                    break;
                case EventType.DamageDealtBeforeDefense:
                    enchantment.DamageDealtBeforeDefense(paramerter.Attacker, paramerter.Defender, paramerter.DamageAmount);
                    break;
                case EventType.DamageReceivedAfterDefense:
                    enchantment.DamageReceivedAfterDefense(paramerter.Attacker, paramerter.Defender, paramerter.DamageAmount);
                    break;
                case EventType.DamageReceivedBeforeDefense:
                    enchantment.DamageReceivedBeforeDefense(paramerter.Attacker, paramerter.Defender, paramerter.DamageAmount);
                    break;
                case EventType.Drop:
                    enchantment.Drop(paramerter.Performer, paramerter.Item);
                    break;
                case EventType.EnterRoom:
                    enchantment.EnterRoom(paramerter.Performer);
                    break;
                case EventType.Equip:
                    enchantment.Equip(paramerter.Performer, paramerter.Item);
                    break;
                case EventType.Get:
                    enchantment.Get(paramerter.Performer, paramerter.Item, paramerter.Container);
                    break;
                case EventType.HeartbeatBigTick:
                    enchantment.HeartbeatBigTick(paramerter.Performer);
                    break;
                case EventType.LeaveRoom:
                    enchantment.LeaveRoom(paramerter.Performer, paramerter.Direction);
                    break;
                case EventType.OnDeath:
                    enchantment.OnDeath(paramerter.Performer);
                    break;
                case EventType.Perform:
                    enchantment.Perform(paramerter.Performer, paramerter.SkillName);
                    break;
                case EventType.ProcessedCommand:
                    enchantment.ProcessedCommand(paramerter.Performer, paramerter.Command);
                    break;
                case EventType.ProcessedCommunication:
                    enchantment.ProcessedCommunication(paramerter.Performer, paramerter.Communication);
                    break;
                case EventType.Put:
                    enchantment.Put(paramerter.Performer, paramerter.Item, paramerter.Container);
                    break;
                case EventType.Relax:
                    enchantment.Relax(paramerter.Performer);
                    break;
                case EventType.ReturnedMessage:
                    enchantment.ReturnedMessage(paramerter.Performer, paramerter.Message);
                    break;
                case EventType.Sit:
                    enchantment.Sit(paramerter.Performer);
                    break;
                case EventType.Sleep:
                    enchantment.Sleep(paramerter.Performer);
                    break;
                case EventType.Stand:
                    enchantment.Stand(paramerter.Performer);
                    break;
                case EventType.Unequip:
                    enchantment.Unequip(paramerter.Performer, paramerter.Item);
                    break;
            }
        }

        private static string CapitializeFirstLetter(string str)
        {
            if (str.Length == 0)
            {
                return str;
            }
            else
            {
                return str.Substring(0, 1).ToUpper() + str.Substring(1);
            }
        }

        public enum EventType
        {
            DamageDealtBeforeDefense,
            DamageDealtAfterDefense,
            DamageReceivedBeforeDefense,
            DamageReceivedAfterDefense,
            EnqueueMessage,
            HeartbeatBigTick,
            OnDeath,
            ToDodge,
            ToHit,
            AttemptToFollow,
            EnterRoom,
            LeaveRoom,
            Cast,
            Perform,
            Drop,
            Get,
            Put,
            Relax,
            Sit,
            Sleep,
            Stand,
            Equip,
            Unequip,
            ProcessedCommand,
            ProcessedCommunication,
            ReturnedMessage
        }

        [ExcludeFromCodeCoverage]
        private class EventParamerters
        {
            public Directions.Direction Direction { get; set; }
            public IMobileObject FollowedTarget { get; set; }
            public IMobileObject Performer { get; set; }
            public string SpellName { get; internal set; }
            public IMobileObject Attacker { get; internal set; }
            public IMobileObject Defender { get; internal set; }
            public int DamageAmount { get; internal set; }
            public IItem Item { get; internal set; }
            public IContainer Container { get; internal set; }
            public string Message { get; internal set; }
            public string SkillName { get; internal set; }
            public string Command { get; internal set; }
            public string Communication { get; internal set; }
            public int RolledValue { get; internal set; }
        }
    }
}
