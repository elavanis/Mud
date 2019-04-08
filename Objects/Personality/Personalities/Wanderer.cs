using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Objects.Global.Direction.Directions;

namespace Objects.Personality.Personalities
{
    public class Wanderer : IWanderer
    {
        public Wanderer()
        {

        }

        public Wanderer(int movementPercent)
        {
            MovementPercent = movementPercent;
        }

        [ExcludeFromCodeCoverage]
        public int MovementPercent { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public List<IBaseObjectId> NavigableRooms { get; } = new List<IBaseObjectId>();

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                if (GlobalReference.GlobalValues.Random.PercentDiceRoll(MovementPercent))
                {
                    if (npc.FollowTarget != null)
                    {
                        //check to see if the follow target is still alive,
                        //if not remove it 
                        if (npc.FollowTarget.Health <= 0)
                        {
                            npc.FollowTarget = null;
                        }
                    }

                    if (npc.FollowTarget == null
                        && npc.Room.CheckLeave(npc) == null)
                    {
                        return GenerateMoveCommand(npc);
                    }
                }
            }

            return command;
        }

        private string GenerateMoveCommand(INonPlayerCharacter npc)
        {
            IRoom currentRoom = npc.Room;
            IRoom nextRoom = null;
            List<string> validDirections = new List<string>();

            if (currentRoom.North != null && currentRoom.North.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.North) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.North.Zone].Rooms[currentRoom.North.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("N");
                }
            }

            if (currentRoom.East != null && currentRoom.East.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.East) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.East.Zone].Rooms[currentRoom.East.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("E");
                }
            }

            if (currentRoom.South != null && currentRoom.South.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.South) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.South.Zone].Rooms[currentRoom.South.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("S");
                }
            }

            if (currentRoom.West != null && currentRoom.West.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.West) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.West.Zone].Rooms[currentRoom.West.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("W");
                }
            }

            if (currentRoom.Up != null && currentRoom.Up.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.Up) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.Up.Zone].Rooms[currentRoom.Up.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("U");
                }
            }

            if (currentRoom.Down != null && currentRoom.Down.Zone == currentRoom.Zone
                && currentRoom.CheckLeaveDirection(npc, Direction.Down) == null)
            {
                nextRoom = GlobalReference.GlobalValues.World.Zones[currentRoom.Down.Zone].Rooms[currentRoom.Down.Room];
                if (NextRoomEnterable(nextRoom, currentRoom, npc))
                {
                    validDirections.Add("D");
                }
            }

            if (validDirections.Count > 0)
            {
                string directon = validDirections[GlobalReference.GlobalValues.Random.Next(validDirections.Count)];
                return directon;
            }
            else
            {
                return null;
            }
        }

        private bool NextRoomEnterable(IRoom nextRoom, IRoom currentRoom, INonPlayerCharacter npc)
        {
            if ((nextRoom.CheckEnter(npc) == null)
               && (NavigableRooms.Count == 0
                    || NavigableRooms.Where(x => x.Zone == nextRoom.Zone).Where(y => y.Id == nextRoom.Id).Count() >= 1))
            {
                return true;
            }

            return false;
        }
    }
}
