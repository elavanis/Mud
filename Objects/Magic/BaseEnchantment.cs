using Objects.Global;
using Objects.Interface;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Global.Direction;
using System;
using Objects.Item.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Effect;
using System.Collections.Generic;
using Objects.Effect.Interface;

namespace Objects.Magic
{
    public abstract class BaseEnchantment : IEnchantment, IEvent
    {
        [ExcludeFromCodeCoverage]
        public IEffect Effect { get; set; }
        [ExcludeFromCodeCoverage]
        public double ActivationPercent { get; set; }
        [ExcludeFromCodeCoverage]
        public TargetOption Target { get; set; }
        [ExcludeFromCodeCoverage]
        public IEffectParameter Parameter { get; set; } = new EffectParameter();
        [ExcludeFromCodeCoverage]
        public DateTime EnchantmentEndingDateTime { get; set; }
        [ExcludeFromCodeCoverage]
        public string RoomNotification { get; set; }
        [ExcludeFromCodeCoverage]
        public string TargetNotification { get; set; }

        public enum TargetOption
        {
            Performer,
            Attacker,
            Defender
        }

        #region Enchantment Triggers
        protected bool ProcessEnchantment()
        {
            return GlobalReference.GlobalValues.Random.PercentDiceRoll(ActivationPercent);
        }

        [ExcludeFromCodeCoverage]
        public virtual string EnqueueMessage(IMobileObject mob, string message)
        {
            //do nothing unless overrode
            return message;
        }

        [ExcludeFromCodeCoverage]
        public virtual void EnterRoom(IMobileObject mob)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void LeaveRoom(IMobileObject mob, Directions.Direction direction)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void DamageDealtBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void DamageDealtAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void DamageReceivedBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void DamageReceivedAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void HeartbeatBigTick(IBaseObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void OnDeath(IMobileObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual int ToDodge(IMobileObject performer, int rolledValue)
        {
            //do nothing unless overrode
            return rolledValue;
        }

        [ExcludeFromCodeCoverage]
        public virtual int ToHit(IMobileObject performer, int rolledValue)
        {
            //do nothing unless overrode
            return rolledValue;
        }

        [ExcludeFromCodeCoverage]
        public virtual void AttemptToFollow(Directions.Direction direction, IMobileObject performer, IMobileObject mob)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Cast(IMobileObject performer, string spellName)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Perform(IMobileObject performer, string skillName)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Drop(IMobileObject performer, IItem item)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Equip(IMobileObject performer, IItem item)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Get(IMobileObject performer, IItem item)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Relax(IMobileObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Sit(IMobileObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Sleep(IMobileObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Stand(IMobileObject performer)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void Unequip(IMobileObject performer, IItem item)
        {
            //do nothing unless overrode
        }

        [ExcludeFromCodeCoverage]
        public virtual void ProcessedCommand(IMobileObject performer, string command)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public virtual void ProcessedCommunication(IMobileObject performer, string communication)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public virtual void ReturnedMessage(IMobileObject performer, string message)
        {
            throw new NotImplementedException();
        }

        #endregion Enchantment Triggers
    }
}
