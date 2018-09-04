using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Mob.Interface;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Room.Interface;
using Objects.Global;
using Objects.Zone.Interface;
using System.Threading;
using Objects.Language;
using Objects.Language.Interface;

namespace Objects.Command.God
{
    public class Goto : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Goto [ZoneId] [RoomId] \r\nGoto [PlayerName]", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Goto" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            if (command.Parameters.Count >= 2)
            {
                int zoneId;
                int roomId;
                if (int.TryParse(command.Parameters[0].ParameterValue, out zoneId)
                    && int.TryParse(command.Parameters[1].ParameterValue, out roomId))
                {
                    try
                    {
                        IRoom newRoom = GlobalReference.GlobalValues.World.Zones[zoneId].Rooms[roomId];

                        return MoveToRoom(performer, newRoom);
                    }
                    catch
                    {
                        return new Result(string.Format("Unable to find zone {0} room {1}.", zoneId, roomId), true);
                    }
                }
                else
                {
                    return new Result("Goto [ZoneId] [RoomId]", true);
                }
            }
            else
            {
                return new Result("Where would you like to goto?", true);
            }
        }

        private static IResult MoveToRoom(IMobileObject performer, IRoom newRoom)
        {
            IPlayerCharacter god = performer as IPlayerCharacter;
            if (god != null)
            {
                #region Leave
                if (god.God && !string.IsNullOrWhiteSpace(god.GotoLeaveMessage))
                {
                    ITranslationMessage translationMessage = new TranslationMessage(god.GotoLeaveMessage);
                    GlobalReference.GlobalValues.Notify.Room(performer, null, god.Room, translationMessage, null, true, false);
                }
                #endregion Leave    

                #region Enter
                if (god.God && !string.IsNullOrWhiteSpace(god.GotoEnterMessage))
                {
                    ITranslationMessage translationMessage = new TranslationMessage(god.GotoEnterMessage);
                    GlobalReference.GlobalValues.Notify.Room(performer, null, god.Room, translationMessage, null, true, false);
                }
                #endregion Enter
            }

            performer.Room.RemoveMobileObjectFromRoom(performer);
            newRoom.AddMobileObjectToRoom(performer);
            performer.Room = newRoom;

            return GlobalReference.GlobalValues.CommandList.PcCommandsLookup["LOOK"].PerformCommand(performer, new Command());
        }
    }
}
