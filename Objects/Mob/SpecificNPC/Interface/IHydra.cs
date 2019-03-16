using Objects.Damage.Interface;
using Objects.Mob.Interface;

namespace Objects.Mob.SpecificNPC.Interface
{
    public interface IHydra : INonPlayerCharacter
    {
        new int Level { get; set; }

        void RegrowHeads();
        new int TakeCombatDamage(int totalDamage, IDamage damage, IMobileObject attacker, uint combatRound);
        new int TakeDamage(int totalDamage, IDamage damage, IMobileObject attacker);
    }
}