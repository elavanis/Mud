using Objects.Effect.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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

                OpenIfMatched(room.North?.Door);
                OpenIfMatched(room.East?.Door);
                OpenIfMatched(room.South?.Door);
                OpenIfMatched(room.West?.Door);
            }
        }

        private void OpenIfMatched(IDoor door)
        {
            if (door != null)
            {
                if (door.Zone == Door.Zone && door.Id == Door.Id)
                {
                    door.Opened = true;
                }
            }
        }
    }
}
