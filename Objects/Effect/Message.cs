using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Effect
{
    public class Message : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            IRoom room = GlobalReference.GlobalValues.World.Zones[parameter.RoomId.Zone].Rooms[parameter.RoomId.Id];
            GlobalReference.GlobalValues.Notify.Room(parameter.Performer, parameter.Target, room, parameter.RoomMessage);

            if (Sound != null)
            {
                string serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { Sound });

                GlobalReference.GlobalValues.Notify.Room(null, null, room, new TranslationMessage(serializeSounds, TagType.Sound));
            }
        }
    }
}
