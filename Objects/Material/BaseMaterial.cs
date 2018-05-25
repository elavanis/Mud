using Objects.Global;
using Objects.Material.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Material
{
    public abstract class BaseMaterial : IMaterial
    {
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

        public decimal Strong()
        {
            return Convert.ToDecimal(GlobalReference.GlobalValues.Random.Next(12, 15) / 10.0);
        }

        public decimal Moderate()
        {
            return Convert.ToDecimal(GlobalReference.GlobalValues.Random.Next(9, 12) / 10.0);
        }

        public decimal Weak()
        {
            return Convert.ToDecimal(GlobalReference.GlobalValues.Random.Next(6, 9) / 10.0);
        }
    }
}
