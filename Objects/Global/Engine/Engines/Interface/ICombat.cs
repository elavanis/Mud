using Objects.Damage.Interface;
using Objects.Global.Stats;
using Objects.Mob;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Command.Interface;
using static Objects.Global.Stats.Stats;
using Objects.Interface;

namespace Objects.Global.Engine.Engines.Interface
{
    public interface ICombat
    {
        void ProcessCombatRound();

        IResult AddCombatPair(IMobileObject attacker, IMobileObject defender);
        bool DetermineIfHit(IMobileObject attacker, IMobileObject defender, Stat attackerStat, Stat defenderStat);
        int DealDamage(IMobileObject attacker, IMobileObject defender, IDamage damage);

        bool AreFighting(IMobileObject mob, IMobileObject mob2);
        bool IsInCombat(IMobileObject mob);
        IMobileObject Opponet(MobileObject mobileObject);
    }
}
