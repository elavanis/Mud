using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Mob.Mount;

namespace Objects.Mob.Interface
{
    public interface IMount : IMobileObject
    {
        int Movement { get; set; }
        int StaminaMultiplier { get; set; }
        int MaxRiders { get; set; }
        List<IMobileObject> Riders { get; set; }
    }
}
