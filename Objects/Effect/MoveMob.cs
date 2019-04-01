using Objects.Effect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Global;
using System.Threading;

namespace Objects.Effect
{
    public class MoveMob : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        [ExcludeFromCodeCoverage]
        public MoveMob()
        {

        }

        public MoveMob(ISound sound)
        {
            Sound = sound;
        }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IMobileObject mob)
            {
                try
                {
                    IRoom newRoom = GlobalReference.GlobalValues.World.Zones[parameter.RoomId.Zone].Rooms[parameter.RoomId.Id];
                    IRoom oldRoom = mob.Room;

                    MoveMobToNewRoom(mob, newRoom, oldRoom);
                }
                catch
                {
                    GlobalReference.GlobalValues.Logger.Log(mob, Global.Logging.LogSettings.LogLevel.ERROR, string.Format("Mob tried to move to Zone {0} Room {1} but failed.", parameter.RoomId.Zone, parameter.RoomId.Id));
                }
            }
        }

        private static void MoveMobToNewRoom(IMobileObject mob, IRoom newRoom, IRoom oldRoom)
        {
            oldRoom.RemoveMobileObjectFromRoom(mob);
            newRoom.AddMobileObjectToRoom(mob);
            mob.Room = newRoom;

            mob.EnqueueCommand("Look");
        }
    }
}
