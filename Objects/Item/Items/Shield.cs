using Objects.Die.Interface;
using Objects.Item.Items.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Shield : Armor, IShield
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Obsolete("Needed for deserialization.", true)]
        public Shield() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Shield(IDice dice, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(dice, AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        public Shield(int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(level, AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        [ExcludeFromCodeCoverage]
        public int NegateDamagePercent { get; set; }
    }
}
