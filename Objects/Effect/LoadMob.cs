using Objects.Effect.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    public class LoadMob : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        [ExcludeFromCodeCoverage]
        public IBaseObjectId RoomId { get; set; }

        public List<int> HoursToLoad { get; set; } = null;

        public void ProcessEffect(IEffectParameter parameter)
        {
            INonPlayerCharacter nonPlayerCharacter = parameter.Performer as INonPlayerCharacter;

            if (nonPlayerCharacter != null
                && (HoursToLoad == null || HoursToLoad.Contains(GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour)))
            {
                IRoom room;
                if (RoomId != null)
                {
                    try
                    {
                        room = GlobalReference.GlobalValues.World.Zones[RoomId.Zone].Rooms[RoomId.Id];
                    }
                    catch
                    {
                        //invalid room passed
                        return;
                    }
                }
                else
                {
                    room = parameter.Target as IRoom;
                    if (room == null)
                    {
                        //target is not a room so we can't load the mob;
                        return;
                    }
                }

                //clone it so we don't keep using the same mob instance
                nonPlayerCharacter = (INonPlayerCharacter)nonPlayerCharacter.Clone();
                room.AddMobileObjectToRoom(nonPlayerCharacter);
                nonPlayerCharacter.Room = room;
                nonPlayerCharacter.FinishLoad();

                if (parameter.RoomMessage != null)
                {
                    GlobalReference.GlobalValues.Notify.Room(nonPlayerCharacter, null, room, parameter.RoomMessage);
                }
            }
        }
    }
}
