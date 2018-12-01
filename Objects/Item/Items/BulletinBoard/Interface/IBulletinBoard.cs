using Objects.Command.Interface;
using Objects.Mob.Interface;

namespace Objects.Item.Items.BulletinBoard.Interface
{
    public interface IBulletinBoard
    {
        ulong CalculateCost(IMobileObject performer);
        void FinishLoad(int zoneObjectSyncValue = -1);
        void Post(IMobileObject performer, string subject, string text);
        IResult Read(IMobileObject performer, int postNumber);
        IResult Remove(IMobileObject performer, int postNumber);
    }
}