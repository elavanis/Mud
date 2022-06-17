using Objects.Damage.Interface;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Item.Items.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Weapon : Equipment, IWeapon
    {
        public Weapon(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        public List<IDamage> DamageList { get; } = new List<IDamage>();

        public WeaponType Type { get; set; } = WeaponType.NotSet;

        [ExcludeFromCodeCoverage]
        /// <summary>
        /// bigger numbers mean slower
        /// </summary>
        public int Speed { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public int RequiredHands { get; set; }

        [ExcludeFromCodeCoverage]
        public Stats.Stat AttackerStat { get; set; } = Stats.Stat.Dexterity;
        [ExcludeFromCodeCoverage]
        public Stats.Stat DeffenderStat { get; set; } = Stats.Stat.Dexterity;

        [ExcludeFromCodeCoverage]
        public long WeaponId { get; set; }

        public enum WeaponType
        {
            NotSet,
            #region Bludgeon
            Club,
            Mace,
            WizardStaff,
            #endregion Bludgeon
            #region Slash
            Axe,
            Sword,
            #endregion Slash
            #region Pierce
            Dagger,
            Pick,
            Spear
            #endregion Pierce
        }
    }
}
