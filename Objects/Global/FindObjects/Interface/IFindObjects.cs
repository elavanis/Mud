using System.Collections.Generic;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Interface;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Global.FindObjects.Interface
{
    public interface IFindObjects
    {
        IItem FindHeldItemsOnMob(IMobileObject mob, string keyword, int itemNumber);
        List<IItem> FindHeldItemsOnMob(IMobileObject mob, string keyword);

        List<IItem> FindItemsInRoom(IRoom room, string keyword);
        IItem FindItemsInRoom(IRoom room, string keyword, int itemNumber);

        List<INonPlayerCharacter> FindNpcInRoom(IRoom room, string keyword);
        List<IPlayerCharacter> FindPcInRoom(IRoom room, string keyword);

        IBaseObject FindObjectOnPersonOrInRoom(IMobileObject performer, string keyword, int itemNumber, bool searchOnPerson = true, bool searchInRoom = true, bool searchNpc = true, bool searchPc = true, bool searchExits = true);
        TagType DetermineFoundObjectTagType(IBaseObject baseObject);
    }
}