using MiscShared;
using Objects.Damage;
using Objects.Damage.Interface;
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
using static Objects.Damage.Damage;
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
        protected int ItemId { get; set; } = 1;
        private int NpcId { get; set; } = 1;

        public BaseZone(int zoneId)
        {
            Zone = new Zone();
            Zone.Id = zoneId;

            if (!GlobalReference.GlobalValues.World.Zones.TryAdd(zoneId, Zone))
            {
                GlobalReference.GlobalValues.World.Zones[zoneId] = Zone;
            }
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
                    room.ZoneId = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }
        }

        #region NPC
        public INonPlayerCharacter CreateNonplayerCharacter(MobType typeOfMob, IRoom room, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription, int level = 0, string? corpseDescription = null)
        {
            INonPlayerCharacter npc = new NonPlayerCharacter(room, examineDescription, lookDescription, sentenceDescription, shortDescription, corpseDescription);
            npc.Id = NpcId++;
            npc.ZoneId = Zone.Id;
            npc.TypeOfMob = typeOfMob;
            npc.Level = level;

            return npc;
        }
        #endregion NPC

        #region Item
        public IWeapon CreateWeapon(WeaponType weaponType, int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IWeapon weapon = new Weapon(examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.Id = ItemId++;
            weapon.ZoneId = Zone.Id;
            weapon.Type = weaponType;
            weapon.Level = level;

            DamageType damageType = DamageType.NotSet;
            switch (weaponType)
            {
                case WeaponType.Club:
                case WeaponType.Mace:
                case WeaponType.WizardStaff:
                    damageType = Damage.DamageType.Bludgeon;
                    break;

                case WeaponType.Axe:
                case WeaponType.Sword:
                    damageType = Damage.DamageType.Slash;
                    break;

                case WeaponType.Dagger:
                case WeaponType.Pick:
                case WeaponType.Spear:
                    damageType = Damage.DamageType.Slash;
                    break;
            }

            IDamage damage = new Damage(GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level), damageType);
            weapon.DamageList.Add(damage);
            weapon.KeyWords.Add(weaponType.ToString());

            return weapon;
        }

        public IArmor CreateArmor(AvalableItemPosition position, int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription, BaseMaterial? material = null)
        {
            IArmor armor = new Armor(level, position, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.Id = ItemId++;
            armor.ZoneId = Zone.Id;
            armor.ItemPosition = position;
            armor.Level = level;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level);
            armor.Material = material;

            return armor;
        }

        public IShield CreateShield(int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription, BaseMaterial? material = null)
        {
            IShield shield = new Shield(level, examineDescription, lookDescription, sentenceDescription, shortDescription);
            shield.Id = ItemId++;
            shield.ZoneId = Zone.Id;
            shield.Level = level;
            shield.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(level);
            shield.Material = material;

            return shield;
        }

        public IEquipment CreateEquipment(AvalableItemPosition avalableItemPosition, int level, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IEquipment equipment = new Equipment(avalableItemPosition, examineDescription, lookDescription, sentenceDescription, shortDescription);
            equipment.Id = ItemId++;
            equipment.ZoneId = Zone.Id;
            equipment.Level = level;

            return equipment;
        }

        #region Create Stuff
        #region No Params
        public IRecallBeacon CreateRecallBeacon()
        {
            IRecallBeacon item = new RecallBeacon();
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;
            return item;
        }
        public IMoney CreateMoney() 
        {
            Money item = new Money();
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;
            return item;
        }
        #endregion No Params

        #region descriptions
        public IItem CreateItem(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IItem item = new Item(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;

            return item;
        }

        public Fountain CreateFountain(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            Fountain item = new Fountain(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;

            return item;
        }

        public IEnchantery CreateEnchantery(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IEnchantery item = new Enchantery(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;

            return item;
        }
        #endregion descriptions

        #region Openable

        public Container CreateContainer(string openMessage, string closeMessage, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription)
        {
            Container item = new Container(openMessage, closeMessage, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Id = ItemId++;
            item.ZoneId = Zone.Id;

            return item;
        }
        #endregion Openable
        #endregion Create Stuff
        #endregion Item

        #region Room
        private IRoom CreateRoom(int zoneId, string examineDescription, string lookDescription, string shortDescription, int movementCost = 1)
        {
            IRoom room = new Room(zoneId, examineDescription, lookDescription, shortDescription);
            room.Id = RoomId++;
            room.ZoneId = Zone.Id;
            room.MovementCost = movementCost;

            return room;
        }

        public virtual IRoom OutdoorRoom(int zoneId, string examineDescription, string lookDescription, string shortDescription, int movementCost = 1)
        {
            IRoom room = CreateRoom(zoneId, examineDescription, lookDescription, shortDescription, movementCost);
            room.Attributes.Add(RoomAttribute.Outdoor);
            room.Attributes.Add(RoomAttribute.Weather);

            return room;
        }

        public virtual IRoom IndoorRoomLight(int zoneId, string examineDescription, string lookDescription, string shortDescription, int movementCost = 1)
        {
            IRoom room = CreateRoom(zoneId, examineDescription, lookDescription, shortDescription, movementCost);
            room.Attributes.Add(RoomAttribute.Indoor);
            room.Attributes.Add(RoomAttribute.Light);

            return room;
        }

        public virtual IRoom IndoorRoomNoLight(int zoneId, string examineDescription, string lookDescription, string shortDescription, int movementCost = 1)
        {
            IRoom room = CreateRoom(zoneId, examineDescription, lookDescription, shortDescription, movementCost);
            room.Attributes.Add(RoomAttribute.Indoor);
            room.Attributes.Add(RoomAttribute.NoLight);

            return room;
        }
        #endregion Room
    }
}