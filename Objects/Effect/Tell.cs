using Objects.Effect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Zone.Interface;
using Objects.Global;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    public class Tell : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        //Because we can not put a reference to the npc who will be telling 
        //we need to scan through the zone to find them.  We iterate over 
        //each room in the zone and check the npc to see if the id matches
        //the one we passed in via the parameter
        public void ProcessEffect(IEffectParameter parameter)
        {
            IZone zone = GlobalReference.GlobalValues.World.Zones[parameter.ObjectId.Zone];
            foreach (IRoom room in zone.Rooms.Values)
            {
                foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
                {
                    if (npc.Id == parameter.ObjectId.Id)
                    {
                        npc.EnqueueCommand(string.Format("Tell {0} {1}", parameter.Target.KeyWords[0], parameter.Message.GetTranslatedMessage(null)));
                        break;
                    }
                }
            }
        }
    }
}
