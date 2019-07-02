using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob
{
    public class Mount : MobileObject, IMount
    {
        public IMobileObject Owner { get; set; }
        public int Movement { get; set; } = 2;
        public int StaminaMultiplier { get; set; } = 10;
        public SummonType SummonType { get; set; } = SummonType.Call;
        public int MaxRiders { get; set; } = 1;
        public List<IMobileObject> Riders { get; set; } = new List<IMobileObject>();
    }

    public enum SummonType
    {
        Summon,
        Call,
        None
    }
}
