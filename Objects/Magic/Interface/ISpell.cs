using Objects.Ability.Interface;
using Objects.Command.Interface;
using Objects.Interface;
using Objects.Mob.Interface;

namespace Objects.Magic.Interface
{
    public interface ISpell : ILearnable, IAbility
    {
        int ManaCost { get; set; }
        string SpellName { get; set; }

        IResult ProcessSpell(IMobileObject performer, ICommand command);
    }
}