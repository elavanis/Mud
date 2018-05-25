using Objects.Mob;
using Objects.Race.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Race
{
    public abstract class BaseRace : IRace
    {
        [ExcludeFromCodeCoverage]
        public virtual double ExperanceModifier { get; } = 1;


        protected List<MobileObject.MobileAttribute> raceAttributes;
        public virtual List<MobileObject.MobileAttribute> RaceAttributes
        {
            get
            {
                if (raceAttributes == null)
                {
                    raceAttributes = new List<MobileObject.MobileAttribute>();
                }
                return raceAttributes;
            }
        }

        #region DamageType
        [ExcludeFromCodeCoverage]
        public decimal Acid { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Bludgeon { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Cold { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Fire { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Force { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Lightning { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Necrotic { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Pierce { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Poison { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Psychic { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Radiant { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Slash { get; set; } = 1;
        [ExcludeFromCodeCoverage]
        public decimal Thunder { get; set; } = 1;
        #endregion DamageType

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
