using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.Map.Interface
{
    public interface IMap
    {
        void SendMapPosition(IMobileObject mob);
    }
}
