using Objects.Effect.Interface;
using Objects.GameDateTime;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect.Zone.EndlessDesert
{
    public class DoorwayToUnderworld : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            IRoom room = parameter.Target as IRoom;
            DoorChange doorChange = DoorChange.NoChange;
            if (room != null)
            {
                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.DayName == Days.Death
                    && GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour >= 12) //night
                {
                    if (room.Down == null)
                    {
                        doorChange = DoorChange.Open;
                    }
                }
                else
                {
                    if (room.Down != null)
                    {
                        doorChange = DoorChange.Close;
                    }
                }
            }

            switch (doorChange)
            {
                case DoorChange.Open:
                    OpenDoor(room);
                    break;
                case DoorChange.Close:
                    CloseDoor(room);
                    break;
            }
        }

        private void CloseDoor(IRoom room)
        {
            if (room.Down != null)
            {
                room.Down = null;
                room.Attributes.Remove(Room.Room.RoomAttribute.Light);
                string message = "As the sun starts to peek over the dunes the portal in the center of the lake disappears and the lake begins to thaw.";
                ITranslationMessage translationMessage = new TranslationMessage(message);
                GlobalReference.GlobalValues.Notify.Room(null, null, room, translationMessage);
            }
        }

        private void OpenDoor(IRoom room)
        {
            room.Down = new Exit() { Zone = -1, Room = -1 };
            room.Attributes.Add(Room.Room.RoomAttribute.Light);
            string message = "As the last rays of light disappear over the horizon the crackle of water quickly freezing can be heard originating from a shimmering gray portal that has appeared in the center of the now frozen lake.  The portal gives off an eerie gray light causing everything to look pale as if touched by dead while a cold breeze can be felt emanating from it adding to the effect.";
            ITranslationMessage translationMessage = new TranslationMessage(message);
            GlobalReference.GlobalValues.Notify.Room(null, null, room, translationMessage);
        }

        public enum DoorChange
        {
            NoChange,
            Open,
            Close
        }
    }
}
