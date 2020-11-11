using Objects.Command.Interface;
using Objects.Mob.Interface;

namespace Objects.Item.Items.Custom.UnderGrandViewCastle.Interface
{
    public interface IRunicStatue
    {
        int SelectedRune { get; }

        string CalculateExamDescription();
        IResult Turn(IMobileObject performer, ICommand command);
    }
}