using Objects.Command.Interface;
using Objects.Interface;
using Objects.Mob.Interface;

namespace Objects.Global.CanMobDoSomething.Interface
{
    public interface ICanMobDoSomething
    {
        bool SeeDueToLight(IMobileObject mob);

        bool SeeObject(IMobileObject observer, IBaseObject objectToSee);

        IResult Move(IMobileObject performer);

        bool Hear(IMobileObject observer, IBaseObject objectToHear);
    }
}