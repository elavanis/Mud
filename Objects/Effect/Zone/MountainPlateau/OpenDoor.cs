using Objects.Effect.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Effect.Zone.MountainPlateau
{
    public class OpenDoor : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public IBaseObjectId Chest { get; set; }
        public IBaseObjectId Statue { get; set; }
        public IBaseObjectId Door { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            IBaseObject container = null;
            IItem statue = null;

            if (parameter.Container != null)
            {
                container = parameter.Container as IBaseObject;
            }

            statue = parameter.Item;

            if (container == null
                || statue == null)
            {
                return;
            }

            if (container.Zone == Chest.Zone && container.Id == Chest.Id
                && statue.Zone == Statue.Zone && statue.Id == Statue.Id)
            {
                IRoom room = parameter.ObjectRoom;

                OpenIfMatched(room.North?.Door, room);
                OpenIfMatched(room.East?.Door, room);
                OpenIfMatched(room.South?.Door, room);
                OpenIfMatched(room.West?.Door, room);
            }
        }

        private void OpenIfMatched(IDoor door, IRoom room)
        {
            if (door != null)
            {
                if (door.Zone == Door.Zone
                    && door.Id == Door.Id
                    && door.Opened == false)
                {
                    door.Opened = true;

                    if (Sound != null)
                    {
                        string serializeSounds = GlobalReference.GlobalValues.Serialization.Serialize(new List<ISound>() { Sound });
                        GlobalReference.GlobalValues.Notify.Room(null, null, room, new TranslationMessage(serializeSounds, TagType.Sound));
                    }
                }
            }
        }
    }
}
