﻿using Objects.Effect.Interface;
using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.Sound.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect.Zone.GrandViewGarden
{
    public class MoveToOtherDimension : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            INonPlayerCharacter npc = parameter.Target as INonPlayerCharacter;
            IPlayerCharacter pc = parameter.Target as IPlayerCharacter;
            if (parameter.Target is IMobileObject mob)
            {
                IRoom oldRoom = mob.Room;

                //only process if they are in the garden and in the normal dimension
                if (oldRoom.ZoneId == 11 && oldRoom.Id < 10)
                {
                    IRoom newRoom = GlobalReference.GlobalValues.World.Zones[mob.Room.ZoneId].Rooms[mob.Room.Id + 10];

                    oldRoom.RemoveMobileObjectFromRoom(mob);

                    string capitalSentenceDescription = GlobalReference.GlobalValues.StringManipulator.CapitalizeFirstLetter(mob.SentenceDescription);
                    string message = $"{capitalSentenceDescription} picked up the rose and then disappeared.";
                    ITranslationMessage translationMessage = new TranslationMessage(message);
                    GlobalReference.GlobalValues.Notify.Room(parameter.Performer, null, oldRoom, translationMessage, new List<IMobileObject>() { mob }, true, false);

                    message = $"{capitalSentenceDescription} suddenly appears from thin air holding a rose.";
                    translationMessage = new TranslationMessage(message);
                    GlobalReference.GlobalValues.Notify.Room(parameter.Performer, null, newRoom, translationMessage, new List<IMobileObject>() { mob }, true, false);

                    newRoom.AddMobileObjectToRoom(mob);
                    mob.Room = newRoom;
                }
            }
        }
    }
}
