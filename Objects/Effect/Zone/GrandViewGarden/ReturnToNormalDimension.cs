using Objects.Effect.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Effect.Zone.GrandViewGarden
{
    public class ReturnToNormalDimension : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            IMobileObject mob = parameter.Target as IMobileObject;
            INonPlayerCharacter npc = parameter.Target as INonPlayerCharacter;
            IPlayerCharacter pc = parameter.Target as IPlayerCharacter;
            if (mob != null)
            {
                IRoom oldRoom = mob.Room;

                //only process if they are in the garden and in the other dimension
                if (oldRoom.Zone == 11 && oldRoom.Id > 10)
                {
                    IRoom newRoom = GlobalReference.GlobalValues.World.Zones[mob.Room.Zone].Rooms[mob.Room.Id - 10];

                    if (npc != null)
                    {
                        oldRoom.RemoveMobileObjectFromRoom(npc);
                    }
                    else
                    {
                        oldRoom.RemoveMobileObjectFromRoom(pc);
                    }

                    string message = $"{mob.SentenceDescription} drops a rose and then disappears.";
                    ITranslationMessage translationMessage = new TranslationMessage(message);
                    GlobalReference.GlobalValues.Notify.Room(parameter.Performer, null, oldRoom, translationMessage, new List<IMobileObject>() { mob }, true, false);
                    GlobalReference.GlobalValues.Notify.Room(parameter.Performer, null, newRoom, translationMessage, new List<IMobileObject>() { mob }, true, false);

                    if (npc != null)
                    {
                        newRoom.AddMobileObjectToRoom(npc);
                    }
                    else
                    {
                        newRoom.AddMobileObjectToRoom(pc);
                    }
                    mob.Room = newRoom;
                }
            }
        }
    }
}
