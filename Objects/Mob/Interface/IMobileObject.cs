using System.Collections.Generic;
using Objects.Damage.Interface;
using Objects.Global.Stats;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Race.Interface;
using Objects.Room.Interface;
using Objects.Skill.Interface;
using Objects.Interface;
using Objects.Magic.Interface;
using static Objects.Global.Language.Translator;

namespace Objects.Mob.Interface
{
    public interface IMobileObject : IBaseObject
    {
        IEnumerable<MobileObject.MobileAttribute> AttributesCurrent { get; }
        int CharismaEffective { get; }
        int CharismaStat { get; set; }
        int CommmandQueueCount { get; }
        int ConstitutionEffective { get; }
        int ConstitutionStat { get; set; }
        string CorpseDescription { get; set; }
        int DexterityEffective { get; }
        int DexterityStat { get; set; }

        IMobileObject? PossingMob { get; set; }
        IMobileObject? PossedMob { get; set; }

        IEnumerable<IArmor> EquipedArmor { get; }
        IEnumerable<IEquipment> EquipedEquipment { get; }
        IEnumerable<IWeapon> EquipedWeapon { get; }
        IMobileObject? FollowTarget { get; set; }
        bool God { get; set; }
        HashSet<Guild.Guild.Guilds> Guild { get; set; }
        int GuildPoints { get; set; }
        int Health { get; set; }
        string HealthDescription { get; }

        int IntelligenceEffective { get; }
        int IntelligenceStat { get; set; }
        bool IsAlive { get; set; }
        bool IsInCombat { get; }
        IMobileObject? Opponent { get; }
        List<IItem> Items { get; }
        Dictionary<string, ISkill> KnownSkills { get; }
        HashSet<Languages> KnownLanguages { get; set; }

        uint LastProccessedTick { get; set; }
        int Level { get; set; }
        int LevelPoints { get; set; }
        int Mana { get; set; }
        int MaxHealth { get; set; }
        int MaxMana { get; set; }
        int MaxStamina { get; set; }
        ulong Money { get; set; }
        MobileObject.CharacterPosition Position { get; set; }
        IRace Race { get; set; }
        IBaseObjectId? RecallPoint { get; set; }
        IRoom Room { get; set; }
        IBaseObjectId RoomId { get; set; }
        Dictionary<string, ISpell> SpellBook { get; }
        int Stamina { get; set; }
        int StrengthEffective { get; }
        int StrengthStat { get; set; }
        int WisdomEffective { get; }
        int WisdomStat { get; set; }

        void AddEquipment(IEquipment equipment);
        bool AreFighting(IMobileObject mob);
        int CalculateAttackOrderRoll();
        int CalculateDamage(IDamage damage);
        int CalculateDamageBlocked(IArmor armor);
        int CalculateToDodgeRoll(Stats.Stat stat, long weaponId, uint combatRound);
        int CalculateToHitRoll(Stats.Stat stat);
        string? DequeueCommand();
        string? DequeueCommunication();
        string? DequeueMessage();
        ICorpse Die(IMobileObject attacker);
        void EnqueueCommand(string message);
        void EnqueueMessage(string message);
        int GetStatEffective(Stats.Stat? stat);
        void LevelMobileObject();
        void RemoveEquipment(IEquipment equipment);
        void ResetMaxStatValues();
        int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker);
        int TakeDamage(int totalDamage, IDamage damage, string attackerDescription);
        int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound);
        IMount? Mount { get; set; }
    }
}