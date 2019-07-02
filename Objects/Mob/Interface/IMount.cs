using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob.Interface
{
    public interface IMount : IMobileObject
    {
        IMobileObject Owner { get; set; }
        int Movement { get; set; }
        int StaminaMultiplier { get; set; }
        SummonType SummonType { get; set; }
        int MaxRiders { get; set; }
        List<IMobileObject> Riders { get; set; }

    }
}
