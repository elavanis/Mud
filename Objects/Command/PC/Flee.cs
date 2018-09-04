using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Direction;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace Objects.Command.PC
{
    public class Flee : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Flee {Direction}", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Flee" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (performer.IsInCombat)
            {
                int attackerRoll = 0;
                int performerRoll = 0;
                IMobileObject attacker = performer.Opponent;

                RollToGetAway(performer, attacker, ref attackerRoll, ref performerRoll);

                if (performerRoll >= attackerRoll && !attacker.AttributesCurrent.Contains(Mob.MobileObject.MobileAttribute.NoFlee))
                {
                    return RunAway(performer, command.Parameters);
                }
                else
                {
                    return new Result("You attempt to run away but are not able to.", false);  //return false so the mob does not get another move
                }
            }
            else
            {
                return new Result("You run around waving your arms and shouting \"Somebody save me.\" but then realize nothing is fighting you.", true);
            }
        }

        private static void RollToGetAway(IMobileObject performer, IMobileObject attacker, ref int attackerRoll, ref int performerRoll)
        {
            if (attacker != null)
            {
                attackerRoll = GlobalReference.GlobalValues.Random.Next(attacker.DexterityEffective);
                performerRoll = GlobalReference.GlobalValues.Random.Next(performer.DexterityEffective);
            }
        }

        private static IResult RunAway(IMobileObject performer, List<IParameter> parameter)
        {
            IResult result = performer.Room.CheckFlee(performer);
            if (result == null)
            {
                bool redirect = false;
                List<Direction> exitDirections = GetRoomExits(performer);

                bool directionChoosen = false;
                Direction chosenDirection = Direction.Down;

                if (parameter.Count > 0)
                {
                    Direction? localDirection = null;
                    string direction = parameter[0].ParameterValue;

                    switch (direction.ToUpper())
                    {
                        case "N":
                        case "NORTH":
                            localDirection = Direction.North;
                            break;
                        case "E":
                        case "EAST":
                            localDirection = Direction.East;
                            break;
                        case "S":
                        case "SOUTH":
                            localDirection = Direction.South;
                            break;
                        case "W":
                        case "WEST":
                            localDirection = Direction.West;
                            break;
                        case "U":
                        case "UP":
                            localDirection = Direction.Up;
                            break;
                        case "D":
                        case "DOWN":
                            localDirection = Direction.Down;
                            break;
                    }

                    if (localDirection != null)
                    {
                        if (exitDirections.Contains((Direction)localDirection))
                        {
                            directionChoosen = true;
                            chosenDirection = (Direction)localDirection;
                        }
                        else
                        {
                            redirect = true;
                            chosenDirection = PickDirection(exitDirections);
                            GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage($"You tried to flee {localDirection} but were unable to instead fled {chosenDirection}."));
                        }
                    }
                    else
                    {
                        chosenDirection = PickDirection(exitDirections);
                    }
                }

                if (!directionChoosen)
                {
                    chosenDirection = PickDirection(exitDirections);
                }

                IRoom proposedRoom = GetProposedRoom(performer.Room, chosenDirection);

                if (performer.Room.Leave(performer, chosenDirection))
                {
                    performer.Room = proposedRoom;
                    proposedRoom.Enter(performer);

                    if (!redirect)
                    {
                        GlobalReference.GlobalValues.Notify.Mob(performer, new TranslationMessage($"You flee {chosenDirection}."));
                    }
                    return GlobalReference.GlobalValues.CommandList.PcCommandsLookup["LOOK"].PerformCommand(performer, new Command());
                }

                //the character was moved before they could move to the desired room.
                return new Result(null, false);
            }
            else
            {
                return result;
            }
        }

        private static IRoom GetProposedRoom(IRoom room, Direction chosenDirection)
        {
            switch (chosenDirection)
            {
                case Direction.North:
                    return GlobalReference.GlobalValues.World.Zones[room.North.Zone].Rooms[room.North.Room];
                case Direction.East:
                    return GlobalReference.GlobalValues.World.Zones[room.East.Zone].Rooms[room.East.Room];
                case Direction.South:
                    return GlobalReference.GlobalValues.World.Zones[room.South.Zone].Rooms[room.South.Room];
                case Direction.West:
                    return GlobalReference.GlobalValues.World.Zones[room.West.Zone].Rooms[room.West.Room];
                case Direction.Up:
                    return GlobalReference.GlobalValues.World.Zones[room.Up.Zone].Rooms[room.Up.Room];
                case Direction.Down:
                    return GlobalReference.GlobalValues.World.Zones[room.Down.Zone].Rooms[room.Down.Room];
                default:
                    throw new Exception($"Unknown direction. {chosenDirection}");
            }

        }

        private static Direction PickDirection(List<Direction> exitDirections)
        {
            return exitDirections[GlobalReference.GlobalValues.Random.Next(exitDirections.Count)];
        }

        private static List<Direction> GetRoomExits(IMobileObject performer)
        {
            List<Direction> exitDirections = new List<Direction>();
            IRoom room = performer.Room;

            if (room.North != null && (room.North.Door == null || room.North.Door.Opened))
            {
                exitDirections.Add(Direction.North);
            }

            if (room.East != null && (room.East.Door == null || room.East.Door.Opened))
            {
                exitDirections.Add(Direction.East);
            }

            if (room.South != null && (room.South.Door == null || room.South.Door.Opened))
            {
                exitDirections.Add(Direction.South);
            }

            if (room.West != null && (room.West.Door == null || room.West.Door.Opened))
            {
                exitDirections.Add(Direction.West);
            }

            if (room.Up != null && (room.Up.Door == null || room.Up.Door.Opened))
            {
                exitDirections.Add(Direction.Up);
            }

            if (room.Down != null && (room.Down.Door == null || room.Down.Door.Opened))
            {
                exitDirections.Add(Direction.Down);
            }

            return exitDirections;
        }
    }
}
