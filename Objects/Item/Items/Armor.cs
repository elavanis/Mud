using Objects.Die.Interface;
using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Material.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Armor : Equipment, IArmor
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Obsolete("Needed for deserialization.", true)]
        public Armor() : base(AvalableItemPosition.NotSet, null, null, null, null) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Armor(IDice dice, AvalableItemPosition position, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(position, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Dice = dice;
        }

        public Armor(int level, AvalableItemPosition position, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : this(GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level), position, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Level = level;
        }

        [ExcludeFromCodeCoverage]
        public IDice Dice { get; set; }

        #region DamageType
        [ExcludeFromCodeCoverage]
        public decimal Acid { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Bludgeon { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Cold { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Fire { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Force { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Lightning { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Necrotic { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Pierce { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Poison { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Psychic { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Radiant { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Slash { get; set; } = decimal.MaxValue;
        [ExcludeFromCodeCoverage]
        public decimal Thunder { get; set; } = decimal.MaxValue;
        #endregion DamageType

        public decimal GetTypeModifier(Damage.Damage.DamageType damageType)
        {
            switch (damageType)
            {
                case Damage.Damage.DamageType.Acid:
                    return Acid;
                case Damage.Damage.DamageType.Bludgeon:
                    return Bludgeon;
                case Damage.Damage.DamageType.Cold:
                    return Cold;
                case Damage.Damage.DamageType.Fire:
                    return Fire;
                case Damage.Damage.DamageType.Force:
                    return Force;
                case Damage.Damage.DamageType.Lightning:
                    return Lightning;
                case Damage.Damage.DamageType.Necrotic:
                    return Necrotic;
                case Damage.Damage.DamageType.Pierce:
                    return Pierce;
                case Damage.Damage.DamageType.Poison:
                    return Poison;
                case Damage.Damage.DamageType.Psychic:
                    return Psychic;
                case Damage.Damage.DamageType.Radiant:
                    return Radiant;
                case Damage.Damage.DamageType.Slash:
                    return Slash;
                case Damage.Damage.DamageType.Thunder:
                    return Thunder;
                default:
                    throw new System.Exception("Unknown damagetype " + damageType.ToString());
            }
        }

        [ExcludeFromCodeCoverage]
        public IMaterial? Material { get; set; } = null;

        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            base.FinishLoad(zoneObjectSyncValue);

            if (Material != null)
            {
                SetDefenses(Material);
            }
        }

        private void SetDefenses(IMaterial material)
        {
            Acid = material.Acid;
            Bludgeon = material.Bludgeon;
            Cold = material.Cold;
            Fire = material.Fire;
            Force = material.Force;
            Lightning = material.Lightning;
            Necrotic = material.Necrotic;
            Pierce = material.Pierce;
            Poison = material.Poison;
            Psychic = material.Psychic;
            Radiant = material.Radiant;
            Slash = material.Slash;
            Thunder = material.Thunder;
        }
    }
}
