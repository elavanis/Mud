using Objects.Command.Interface;
using Objects.Global.Direction;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Trap.Interface;
using Shared.Sound.Interface;
using System.Collections.Generic;

namespace Objects.Room.Interface
{
    public interface IRoom : IBaseObject, ILoadable
    {
        void AddMobileObjectToRoom(IMobileObject mob);
        bool RemoveMobileObjectFromRoom(IMobileObject mob);

        void AddItemToRoom(IItem item, int position = 0);
        bool RemoveItemFromRoom(IItem item);

        string Owner { get; set; }
        HashSet<string> Guests { get; set; }

        HashSet<Room.RoomAttribute> Attributes { get; }
        IExit North { get; set; }
        IExit East { get; set; }
        IExit South { get; set; }
        IExit West { get; set; }
        IExit Up { get; set; }
        IExit Down { get; set; }
        IReadOnlyList<IItem> Items { get; }
        IReadOnlyList<INonPlayerCharacter> NonPlayerCharacters { get; }
        IReadOnlyList<IMobileObject> OtherMobs { get; }

        IReadOnlyList<IPlayerCharacter> PlayerCharacters { get; }
        List<ITrap> Traps { get; }
        int MovementCost { get; set; }

        List<ISound> Sounds { get; }
        string SerializedSounds { get; }
        string PrecipitationNotification { get; }
        string WindSpeedNotification { get; }
        string RoomPrecipitationExtraHighBegin { get; set; }
        string RoomPrecipitationExtraHighEnd { get; set; }
        string RoomPrecipitationExtraLowBegin { get; set; }
        string RoomPrecipitationExtraLowEnd { get; set; }
        string RoomPrecipitationHighBegin { get; set; }
        string RoomPrecipitationHighEnd { get; set; }
        string RoomPrecipitationLowBegin { get; set; }
        string RoomPrecipitationLowEnd { get; set; }
        string RoomWindSpeedExtraHighBegin { get; set; }
        string RoomWindSpeedExtraHighEnd { get; set; }
        string RoomWindSpeedExtraLowBegin { get; set; }
        string RoomWindSpeedExtraLowEnd { get; set; }
        string RoomWindSpeedHighBegin { get; set; }
        string RoomWindSpeedHighEnd { get; set; }
        string RoomWindSpeedLowBegin { get; set; }
        string RoomWindSpeedLowEnd { get; set; }

        IResult CheckEnter(IMobileObject mobileObject);
        IResult CheckFlee(IMobileObject mobileObject);
        IResult CheckLeave(IMobileObject mobileObject);
        IResult CheckLeaveDirection(IMobileObject mobileObject, Directions.Direction direction);
        void Enter(IMobileObject performer);
        bool Leave(IMobileObject performer, Directions.Direction direction, bool mounted);

    }
}
