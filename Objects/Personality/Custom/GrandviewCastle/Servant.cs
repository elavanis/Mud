using Objects.Global;
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
                        npc.EnqueueCommand("Say Right away Your Honorable Royal Majestic Graciousness.");
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
            else if (StateMachine == State.OnWayToKitchen)
            {
                if (npc.Room.Id == 19)
                {
                    List<INonPlayerCharacter> nonPlayerCharacters = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "cook");
                    if (nonPlayerCharacters.Count > 0)
                    {
                        StateMachine = State.AskCookForHasenpfeffer;
                        npc.EnqueueCommand("Say The king wants hasenpfeffer to eat.");
                    }
                    else
                    {
                        switch (Step)
                        {
                            case 1:
                                npc.EnqueueCommand("Say Hello?");
                                break;
                            case 5:
                                npc.EnqueueCommand("Say Is there anyone here?");
                                break;
                            case 10:
                                npc.EnqueueCommand("Say Great how am I going to make hasenpfeffer?");
                                break;
                            case 15:
                                npc.EnqueueCommand("Emote scurries around the kitchen looking for something to give the king.");
                                break;
                            case 20:
                                npc.EnqueueCommand("Emote scurries around the kitchen looking for something to give the king.");
                                break;
                            case 25:
                                npc.EnqueueCommand("Say Ah Ha!");
                                break;
                            case 30:
                                npc.EnqueueCommand("Say This carrot will work.");
                                break;
                            case 35:
                                npc.EnqueueCommand("Wait");
                                npc.EnqueueCommand("South");
                                npc.EnqueueCommand("Wait");
                                npc.EnqueueCommand("Up");
                                npc.EnqueueCommand("Wait");
                                npc.EnqueueCommand("West");
                                StateMachine = State.GiveToKing;
                                break;

                        }

                    }
                }
            }
            else if (StateMachine == State.AskCookForHasenpfeffer)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>cook says here you go. Hasenpfeffer for the king.</Communication>")
                    {
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("South");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Up");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("West");
                        StateMachine = State.GiveToKing;
                    }
                }
            }
            else if (StateMachine == State.GiveToKing)
            {
                if (npc.Room.Id == 21
                    && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "king").Count > 0)
                {
                    npc.EnqueueCommand("Your hasenpfeffer Your Magisty.");
                    StateMachine = State.Wait;
                }
            }

            //give carrots

            return null;
        }

        private enum State
        {
            Wait,
            AskedWhatWanted,
            KingToldHasenpfeffer,
            OnWayToKitchen,
            AskCookForHasenpfeffer,
            TakeBackToKing,
            PanicAndMakeCarrot,
            GiveToKing
        }
    }
}
