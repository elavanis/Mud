using Objects.Damage.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Command.Interface;
using static Objects.Global.Stats.Stats;

namespace Objects.Global.Engine.Engines.Interface
{
    public interface ICombat
    {
        void ProcessCombatRound();

        IResult AddCombatPair(IMobileObject attacker, IMobileObject defender);
        bool AreFighting(IMobileObject mob, IMobileObject mob2);
        bool IsInCombat(IMobileObject mob);
        IMobileObject? Opponet(IMobileObject mobileObject);
    }
}
