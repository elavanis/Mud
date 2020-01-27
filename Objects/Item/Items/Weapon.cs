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
        public Weapon()
        {
            ItemPosition = AvalableItemPosition.Wield;
        }

        private List<IDamage> _damage = null;
        public List<IDamage> DamageList
        {
            get
            {
                if (_damage == null)
                {
                    _damage = new List<IDamage>();
                }
                return _damage;
            }
        }

        public WeaponType? Type { get; set; }

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

        private long _weaponId;
        public long WeaponId
        {
            get
            {
                return _weaponId;
            }
            set
            {
                _weaponId = value;
            }
        }

        public enum WeaponType
        {
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
