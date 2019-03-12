using Objects.Damage.Interface;
using Objects.Mob.Interface;

namespace Objects.Mob.SpecificNPC.Interface
{
    public interface IHydra : INonPlayerCharacter
    {
        int Level { get; set; }

        void RegrowHeads();
        int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound);
        int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker);
    }
}