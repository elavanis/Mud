using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Custom.GrandviewCastle
{
    public class Servant : IPersonality
    {
        private State StateMachine { get; set; } = State.Wait;
        private int Step;

        public string Process(INonPlayerCharacter npc, string command)
        {
            Step++;

            if (StateMachine == State.Wait)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>king says Servant, bring me my meal.</Communication>")
                    {
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("Emote bows.");
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("Say Your Honorable Majestic Majesty Graciousness, what would you like to eat?");
                        StateMachine = State.AskedWhatWanted;
                        Step = 0;
                    }
                }
            }
            else if (StateMachine == State.AskedWhatWanted)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>king says Bring me hasenpfeffer.</Communication>")
                    {
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("wait");
                        npc.EnqueueCommand("Say Right away your Honorable Royal Majestic Graciousness.");
                        npc.EnqueueCommand("wait");
                        StateMachine = State.KingToldHasenpfeffer;
                        Step = 0;
                    }
                }
            }
            else if (StateMachine == State.KingToldHasenpfeffer)
            {
                npc.EnqueueCommand("East");
                npc.EnqueueCommand("Wait");
                npc.EnqueueCommand("Down");
                npc.EnqueueCommand("Wait");
                npc.EnqueueCommand("North");
                npc.EnqueueCommand("Wait");
                StateMachine = State.OnWayToKitchen;
            }


            //give carrots

            return "Wait";
        }

        private enum State
        {
            Wait,
            AskedWhatWanted,
            KingToldHasenpfeffer,
            OnWayToKitchen,
            AskCookForHasenpfeffer,
            TakeBackToKing,
            PanicAndMakeCarrot



        }
    }
}
