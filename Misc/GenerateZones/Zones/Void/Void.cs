using System.Linq;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Mob.SpecificNPC;
using Objects.Item.Items.Interface;
using Objects.Item.Items;
using Objects.Damage.Interface;
using Objects.Damage;
using Objects.Die;
using Objects.Mob.Interface;
using Objects.Mob;

namespace GenerateZones.Zones.LoadingArea
{
    public class Void : BaseZone, IZoneCode
    {
        public Void() : base(-1)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(Void);

            BuildRoomsViaReflection(this.GetType());

            return Zone;
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom(Zone.Id, "You float weightless in the void between realms.", "This room is an endless nothing.", "The Void");
            room.Attributes.Add(Room.RoomAttribute.Outdoor);

            return room;
        }

        #endregion Rooms
    }
}

