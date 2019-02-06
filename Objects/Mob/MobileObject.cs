using Objects.Damage.Interface;
using Objects.Die;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Guild;
using Objects.Guild.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Magic;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Race.Interface;
using Objects.Race.Races;
using Objects.Room.Interface;
using Objects.Skill.Interface;
using Shared.TagWrapper;
using Shared.TelnetItems;
using Shared.TelnetItems.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using static Objects.Damage.Damage;
using static Shared.TagWrapper.TagWrapper;
using Objects.LoadPercentage.Interface;
using Objects.Damage;
using static Objects.Global.Language.Translator;
using Objects.Global.Language;
using Objects.Global.Language.Interface;
using Objects.Language.Interface;
using static Objects.Global.Logging.LogSettings;

namespace Objects.Mob
{
    public abstract class MobileObject : BaseObject, IContainer, IMobileObject
    {
        #region Properties
        [ExcludeFromCodeCoverage]
        public bool IsAlive { get; set; } = true;

        [ExcludeFromCodeCoverage]
        public IRace Race { get; set; } = new Human();

        [ExcludeFromCodeCoverage]
        public virtual int Level { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public int LevelPoints { get; set; }

        [ExcludeFromCodeCoverage]
        public HashSet<Guild.Guild.Guilds> Guild { get; set; } = new HashSet<Guild.Guild.Guilds>();

        [ExcludeFromCodeCoverage]
        public int GuildPoints { get; set; }

        [ExcludeFromCodeCoverage]
        public IRoom Room { get; set; }

        [ExcludeFromCodeCoverage]
        public IBaseObjectId RoomId { get; set; }

        [ExcludeFromCodeCoverage]
        public IBaseObjectId RecallPoint { get; set; }

        [ExcludeFromCodeCoverage]
        public ulong LastProccessedTick { get; set; } = 0;

        [ExcludeFromCodeCoverage]
        public CharacterPosition Position { get; set; } = CharacterPosition.Stand;

        [ExcludeFromCodeCoverage]
        public ulong Money { get; set; }

        [ExcludeFromCodeCoverage]
        public string CorpseLongDescription { get; set; }

        [ExcludeFromCodeCoverage]
        public List<IItem> Items { get; } = new List<IItem>();

        [ExcludeFromCodeCoverage]
        public List<ILoadPercentage> LoadableItems { get; } = new List<ILoadPercentage>();

        [ExcludeFromCodeCoverage]
        public bool God { get; set; }

        public bool IsInCombat
        {
            get
            {
                return GlobalReference.GlobalValues.Engine.Combat.IsInCombat(this);
            }
        }

        public IMobileObject Opponent
        {
            get
            {
                return GlobalReference.GlobalValues.Engine.Combat.Opponet(this);
            }
        }


        public bool AreFighting(IMobileObject mob)
        {
            return GlobalReference.GlobalValues.Engine.Combat.AreFighting(this, mob);
        }

        private Dictionary<string, ISpell> _spellBook = null;
        public Dictionary<string, ISpell> SpellBook
        {
            get
            {
                if (_spellBook == null)
                {
                    _spellBook = new Dictionary<string, ISpell>();
                }
                return _spellBook;
            }
        }

        private Dictionary<string, ISkill> _knownSkills = null;
        public Dictionary<string, ISkill> KnownSkills
        {
            get
            {
                if (_knownSkills == null)
                {
                    _knownSkills = new Dictionary<string, ISkill>();
                }
                return _knownSkills;
            }
        }



        private List<MobileAttribute> _mobileAttributes = null;
        private List<MobileAttribute> AttributesMobileObject
        {
            get
            {
                if (_mobileAttributes == null)
                {
                    _mobileAttributes = new List<MobileAttribute>();
                }
                return _mobileAttributes;
            }
        }

        public void AddAttribute(MobileAttribute attribute)
        {
            AttributesMobileObject.Add(attribute);
        }

        public void RemoveAttribute(MobileAttribute attribute)
        {
            AttributesMobileObject.Remove(attribute);
        }

        public IEnumerable<MobileAttribute> AttributesCurrent
        {
            get
            {
                List<MobileAttribute> newList = new List<MobileAttribute>(AttributesMobileObject);
                newList.AddRange(Race.RaceAttributes);
                newList.AddRange(EquipedEquipment.SelectMany(e => e.AttributesForMobileObjectsWhenEquiped));
                return newList;
            }
        }

        private IMobileObject followTarget;
        public IMobileObject FollowTarget
        {
            get
            {
                if (followTarget == null)
                {
                    return null;
                }
                else
                {
                    if (followTarget.IsAlive)
                    {
                        return followTarget;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            set
            {
                followTarget = value;
            }
        }

        #region Combat Properties
        private uint CombatRound { get; set; }
        private int DamageMultiplier { get; set; } = 1;
        #endregion Combat Properties


        #region Stats
        [ExcludeFromCodeCoverage]
        public int StrengthStat { get; set; }
        [ExcludeFromCodeCoverage]
        public int DexterityStat { get; set; }
        [ExcludeFromCodeCoverage]
        public int ConstitutionStat { get; set; }
        [ExcludeFromCodeCoverage]
        public int IntelligenceStat { get; set; }
        [ExcludeFromCodeCoverage]
        public int WisdomStat { get; set; }
        [ExcludeFromCodeCoverage]
        public int CharismaStat { get; set; }

        public virtual int StrengthEffective
        {
            get
            {
                return StrengthStat + EquipmentModifer(EquipmentModifier.Strength);
            }
        }
        public virtual int DexterityEffective
        {
            get
            {
                return DexterityStat + EquipmentModifer(EquipmentModifier.Dexterity);
            }
        }
        public virtual int ConstitutionEffective
        {
            get
            {
                return ConstitutionStat + EquipmentModifer(EquipmentModifier.Constitution);
            }
        }
        public virtual int IntelligenceEffective
        {
            get
            {
                return IntelligenceStat + EquipmentModifer(EquipmentModifier.Intelligence);
            }
        }
        public virtual int WisdomEffective
        {
            get
            {
                return WisdomStat + EquipmentModifer(EquipmentModifier.Wisdom);
            }
        }
        public virtual int CharismaEffective
        {
            get
            {
                return CharismaStat + EquipmentModifer(EquipmentModifier.Charisma);
            }
        }
        #endregion Stats

        #region Health/Mana/Stamina
        [ExcludeFromCodeCoverage]
        public int Health { get; set; }

        public string HealthDescription
        {
            get
            {
                int healthPercent = Health * 100 / Math.Max(1, MaxHealth); //so we don't have division by 0 errors if max health is not set for some reason

                if (healthPercent >= 100)
                {
                    return $"{SentenceDescription} is in perfect health.";
                }
                else if (healthPercent >= 80)
                {
                    return $"{SentenceDescription} has some light scratches on them but nothing bad.";
                }
                else if (healthPercent >= 60)
                {
                    return $"{SentenceDescription} has some minor cuts with traces of blood.";
                }
                else if (healthPercent >= 40)
                {
                    return $"{SentenceDescription} has deep lacerations that will leave scars.";
                }
                else if (healthPercent >= 20)
                {
                    return $"{SentenceDescription} has been badly beaten.  They have bruises on bruises that are covered in blood from their many open wounds.";
                }
                else if (healthPercent >= 0)
                {
                    return $"{SentenceDescription} has begun to grow pale from loss of blood.  They may soon be riding on Charon's boat to the underworld.";
                }

                return ExamineDescription; //should never get this

            }
        }


        private int _maxHealth = -1;
        public int MaxHealth
        {
            get
            {
                if (_maxHealth == -1)
                {
                    _maxHealth = ConstitutionEffective * 10 + EquipmentModifer(EquipmentModifier.MaxHealth);
                }
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        [ExcludeFromCodeCoverage]
        public int Mana { get; set; }

        private int _maxMana = -1;
        public int MaxMana
        {
            get
            {
                if (_maxMana == -1)
                {
                    _maxMana = IntelligenceEffective * 10 + EquipmentModifer(EquipmentModifier.MaxMana);
                }
                return _maxMana;
            }
            set
            {
                _maxMana = value;
            }
        }

        [ExcludeFromCodeCoverage]
        public int Stamina { get; set; }

        private int _maxStamina = -1;
        public int MaxStamina
        {
            get
            {
                if (_maxStamina == -1)
                {
                    _maxStamina = ConstitutionEffective * 10 + EquipmentModifer(EquipmentModifier.MaxStamina);
                }
                return _maxStamina;
            }
            set
            {
                _maxStamina = value;
            }
        }

        public void ResetMaxStatValues()
        {
            MaxHealth = -1;
            MaxMana = -1;
            MaxStamina = -1;
        }
        #endregion Health/Mana/Stamina

        #region Equipment
        private List<IEquipment> _equipment = null;
        public IEnumerable<IEquipment> EquipedEquipment
        {
            get
            {
                if (_equipment == null)
                {
                    _equipment = new List<IEquipment>();
                }
                return _equipment;
            }
        }

        public void AddEquipment(IEquipment equipment)
        {
            if (_equipment == null)
            {
                _equipment = new List<IEquipment>();
            }
            _equipment.Add(equipment);

            ResetMaxStatValues();
        }

        public void RemoveEquipment(IEquipment equipment)
        {
            if (_equipment == null)
            {
                return;
            }
            _equipment.Remove(equipment);

            ResetMaxStatValues();
        }

        public virtual IEnumerable<IWeapon> EquipedWeapon
        {
            get
            {
                List<IWeapon> weapons = new List<IWeapon>();
                foreach (IItem item in EquipedEquipment)
                {
                    IWeapon weapon = item as IWeapon;
                    if (weapon != null)
                    {
                        weapons.Add(weapon);
                    }
                }

                if (weapons.Count == 0)
                {
                    IWeapon defaultWeapon = new Weapon();
                    defaultWeapon.AttackerStat = Stats.Stat.Dexterity;
                    defaultWeapon.DeffenderStat = Stats.Stat.Dexterity;
                    int strength = Math.Max(1, StrengthEffective);
                    int sides = strength;
                    defaultWeapon.DamageList.Add(new Damage.Damage(new Dice(1, sides)) { Type = DamageType.Bludgeon });
                    defaultWeapon.Speed = 1;
                    defaultWeapon.KeyWords.Add("BareHands");
                    weapons.Add(defaultWeapon);
                }

                return weapons;
            }
        }

        public virtual IEnumerable<IArmor> EquipedArmor
        {
            get
            {
                List<IArmor> armors = new List<IArmor>();
                foreach (IItem item in EquipedEquipment)
                {
                    IArmor armor = item as IArmor;
                    if (armor != null)
                    {
                        armors.Add(armor);
                    }
                }
                return armors;
            }
        }

        protected int EquipmentModifer(EquipmentModifier modifer)
        {
            int modifiedValue = 0;
            foreach (IEquipment item in EquipedEquipment)
            {
                switch (modifer)
                {
                    case EquipmentModifier.Charisma:
                        modifiedValue += item.Charisma;
                        break;
                    case EquipmentModifier.Constitution:
                        modifiedValue += item.Constitution;
                        break;
                    case EquipmentModifier.Dexterity:
                        modifiedValue += item.Dexterity;
                        break;
                    case EquipmentModifier.Intelligence:
                        modifiedValue += item.Intelligence;
                        break;
                    case EquipmentModifier.MaxHealth:
                        modifiedValue += item.MaxHealth;
                        break;
                    case EquipmentModifier.MaxMana:
                        modifiedValue += item.MaxMana;
                        break;
                    case EquipmentModifier.MaxStamina:
                        modifiedValue += item.MaxStamina;
                        break;
                    case EquipmentModifier.Strength:
                        modifiedValue += item.Strength;
                        break;
                    case EquipmentModifier.Wisdom:
                        modifiedValue += item.Wisdom;
                        break;
                }
            }

            return modifiedValue;
        }

        protected enum EquipmentModifier
        {
            Strength,
            Dexterity,
            Constitution,
            Intelligence,
            Wisdom,
            Charisma,
            MaxHealth,
            MaxMana,
            MaxStamina
        }
        #endregion Equipment

        #endregion Properties

        #region Methods
        #region Combat/Damage

        public virtual int CalculateToHitRoll(Stats.Stat stat)
        {
            return GlobalReference.GlobalValues.Random.Next(GetStatEffective(stat));
        }

        public virtual int CalculateToDodgeRoll(Stats.Stat stat)
        {
            return GlobalReference.GlobalValues.Random.Next(GetStatEffective(stat));
        }

        public virtual int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker)
        {
            return TakeDamage(totalDamage, damage, attacker, 1);
        }

        public virtual int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound)
        {
            if (CombatRound == combatRound)
            {
                DamageMultiplier *= 2;
            }
            else
            {
                CombatRound = combatRound;
                DamageMultiplier = 1;
            }

            return TakeDamage(totalDamage, damage, attacker, DamageMultiplier);
        }

        private int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker, int damageMultiplier)
        {
            GlobalReference.GlobalValues.Engine.Event.DamageReceivedBeforeDefense(attacker, this, totalDamage);

            int absoredDamage = 0;
            int stoppedDamage = 0;
            int mobReceivedDamage = 0;
            bool shieldBlocked = false;

            int healthBeforeDamage = Health;

            foreach (IArmor armor in EquipedArmor)
            {
                IShield shield = armor as IShield;
                if (shield != null)
                {
                    int shieldNegateRoll = GlobalReference.GlobalValues.Random.Next(101);
                    if (shieldNegateRoll <= shield.NegateDamagePercent)
                    {
                        shieldBlocked = true;
                    }
                }

                int damageBlocked = CalculateDamageBlocked(armor);
                decimal damageTypeModifer = armor.GetTypeModifier(damage.Type);

                if (damageTypeModifer == decimal.MaxValue)
                {
                    //we have an item that makes the user immune to the damage type
                    //set the stopped damage = to the total damage
                    stoppedDamage = totalDamage;
                }
                else if (damageTypeModifer >= 0)
                {
                    //values 0-1 reduce the effectiveness of the armor
                    //values > 1 increase the effectiveness of the armor
                    //ie .5 reduces the damage blocked by half
                    //ie 1.5 increase the damage blocked by 50%
                    stoppedDamage += (int)(damageBlocked * damageTypeModifer);
                }
                //armor absorbs this damage into life
                else
                {
                    //low negative number absorb a little life
                    //high negative numbers absorb more life
                    //ie  -.5 absorbs half the blocked damage back as life
                    //ie -1.5 absorbs 1.5 times the damage blocked back as life
                    stoppedDamage += damageBlocked;
                    absoredDamage += (int)(damageBlocked * damageTypeModifer);
                }
            }

            stoppedDamage += AddDefenseStatBonus(damage);

            stoppedDamage /= damageMultiplier;

            //damage can not be negative
            int receivedDamage = Math.Max(0, totalDamage - stoppedDamage);


            if (shieldBlocked != true
                && receivedDamage > 0)
            {
                //this is damage that went through to the mob.  
                //Multiply it by their race damage modifier and take the modified damage.
                mobReceivedDamage = (int)(receivedDamage * GetTypeModifier(damage.Type));
                Health -= mobReceivedDamage;
            }

            //absoredDamage is negative because of the previous multipliers being negative
            //so we want to subtract the damage to make it positive
            Health -= absoredDamage;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            else if (Health <= 0
                        && healthBeforeDamage > 0) //don't let a mob die two or more times.
            {
                KillMobAndRewardXP(attacker);
            }

            //return received damage minus any absorbed damage (it is negative so we add it)
            int netDamage = mobReceivedDamage + absoredDamage;

            GlobalReference.GlobalValues.Engine.Event.DamageReceivedAfterDefense(attacker, this, netDamage);

            return netDamage;
        }

        public void KillMobAndRewardXP(IMobileObject attacker)
        {
            Die();
            INonPlayerCharacter npc = this as INonPlayerCharacter;
            IPlayerCharacter pc = attacker as IPlayerCharacter;
            if (npc != null && attacker != null)
            {
                pc.Experience += npc.EXP;
            }
        }

        private decimal GetTypeModifier(DamageType damageType)
        {
            switch (damageType)
            {
                case Damage.Damage.DamageType.Acid:
                    return Race.Acid;
                case Damage.Damage.DamageType.Bludgeon:
                    return Race.Bludgeon;
                case Damage.Damage.DamageType.Cold:
                    return Race.Cold;
                case Damage.Damage.DamageType.Fire:
                    return Race.Fire;
                case Damage.Damage.DamageType.Force:
                    return Race.Force;
                case Damage.Damage.DamageType.Lightning:
                    return Race.Lightning;
                case Damage.Damage.DamageType.Necrotic:
                    return Race.Necrotic;
                case Damage.Damage.DamageType.Pierce:
                    return Race.Pierce;
                case Damage.Damage.DamageType.Poison:
                    return Race.Poison;
                case Damage.Damage.DamageType.Psychic:
                    return Race.Psychic;
                case Damage.Damage.DamageType.Radiant:
                    return Race.Radiant;
                case Damage.Damage.DamageType.Slash:
                    return Race.Slash;
                case Damage.Damage.DamageType.Thunder:
                    return Race.Thunder;
                default:
                    throw new System.Exception("Unknown damage type " + damageType.ToString());
            }
        }

        private int AddDefenseStatBonus(IDamage damage)
        {
            if (damage.BonusDefenseStat != null)
            {
                return GlobalReference.GlobalValues.Random.Next(GetStatEffective(damage.BonusDefenseStat)) + 1;
            }

            return 0;
        }

        public int CalculateDamageBlocked(IArmor armor)
        {
            return armor.Dice.RollDice();
        }

        public virtual int CalculateDamage(IDamage damage)
        {
            int totalDamage = damage.Dice.RollDice();
            totalDamage += AddDamageStatBonus(damage);
            return totalDamage;
        }

        private int AddDamageStatBonus(IDamage damage)
        {
            if (damage.BonusDamageStat != null)
            {
                return GlobalReference.GlobalValues.Random.Next(GetStatEffective(damage.BonusDamageStat)) + 1;
            }

            return 0;
        }

        public int GetStatEffective(Stats.Stat? stat)
        {
            int value = 0;
            switch (stat)
            {
                case Stats.Stat.Strength:
                    value = StrengthEffective;
                    break;
                case Stats.Stat.Dexterity:
                    value = DexterityEffective;
                    break;
                case Stats.Stat.Constitution:
                    value = ConstitutionEffective;
                    break;
                case Stats.Stat.Intelligence:
                    value = IntelligenceEffective;
                    break;

                case Stats.Stat.Wisdom:
                    value = WisdomEffective;
                    break;
                case Stats.Stat.Charisma:
                    value = CharismaEffective;
                    break;
            }

            return value;
        }

        public virtual int CalculateAttackOrderRoll()
        {
            return GlobalReference.GlobalValues.Random.Next(GetStatEffective(Stats.Stat.Dexterity));
        }

        /// <summary>
        /// Causes the mobile to die and returns the corpse
        /// </summary>
        /// <returns></returns>
        public virtual ICorpse Die()
        {
            GlobalReference.GlobalValues.Engine.Event.OnDeath(this);
            IsAlive = false;

            if (PossingMob != null)
            {
                PossingMob.PossedMob = null;
                PossingMob.EnqueueCommand("Look");
                PossingMob = null;
            }

            foreach (IEnchantment enchantment in Enchantments)
            {
                enchantment.EnchantmentEndingDateTime = new DateTime();  //set the end date to the past so its not fired and wil be cleaned up 
            }

            Corpse corpse = new Corpse();
            corpse.TimeOfDeath = DateTime.UtcNow;
            corpse.ShortDescription = "A corpse lies here.";
            corpse.LookDescription = CorpseLongDescription ?? "This corpse once was living but no life exists here now.";
            corpse.ExamineDescription = CorpseLongDescription ?? "This corpse once was living but no life exists here now.";
            corpse.KeyWords.Add("Corpse");
            corpse.SentenceDescription = "corpse";

            corpse.Items.AddRange(EquipedEquipment);
            corpse.Items.AddRange(Items);
            _equipment.Clear();
            Items.Clear();
            corpse.Items.Add(new Money(Money));
            Money = 0;

            return corpse;
        }
        #endregion Combat/Damage

        public void LevelMobileObject()
        {
            int strengthBegin = StrengthStat;
            int dexterityBegin = DexterityStat;
            int constitutionBegin = ConstitutionStat;
            int intelligenceBegin = IntelligenceStat;
            int wisdomBegin = WisdomStat;
            int charismaBegin = CharismaStat;

            StrengthStat = (int)Math.Round(StrengthStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            DexterityStat = (int)Math.Round(DexterityStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            ConstitutionStat = (int)Math.Round(ConstitutionStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            IntelligenceStat = (int)Math.Round(IntelligenceStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            WisdomStat = (int)Math.Round(WisdomStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);
            CharismaStat = (int)Math.Round(CharismaStat * GlobalReference.GlobalValues.Settings.Multiplier, 0, MidpointRounding.AwayFromZero);

            int difference = (StrengthStat - strengthBegin)
                + (DexterityStat - dexterityBegin)
                + (ConstitutionStat - constitutionBegin)
                + (IntelligenceStat - intelligenceBegin)
                + (WisdomStat - wisdomBegin)
                + (CharismaStat - charismaBegin);

            int average = (int)Math.Round(difference / 6.0, 0, MidpointRounding.AwayFromZero);

            LevelPoints += average;
            Level++;
            ResetMaxStatValues();
        }


        #endregion Methods

        /// <summary>
        /// attributes that are part are temporary in nature
        /// </summary>
        public enum MobileAttribute
        {
            Fly,
            Hidden,
            Infravision,
            Invisibile,
            NoFlee,
            SeeInvisible,
            NoDisarm
        }

        public enum CharacterPosition
        {
            Stand,
            Sit,
            Relax,
            Sleep,
            Mounted
        }

        #region Message/Commands
        protected ConcurrentQueue<string> _messageQueue { get; } = new ConcurrentQueue<string>();

        public virtual void EnqueueMessage(string message)
        {
            //if a message would be blank it is now marked null
            //skipping null messages will now make it no longer enqueue multiple status updates
            if (message != null)
            {
                InternalEnqueueMessage(GlobalReference.GlobalValues.Engine.Event.EnqueueMessage(this, message));

                //do not add any extra status update for sound messages
                if (!message.StartsWith("<Sound>"))
                {
                    //only send status for the posses mob
                    if (PossedMob == null)
                    {
                        InternalEnqueueMessage(GlobalReference.GlobalValues.Engine.Event.EnqueueMessage(this, Status()));
                    }

                    if (LevelPoints > 0)
                    {
                        string levelPointsMessage = GlobalReference.GlobalValues.TagWrapper.WrapInTag(string.Format("You have {0} level points to spend.", LevelPoints));
                        InternalEnqueueMessage(GlobalReference.GlobalValues.Engine.Event.EnqueueMessage(this, levelPointsMessage));
                    }

                    while (_messageQueue.Count >= 100)
                    {
                        string temp;
                        _messageQueue.TryDequeue(out temp);
                    }
                }
            }
        }

        private void InternalEnqueueMessage(string message)
        {
            if (PossingMob == null)
            {
                _messageQueue.Enqueue(message);
            }
            else
            {
                _messageQueue.Enqueue(message);     //allow the possessed mob to see what they are doing
                PossingMob.EnqueueMessage(message);
            }
        }

        private string Status()
        {
            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendLine();

            string message = string.Format("{0}/{1} ", Health, MaxHealth);
            strBldr.Append(GlobalReference.GlobalValues.TagWrapper.WrapInTag(message, TagType.Health));

            message = string.Format("{0}/{1} ", Mana, MaxMana);
            strBldr.Append(GlobalReference.GlobalValues.TagWrapper.WrapInTag(message, TagType.Mana));

            message = string.Format("{0}/{1}", Stamina, MaxStamina);
            strBldr.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(message, TagType.Stamina));

            return strBldr.ToString();
        }

        public string DequeueMessage()
        {
            string message;
            _messageQueue.TryDequeue(out message);
            return message;
        }

        protected ConcurrentQueue<string> _communicationQueue { get; } = new ConcurrentQueue<string>();
        protected ConcurrentQueue<string> _commandQueue { get; } = new ConcurrentQueue<string>();
        public HashSet<Languages> KnownLanguages { get; set; } = new HashSet<Languages>();

        public int CommmandQueueCount
        {
            get
            {
                return _commandQueue.Count;
            }
        }

        public IMobileObject PossingMob { get; set; }
        public IMobileObject PossedMob { get; set; }


        public void EnqueueCommand(string message)
        {
            string upperMessage = message.ToUpper();

            if (upperMessage.StartsWith("REQUESTASSET"))
            {
                RequestAsset(message);
            }
            else if (upperMessage.StartsWith("VALIDATEASSET"))
            {
                string hashedValue = GlobalReference.GlobalValues.ValidateAsset.GetCheckSum(message);
                _messageQueue.Enqueue(GlobalReference.GlobalValues.TagWrapper.WrapInTag(message + "|" + hashedValue, TagType.FileValidation));
            }
            else if (PossedMob != null
                        && !upperMessage.StartsWith("POSSESS")) //send everything other than possess commands to the possessed mob
            {
                PossedMob.EnqueueCommand(message);
            }
            else if (upperMessage.StartsWith("SAY")
                || upperMessage.StartsWith("SHOUT")
                || upperMessage.StartsWith("TELL")
                || upperMessage.StartsWith("EMOTE")
                )
            {
                _communicationQueue.Enqueue(message);
            }
            else
            {
                _commandQueue.Enqueue(message);
            }
        }

        private void RequestAsset(string message)
        {
            string[] splitMessage = message.Split('|');

            switch (splitMessage[1].ToUpper())
            {
                case "SOUND":
                case "MAP":
                    string fileLocation = Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, splitMessage[2]);
                    try
                    {
                        if (GlobalReference.GlobalValues.FileIO.Exists(fileLocation))
                        {
                            IData data = new Data(Data.DataType.File, fileLocation, GlobalReference.GlobalValues.FileIO);
                            data.AssetName = splitMessage[2];
                            string serializedData = GlobalReference.GlobalValues.Serialization.Serialize(data);
                            EnqueueMessage(GlobalReference.GlobalValues.TagWrapper.WrapInTag(serializedData, TagType.Data));
                        }
                        else
                        {
                            GlobalReference.GlobalValues.Logger.Log(LogLevel.ERROR, $"File {fileLocation} does not exit.");
                        }

                    }
                    catch (Exception ex)
                    {
                        GlobalReference.GlobalValues.Logger.Log(Global.Logging.LogSettings.LogLevel.ERROR, string.Format("Unable to read file at {0}. {1} {2}", fileLocation, ex.Message, ex.StackTrace));
                    }
                    break;

            }
        }

        public string DequeueCommunication()
        {
            string communication;
            _communicationQueue.TryDequeue(out communication);
            return communication;
        }
        public string DequeueCommand()
        {
            string command;
            _commandQueue.TryDequeue(out command);
            return command;
        }
        #endregion Message/Commands
    }
}
