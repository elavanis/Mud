using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Global.Direction;
using Objects.Language;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Objects.Mob.MobileObject;
using static Objects.Room.Room;

namespace Objects.Skill.Skills
{
    public class Track : BaseSkill
    {
        public Track() : base(nameof(Track))
        {
            PerformerNotificationSuccess = new TranslationMessage("You search around for any sign of your target.");
            StaminaCost = 0;
        }

        public override string TeachMessage
        {
            get
            {
                return "A good tracker needs to be able to search for their target using any means possible.";
            }
        }

        public override IResult ProcessSkill(IMobileObject performer, ICommand command)
        {
            if (performer.Position == CharacterPosition.Sleep)
            {
                return new Result("You can not track while asleep.", true);
            }

            if (command.Parameters.Count > 1)
            {
                string target = command.Parameters[1].ParameterValue;
                IMobileObject foundMob = FindMobInRoom(performer.Room, target);

                if (foundMob != null)
                {
                    return new Result(string.Format("You look up and see the {0} in front of you.", target), false);
                }


                HashSet<IRoom> searchedRooms = new HashSet<IRoom>();
                searchedRooms.Add(performer.Room);
                return SearchOtherRooms(target, performer, searchedRooms);
            }
            else
            {
                return new Result("What are you trying to track?", true);
            }
        }

        private IMobileObject FindMobInRoom(IRoom roomToSearch, string target)
        {
            IMobileObject foundItem = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(roomToSearch, target).FirstOrDefault();
            if (foundItem == null)
            {
                foundItem = GlobalReference.GlobalValues.FindObjects.FindPcInRoom(roomToSearch, target).FirstOrDefault();
            }

            return foundItem;
        }

        private IResult SearchOtherRooms(string target, IMobileObject performer, HashSet<IRoom> searchedRooms)
        {
            Queue<Trail> newTrails = new Queue<Trail>();
            IRoom currentRoom = performer.Room;

            Trail trail = null;
            foreach (Directions.Direction direction in Enum.GetValues(typeof(Directions.Direction)))
            {
                Trail brandNewTrail = new Trail() { Direction = direction, Distance = 0 };
                trail = LookForMobInNextRoom(performer, target, searchedRooms, newTrails, currentRoom, direction, brandNewTrail);
                if (trail != null)
                {
                    return new Result(string.Format("You pickup the trail of a {0} to the {1}.", target, trail.Direction), false);
                }
            }


            while (newTrails.Count > 0)
            {
                Trail dequeuedTrail = newTrails.Dequeue();
                foreach (Directions.Direction direction in Enum.GetValues(typeof(Directions.Direction)))
                {
                    trail = LookForMobInNextRoom(performer, target, searchedRooms, newTrails, dequeuedTrail.Room, direction, dequeuedTrail);
                    if (trail != null)
                    {
                        return new Result(string.Format("You pickup the trail of a {0} to the {1}.", target, trail.Direction), false);
                    }
                }
            }

            return new Result(string.Format("You were unable to pick up a trail to a {0}.", target), false);
        }

        private Trail LookForMobInNextRoom(IMobileObject performer, string target, HashSet<IRoom> searchedRooms, Queue<Trail> newTrails, IRoom currentRoom, Directions.Direction direction, Trail existingTrail)
        {
            Trail trail = null;
            switch (direction)
            {
                case Directions.Direction.North:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.North, target, newTrails, existingTrail);
                    break;
                case Directions.Direction.East:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.East, target, newTrails, existingTrail);
                    break;
                case Directions.Direction.South:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.South, target, newTrails, existingTrail);
                    break;
                case Directions.Direction.West:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.West, target, newTrails, existingTrail);
                    break;
                case Directions.Direction.Up:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.Up, target, newTrails, existingTrail);
                    break;
                case Directions.Direction.Down:
                    trail = AddNextRoom(performer, searchedRooms, direction, currentRoom.Down, target, newTrails, existingTrail);
                    break;
            }

            return trail;
        }

        private Trail AddNextRoom(IMobileObject performer, HashSet<IRoom> searchedRooms, Directions.Direction direction, IExit exit, string targetKeyword, Queue<Trail> newTrails, Trail trail)
        {
            if (exit != null)
            {
                IRoom nextRoom = GlobalReference.GlobalValues.World.Zones[exit.Zone].Rooms[exit.Room];

                if (searchedRooms.Contains(nextRoom))
                {
                    return null;
                }

                //check to make sure the next room does not contain the attribute NoTrack
                //if so return null
                if (nextRoom.Attributes.Contains(RoomAttribute.NoTrack))
                {
                    searchedRooms.Add(nextRoom);  //don't bother to keep searching this room
                    return null;
                }

                trail.Distance++;
                IMobileObject foundMob = FindMobInRoom(nextRoom, targetKeyword);
                if (foundMob != null)
                {
                    return trail;
                }

                if (!searchedRooms.Contains(nextRoom))
                {
                    trail.Room = nextRoom;
                    newTrails.Enqueue(trail);
                    searchedRooms.Add(nextRoom);
                }
            }

            return null;
        }

        internal protected class Trail
        {
            [ExcludeFromCodeCoverage]
            internal protected Directions.Direction Direction { get; set; }
            [ExcludeFromCodeCoverage]
            internal protected int Distance { get; set; }
            [ExcludeFromCodeCoverage]
            internal protected IRoom Room { get; set; }
        }
    }
}
