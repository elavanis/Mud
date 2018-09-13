using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Effect
{
    public class LoadMob : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public List<int> HoursToLoad { get; set; } = null;

        public void ProcessEffect(IEffectParameter parameter)
        {
            IRoom room = parameter.Target as IRoom;
            INonPlayerCharacter nonPlayerCharacter = parameter.Performer as INonPlayerCharacter;

            if (room != null && nonPlayerCharacter != null
                && (HoursToLoad == null || HoursToLoad.Contains(GlobalReference.GlobalValues.GameDateTime.InGameDateTime.Hour)))
            {
                //clone it so we don't keep using the same mob instance
                nonPlayerCharacter = (INonPlayerCharacter)nonPlayerCharacter.Clone();
                room.AddMobileObjectToRoom(nonPlayerCharacter);
                nonPlayerCharacter.Room = room;
                nonPlayerCharacter.FinishLoad();

                GlobalReference.GlobalValues.Notify.Room(nonPlayerCharacter, null, room, parameter.RoomMessage);
            }
        }
    }
}
