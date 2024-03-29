﻿using Objects.Damage.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob;
using System.Collections.Generic;
using System.Linq;
using static Objects.Global.Stats.Stats;
using System.Collections.Concurrent;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using Objects.Command;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Engines.AdditionalCombat;

namespace Objects.Global.Engine.Engines
{
    public class Combat : ICombat
    {
        private uint _combatRound { get; set; } = 0;

        private ConcurrentDictionary<IMobileObject, CombatPair> Combatants { get; } = new ConcurrentDictionary<IMobileObject, CombatPair>();

        public void ProcessCombatRound()
        {
            //set the combat round so we can determine if slow weapons need to hit
            _combatRound++;

            List<IMobileObject> combatOrder = Combatants.Keys.OrderByDescending(o => o.CalculateAttackOrderRoll()).ToList();

            if (combatOrder.Count > 0)
            {
                foreach (IMobileObject mob in combatOrder)
                {
                    ProcessAttack(mob);
                }
            }
        }

        public IResult AddCombatPair(IMobileObject attacker, IMobileObject defender)
        {
            if (Combatants.TryGetValue(attacker, out CombatPair? pair))
            {
                return new Result(string.Format("You are already attacking {0}.", pair.Defender.KeyWords.FirstOrDefault()), true);
            }
            else
            {
                pair = new CombatPair();
                pair.Attacker = attacker;
                pair.Defender = defender;
                Combatants.TryAdd(attacker, pair);

                //add the defender attacking the attacker
                AddCombatPairInternal(defender, attacker);

                return new Result(string.Format("You begin to attack {0}.", defender.KeyWords.FirstOrDefault()), false);
            }
        }

        public bool AreFighting(IMobileObject mob, IMobileObject mob2)
        {
            CombatPair? pair;
            if (Combatants.TryGetValue(mob, out pair))
            {
                if (pair.Defender == mob2)
                {
                    return true;
                }
            }

            if (Combatants.TryGetValue(mob2, out pair))
            {
                if (pair.Defender == mob)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsInCombat(IMobileObject mob)
        {
            return Combatants.ContainsKey(mob);
        }

        public IMobileObject? Opponet(IMobileObject mobileObject)
        {
            if (Combatants.TryGetValue(mobileObject, out CombatPair? pair))
            {
                return pair.Defender;
            }
            else
            {
                return null;
            }
        }

        private void ProcessAttack(IMobileObject mob)
        {
            CombatPair pair = Combatants[mob];
            bool invalidComabatPair = RemoveInvalidCombatant(mob, pair);

            if (!invalidComabatPair)
            {
                ProcessWeapons(pair.Attacker, pair.Defender);
                AddCombatPairInternal(pair.Defender, pair.Attacker);
            }
        }

        private bool RemoveInvalidCombatant(IMobileObject mob, CombatPair pair)
        {
            if ((pair.Attacker.Health <= 0 || pair.Defender.Health <= 0)
                || (pair.Attacker.Room != pair.Defender.Room)
                || pair.Attacker.Room.Attributes.Contains(Room.Room.RoomAttribute.Peaceful))
            {
                Combatants.TryRemove(mob, out CombatPair? tempCombatPair);
                return true;
            }

            return false;
        }

        private void ProcessWeapons(IMobileObject attacker, IMobileObject defender)
        {
            foreach (IWeapon weapon in attacker.EquipedWeapon)
            {
                if (weapon.WeaponId == 0)
                {
                    if (weapon.Id == 0 && weapon.ZoneId == 0)
                    {
                        //someone is using bare hands so just set a random value that shouldn't be used
                        weapon.WeaponId = GlobalReference.GlobalValues.Random.Next(int.MinValue, 0);
                    }
                    else
                    {
                        weapon.WeaponId = GlobalReference.GlobalValues.WeaponId.Id;
                    }
                }

                if (_combatRound % weapon.Speed == 0
                    && DetermineIfHit(attacker, defender, weapon.AttackerStat, weapon.DeffenderStat, weapon.WeaponId))
                {
                    ProcessWeaponDamage(attacker, defender, weapon);
                }
            }
        }

        private void ProcessWeaponDamage(IMobileObject attacker, IMobileObject defender, IWeapon weapon)
        {
            foreach (IDamage damage in weapon.DamageList)
            {
                int amountOfDamage = DealDamage(attacker, defender, damage);
            }
        }

        /// <summary>
        /// Version that is used for internal that will not try to add the opposite attack order
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        private void AddCombatPairInternal(IMobileObject attacker, IMobileObject defender)
        {
            if (!Combatants.Keys.Contains(attacker))
            {
                CombatPair pair = new CombatPair();
                pair.Attacker = attacker;
                pair.Defender = defender;
                Combatants.TryAdd(attacker, pair);
            }
        }

        private bool DetermineIfHit(IMobileObject attacker, IMobileObject defender, Stat attackerStat, Stat defenderStat, long weaponId)
        {
            int hitRoll = attacker.CalculateToHitRoll(attackerStat);
            int defenseRoll = defender.CalculateToDodgeRoll(defenderStat, weaponId, _combatRound);

            if (hitRoll >= defenseRoll)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int DealDamage(IMobileObject attacker, IMobileObject defender, IDamage damage)
        {
            int totalDamage = attacker.CalculateDamage(damage);

            int damageReceived = defender.TakeCombatDamage(totalDamage, damage, attacker, _combatRound);

            return damageReceived;
        }
    }
}
