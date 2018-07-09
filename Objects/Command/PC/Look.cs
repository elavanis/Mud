using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Global;
using static Objects.Mob.MobileObject;
using Objects.Interface;
using Shared.TagWrapper;
using static Shared.TagWrapper.TagWrapper;
using Objects.Item.Items.Interface;
using Objects.Item.Interface;

namespace Objects.Command.PC
{
    public class Look : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result(true, "(L)ook {Object To Look At}");

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "L", "Look" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            string message = null;

            if (performer.Position == CharacterPosition.Sleep)
            {
                return new Result(false, "You can not look while asleep.");
            }

            if (!GlobalReference.GlobalValues.CanMobDoSomething.SeeDueToLight(performer))
            {
                return new Result(false, "You can not see here because it is to dark.");
            }

            if (command.Parameters.Count > 0)
            {
                string target = command.Parameters[0].ParameterValue;
                int targetNumber = command.Parameters[0].ParameterNumber;
                IBaseObject foundItem = GlobalReference.GlobalValues.FindObjects.FindObjectOnPersonOrInRoom(performer, target, targetNumber);
                if (foundItem != null)
                {
                    if (foundItem as IPlayerCharacter != null)
                    {
                        message = BuildMobLookMessage(foundItem as IMobileObject, TagWrapper.TagType.PlayerCharacter);
                        return new Result(true, message);
                    }
                    else if (foundItem as INonPlayerCharacter != null)
                    {
                        message = BuildMobLookMessage(foundItem as IMobileObject, TagWrapper.TagType.NonPlayerCharacter);
                        return new Result(true, message);
                    }
                    else if (foundItem as IContainer != null)
                    {
                        IContainer container = foundItem as IContainer;
                        StringBuilder strBldr = new StringBuilder();
                        strBldr.AppendLine(foundItem.LookDescription);
                        if (container.Items.Count == 0)
                        {
                            strBldr.AppendLine("<Empty>");
                        }
                        else
                        {
                            foreach (IItem item in container.Items)
                            {
                                strBldr.AppendLine(item.ShortDescription);
                            }
                        }

                        message = GlobalReference.GlobalValues.TagWrapper.WrapInTag(strBldr.ToString().Trim(), TagWrapper.TagType.Item);
                        return new Result(true, message, null);
                    }
                    else
                    {
                        message = GlobalReference.GlobalValues.TagWrapper.WrapInTag(foundItem.LookDescription, TagWrapper.TagType.Item);
                        return new Result(true, message);
                    }
                }

                return new Result(false, "Unable to find anything that matches that description.");
            }
            else
            {
                return LookAtRoom(performer);
            }
        }

        private static string BuildMobLookMessage(IMobileObject mob, TagType wrapType)
        {
            StringBuilder masterMessage = new StringBuilder();
            masterMessage.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(mob.LookDescription, wrapType));


            StringBuilder strBldr = new StringBuilder();
            foreach (IEquipment equipment in mob.EquipedEquipment)
            {
                strBldr.AppendLine(equipment.ShortDescription);
            }
            if (strBldr.Length > 0)
            {
                masterMessage.Append(GlobalReference.GlobalValues.TagWrapper.WrapInTag(strBldr.ToString().Trim(), TagType.Item));
            }

            return masterMessage.ToString().Trim();
        }

        private static IResult LookAtRoom(IMobileObject performer)
        {
            IRoom room = performer.Room;
            StringBuilder masterBuilder = new StringBuilder();

            StringBuilder strBldr = new StringBuilder();

            IPlayerCharacter playerCharacter = performer as IPlayerCharacter;
            if (playerCharacter != null && playerCharacter.Debug)
            {
                strBldr.AppendLine(string.Format("[Zone{0} - Room{1}]", room.Zone, room.Id));
            }

            strBldr.AppendLine(string.Format("[{0}]", room.ShortDescription));
            strBldr.AppendLine(room.LookDescription);

            strBldr.Append("Exits:");

            if (room.North != null)
            {
                strBldr.Append("N ");
            }
            if (room.East != null)
            {
                strBldr.Append("E ");
            }
            if (room.South != null)
            {
                strBldr.Append("S ");
            }
            if (room.West != null)
            {
                strBldr.Append("W ");
            }
            if (room.Up != null)
            {
                strBldr.Append("U ");
            }
            if (room.Down != null)
            {
                strBldr.Append("D");
            }

            masterBuilder.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(strBldr.ToString().Trim(), TagType.Room));

            #region Items
            strBldr.Clear();
            foreach (IItem item in room.Items)
            {
                if (GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(performer, item))
                {
                    strBldr.AppendLine(item.ShortDescription);
                }
            }

            string line = strBldr.ToString().Trim();
            if (line.Length > 0)
            {
                masterBuilder.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(line, TagType.Item));
            }
            #endregion Items

            #region NPC
            strBldr.Clear();
            foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
            {
                if (GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(performer, npc))
                {
                    strBldr.AppendLine(npc.ShortDescription);
                }
            }

            line = strBldr.ToString().Trim();
            if (line.Length > 0)
            {
                masterBuilder.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(line, TagType.NonPlayerCharacter));
            }
            #endregion NPC

            #region PC
            strBldr.Clear();
            foreach (IPlayerCharacter pc in room.PlayerCharacters)
            {
                if (GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(performer, pc))
                {
                    strBldr.AppendLine(pc.ShortDescription);
                }
            }

            line = strBldr.ToString().Trim();
            if (line.Length > 0)
            {
                masterBuilder.AppendLine(GlobalReference.GlobalValues.TagWrapper.WrapInTag(line, TagType.PlayerCharacter));
            }

            string message = masterBuilder.ToString().Trim();
            #endregion PC
            return new Result(true, message, null);
        }
    }
}