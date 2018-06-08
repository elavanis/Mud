using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.Sound;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
                string serializeSounds;

                if (Sound.RandomSounds.Count > 1)
                {
                    //only pick one of the sounds to play, but we need to make a copy of it to serialize
                    ISound localSound = new Sound();
                    localSound.Loop = Sound.Loop;
                    localSound.SoundName = Sound.RandomSounds[GlobalReference.GlobalValues.Random.Next(Sound.RandomSounds.Count)];
                    serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { localSound });
                }
                else
                {
                    serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { Sound });
                }

                GlobalReference.GlobalValues.Notify.Room(null, null, room, new TranslationMessage(serializeSounds, TagType.Sound));
            }
        }
    }
}
