using MiscShared;
using Objects.Global;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Material;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;
using System;
using System.Linq;
using System.Reflection;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

namespace GenerateZones.Zones
{
    public class BaseZone
    {
        public IZone Zone { get; set; }
        private int RoomId { get; set; } = 1;
        private int ItemId { get; set; } = 1;
        private int NpcId { get; set; } = 1;

        public BaseZone(int zoneId)
        {
            Zone = new Zone();
            Zone.Id = zoneId;
        }

        public void BuildRoomsViaReflection(Type type)
        {
            int methodCount = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }
        }

        #region NPC
        public INonPlayerCharacter CreateNonplayerCharacter(MobType typeOfMob, int level = 0)
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = NpcId++;
            npc.Zone = Zone.Id;
            npc.TypeOfMob = typeOfMob;
            npc.Level = level;

            return npc;
        }
        #endregion NPC

        #region Item
        public IWeapon CreateWeapon(WeaponType weaponType, int level)
        {
            IWeapon weapon = new Weapon();
            weapon.Id = ItemId++;
            weapon.Zone = Zone.Id;
            weapon.Type = weaponType;
            weapon.Level = level;

            return weapon;
        }

        public IArmor CreateArmor(AvalableItemPosition position, int level, BaseMaterial material = null)
        {
            IArmor armor = new Armor();
            armor.Id = ItemId++;
            armor.Zone = Zone.Id;
            armor.ItemPosition = position;
            armor.Level = level;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level);
            armor.Material = material;

            return armor;
        }

        public IShield CreateShield(int level, BaseMaterial material = null)
        {
            IShield shield = new Shield();
            shield.Id = ItemId++;
            shield.Zone = Zone.Id;
            shield.Level = level;
            shield.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level);
            shield.Material = material;

            return shield;
        }

        public IEquipment CreateEquipment(int level)
        {
            IEquipment equipment = new Equipment();
            equipment.Id = ItemId++;
            equipment.Zone = Zone.Id;
            equipment.Level = level;

            return equipment;
        }

        public T CreateItem<T>()
        {
            Type type = typeof(T);
            IItem item = null;

            if (type == typeof(IItem)
                || type == typeof(Item))
            {
                item = new Item();
            }
            else if (type == typeof(Fountain))
            {
                item = new Fountain();
            }
            else if (type == typeof(IRecallBeacon)
                || type == typeof(RecallBeacon))
            {
                item = new RecallBeacon();
            }
            else if (type == typeof(IContainer)
                || type == typeof(Container))
            {
                item = new Container();
            }
            else if (type == typeof(IEnchantery)
                || type == typeof(Enchantery))
            {
                item = new Enchantery();
            }

            if (item == null)
            {
                throw new Exception($"Unsupported type {type.ToString()}");
            }
            else
            {
                item.Id = ItemId++;
                item.Zone = Zone.Id;
            }

            return (T)item;
        }
        #endregion Item

        #region Room
        public IRoom CreateRoom(int movementCost = 1)
        {
            IRoom room = new Room();
            room.Id = RoomId++;
            room.Zone = Zone.Id;
            room.MovementCost = movementCost;

            return room;
        }

        public virtual IRoom OutdoorRoom(int movementCost = 1)
        {
            IRoom room = CreateRoom(movementCost);
            room.Attributes.Add(RoomAttribute.Outdoor);
            room.Attributes.Add(RoomAttribute.Weather);

            return room;
        }

        public virtual IRoom IndoorRoomLight(int movementCost = 1)
        {
            IRoom room = CreateRoom(movementCost);
            room.Attributes.Add(RoomAttribute.Indoor);
            room.Attributes.Add(RoomAttribute.Light);

            return room;
        }

        public virtual IRoom IndoorRoomNoLight(int movementCost = 1)
        {
            IRoom room = CreateRoom(movementCost);
            room.Attributes.Add(RoomAttribute.Indoor);
            room.Attributes.Add(RoomAttribute.NoLight);

            return room;
        }
        #endregion Room
    }
}