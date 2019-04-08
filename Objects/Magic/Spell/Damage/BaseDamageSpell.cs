using Objects.Global;
using Objects.Magic.Spell.Generic;
using static Objects.Damage.Damage;

namespace Objects.Magic.Spell.Damage
{
    public abstract class BaseDamageSpell : SingleTargetSpell
    {
        public BaseDamageSpell(string spellName, int die, int sides, DamageType damageType, int manaCost = -1)
        {
            Effect = new Objects.Effect.Damage();
            Parameter.Dice = GlobalReference.GlobalValues.DefaultValues.ReduceValues(die, sides);
            Parameter.Damage = new Objects.Damage.Damage(Parameter.Dice);
            Parameter.Damage.Type = damageType;

            SpellName = spellName;
            if (manaCost == -1)
            {
                ManaCost = sides * die / 20;
            }
            else
            {
                ManaCost = manaCost;
            }
        }
    }
}