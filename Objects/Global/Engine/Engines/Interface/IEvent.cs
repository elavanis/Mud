﻿using Objects.Global.Direction;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;

namespace Objects.Global.Engine.Engines.Interface
{
    public interface IEvent
    {
        #region Events
        #region Damage
        void DamageBeforeDefense(IMobileObject attacker, IMobileObject defender, int damageAmount, string attackerDescription = null);
        void DamageAfterDefense(IMobileObject attacker, IMobileObject defender, int damageAmount, string attackerDescription = null);
        #endregion Damage
        string EnqueueMessage(IMobileObject mob, string message);
        void HeartbeatBigTick(IBaseObject performer);
        void OnDeath(IMobileObject performer);
        int ToDodge(IMobileObject performer, int rolledValue);
        int ToHit(IMobileObject performer, int rolledValue);
        #endregion Events

        #region Room Enter/Leave
        void AttemptToFollow(Directions.Direction direction, IMobileObject performer, IMobileObject followedTarget);
        void EnterRoom(IMobileObject mob);
        void LeaveRoom(IMobileObject mob, Directions.Direction direction);
        #endregion Room Enter/Leave

        #region Commands
        void Cast(IMobileObject performer, string spellName);
        void Perform(IMobileObject performer, string skillName);
        void Drop(IMobileObject performer, IItem item);
        void Get(IMobileObject performer, IItem item, IContainer container = null);
        void Open(IMobileObject performer, IItem item);
        void Close(IMobileObject performer, IItem item);

        void Put(IMobileObject performer, IItem item, IContainer container);
        void Relax(IMobileObject performer);
        void Sit(IMobileObject performer);
        void Sleep(IMobileObject performer);
        void Stand(IMobileObject performer);
        void Equip(IMobileObject performer, IItem item);
        void Unequip(IMobileObject performer, IItem item);
        #endregion Commands

        #region Input/Output
        void ProcessedCommand(IMobileObject performer, string command);
        void ProcessedCommunication(IMobileObject performer, string communication);
        void ReturnedMessage(IMobileObject performer, string message);
        #endregion Input/Output
    }
}
