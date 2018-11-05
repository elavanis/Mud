using Objects.Global.Notify.Interface;
using Objects.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.Notify
{
    public class Notify : INotify
    {
        public void Zone(IMobileObject performer, IBaseObject target, IZone zone, ITranslationMessage message, List<IMobileObject> excludedMobs = null, bool requiredToSee = false, bool requiredToHear = false)
        {
            foreach (IRoom room in zone.Rooms.Values)
            {
                Room(performer, target, room, message, excludedMobs, requiredToSee, requiredToHear);
            }
        }

        public void Room(IMobileObject performer, IBaseObject target, IRoom room, ITranslationMessage message, List<IMobileObject> excludedMobs = null, bool requiredToSee = false, bool requiredToHear = false)
        {
            foreach (INonPlayerCharacter npc in room.NonPlayerCharacters)
            {
                AddMessage(performer, target, npc, message, excludedMobs, requiredToSee, requiredToHear);
            }

            foreach (IPlayerCharacter pc in room.PlayerCharacters)
            {
                AddMessage(performer, target, pc, message, excludedMobs, requiredToSee, requiredToHear);
            }
        }

        public void Mob(IMobileObject performer, IBaseObject target, IMobileObject notifie, ITranslationMessage message, bool requiredToSee = false, bool requiredToHear = false)
        {
            AddMessage(performer, target, notifie, message, null, requiredToSee, requiredToHear);
        }

        public void Mob(IMobileObject notifie, ITranslationMessage message)
        {
            notifie.EnqueueMessage(message.GetTranslatedMessage(notifie));
        }

        private void AddMessage(IMobileObject performer, IBaseObject target, IMobileObject notifie, ITranslationMessage message, List<IMobileObject> excludedMobs = null, bool requiredToSee = false, bool requiredToHear = false)
        {
            if (excludedMobs == null || !excludedMobs.Contains(notifie))
            {
                string performerSentence = "unknown";
                string targetSentence = "unknown";

                if ((!requiredToSee || GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(notifie, performer))
                    && (!requiredToHear || GlobalReference.GlobalValues.CanMobDoSomething.Hear(notifie, performer)))
                {
                    if (performer?.SentenceDescription != null)
                    {
                        performerSentence = performer.SentenceDescription;
                    }
                }

                if ((!requiredToSee || GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(notifie, target))
                   && (!requiredToHear || GlobalReference.GlobalValues.CanMobDoSomething.Hear(notifie, target)))
                {
                    if (target?.SentenceDescription != null)
                    {
                        targetSentence = target.SentenceDescription;
                    }
                }

                notifie.EnqueueMessage(GlobalReference.GlobalValues.StringManipulator.UpdateTargetPerformer(performerSentence, targetSentence, message.GetTranslatedMessage(notifie)));


                //if (performerSentence = "unknown" && targetSentence = "unknown")
                //{
                //    //don't do anything the notifie didn't see or hear anything
                //}
                //else
                //{
                //    notifie.EnqueueMessage(GlobalReference.GlobalValues.Notify.UpdateTargetPerformer(performerSentence, targetSentence, message.GetTranslatedMessage(notifie)));
                //}

            }
        }

        //private string UpdateTargetPerformer(string performer, string target, string message)
        //{
        //    List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
        //    keyValuePairs.Add(new KeyValuePair<string, string>("{performer}", performer));
        //    keyValuePairs.Add(new KeyValuePair<string, string>("{target}", target));
        //    string updatedMessage = GlobalReference.GlobalValues.StringManipulator.Manipulate(keyValuePairs, message);

        //    return updatedMessage;
        //}
    }
}
