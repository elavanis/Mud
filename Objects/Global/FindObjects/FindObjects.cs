using Objects.Item.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Interface;
using static Shared.TagWrapper.TagWrapper;
using Objects.Item.Items.Interface;
using Objects.Global.FindObjects.Interface;

namespace Objects.Global.FindObjects
{
    public class FindObjects : IFindObjects
    {
        public IItem FindHeldItemsOnMob(IMobileObject mob, string keyword, int itemNumber)
        {
            List<IItem> heldItems = FindHeldItemsOnMob(mob, keyword);
            if (heldItems.Count > itemNumber)
            {
                return heldItems[itemNumber];
            }
            else
            {
                return null;
            }
        }

        public List<IItem> FindHeldItemsOnMob(IMobileObject mob, string keyword)
        {
            List<IItem> heldItems = mob.Items.Where(i => i.KeyWords.Contains(keyword, StringComparer.OrdinalIgnoreCase)).ToList();
            return heldItems;
        }

        public IItem FindItemsInRoom(IRoom room, string keyword, int itemNumber)
        {
            List<IItem> itemsInRoom = FindItemsInRoom(room, keyword);
            if (itemsInRoom.Count > itemNumber)
            {
                return itemsInRoom[itemNumber];
            }
            else
            {
                return null;
            }
        }

        public List<IItem> FindItemsInRoom(IRoom room, string keyword)
        {
            List<IItem> heldItems = room.Items.Where(i => i.KeyWords.Contains(keyword, StringComparer.OrdinalIgnoreCase)).ToList();
            return heldItems;
        }

        public List<INonPlayerCharacter> FindNpcInRoom(IRoom room, string keyword)
        {
            List<INonPlayerCharacter> npc = room.NonPlayerCharacters.Where(i => i.KeyWords.Contains(keyword, StringComparer.OrdinalIgnoreCase)).ToList();
            return npc;
        }

        public List<IPlayerCharacter> FindPcInRoom(IRoom room, string keyword)
        {
            List<IPlayerCharacter> pc = room.PlayerCharacters.Where(i => i.KeyWords.Contains(keyword, StringComparer.OrdinalIgnoreCase)).ToList();
            return pc;
        }

        public List<IDoor> FindDoors(IRoom room, string keyword)
        {
            List<IDoor> doors = new List<IDoor>();
            AddDoors(room.North, keyword, doors);
            AddDoors(room.East, keyword, doors);
            AddDoors(room.South, keyword, doors);
            AddDoors(room.West, keyword, doors);
            AddDoors(room.Up, keyword, doors);
            AddDoors(room.Down, keyword, doors);

            return doors;
        }

        private void AddDoors(IExit exit, string keyword, List<IDoor> doors)
        {
            if (exit != null)
            {
                IDoor door = GetDoor(exit, keyword);
                if (door != null)
                {
                    doors.Add(door);
                }
            }
        }

        private IDoor GetDoor(IExit exit, string keyword)
        {
            if (exit.Door != null)
            {
                if (exit.Door.KeyWords.Any(s => s.Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    return exit.Door;
                }

            }
            return null;
        }

        public IBaseObject FindObjectOnPersonOrInRoom(IMobileObject mob, string keyword, int objectNumber, bool searchOnPerson = true, bool searchInRoom = true, bool searchNpc = true, bool searchPc = true, bool searchExits = true)
        {
            int objectPosition = objectNumber;
            if (searchOnPerson)
            {
                List<IItem> items = FindHeldItemsOnMob(mob, keyword);
                if (items.Count > objectPosition)
                {
                    return items[objectPosition];
                }

                objectPosition -= items.Count;
            }

            if (searchInRoom)
            {
                List<IItem> items = FindItemsInRoom(mob.Room, keyword);
                if (items.Count > objectPosition)
                {
                    return items[objectPosition];
                }

                objectPosition -= items.Count;
            }

            if (searchNpc)
            {
                List<INonPlayerCharacter> npcs = FindNpcInRoom(mob.Room, keyword);
                if (npcs.Count > objectPosition)
                {
                    return npcs[objectPosition];
                }

                objectPosition -= npcs.Count;
            }

            if (searchPc)
            {
                List<IPlayerCharacter> pcs = FindPcInRoom(mob.Room, keyword);
                if (pcs.Count > objectPosition)
                {
                    return pcs[objectPosition];
                }
                objectPosition -= pcs.Count;
            }

            if (searchExits)
            {
                List<IDoor> doors = FindDoors(mob.Room, keyword);
                if (doors.Count > objectPosition)
                {
                    return doors[objectPosition];
                }
            }

            return null;
        }



        public TagType DetermineFoundObjectTagType(IBaseObject baseObject)
        {
            if (baseObject is IItem item)
            {
                return TagType.Item;
            }

            if (baseObject is INonPlayerCharacter npc)
            {
                return TagType.NonPlayerCharacter;
            }

            if (baseObject is IPlayerCharacter pc)
            {
                return TagType.PlayerCharacter;
            }

            return TagType.Info;
        }
    }
}
