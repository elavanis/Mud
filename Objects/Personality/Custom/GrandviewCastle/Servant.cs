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
            if (command != null)
            {
                return command;
            }

            if (npc.IsInCombat)
            {
                return null;
            }

            Step++;

            if (StateMachine == State.Wait)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>King says Servant, bring me my meal.</Communication>")
                    {
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Emote bows.");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Say Your Honorable Majestic Majesty Graciousness, what would you like to eat?");
                        StateMachine = State.AskedWhatWanted;
                    }
                }
            }
            else if (StateMachine == State.AskedWhatWanted)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>King says Bring me hasenpfeffer.</Communication>")
                    {
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Say Right away Your Honorable Royal Majestic Graciousness.");
                        npc.EnqueueCommand("Wait");
                        StateMachine = State.KingToldHasenpfeffer;
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
                        return "Say The King wants hasenpfeffer to eat.";
                    }
                    else
                    {
                        StateMachine = State.EmptyKitchen;
                        Step = 0;
                    }
                }
            }
            else if (StateMachine == State.EmptyKitchen)
            {
                switch (Step)
                {
                    case 1:
                        return "Say Hello?";
                    case 5:
                        return "Say Is there anyone here?";
                    case 10:
                        return "Say Great how am I going to make hasenpfeffer?";
                    case 15:
                        return "Emote scurries around the kitchen looking for something to give the King.";
                    case 20:
                        return "Emote scurries around the kitchen looking for something to give the King.";
                    case 25:
                        return "Say Ah Ha!";
                    case 30:
                        return "Say This carrot will work.";
                    case 35:
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("South");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Up");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("West");
                        StateMachine = State.GiveToKingCarrot;
                        break;

                }
            }
            else if (StateMachine == State.AskCookForHasenpfeffer)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>cook says here you go. Hasenpfeffer for the King.</Communication>")
                    {
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("South");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("Up");
                        npc.EnqueueCommand("Wait");
                        npc.EnqueueCommand("West");
                        StateMachine = State.GiveToKingHasenpfeffer;
                    }
                }
            }
            else if (StateMachine == State.GiveToKingHasenpfeffer)
            {
                if (npc.Room.Id == 21
                    && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "King").Count > 0)
                {
                    npc.EnqueueCommand("Bon appetit Most Gracious Majesty.");
                    StateMachine = State.Wait;
                }
            }
            else if (StateMachine == State.GiveToKingCarrot)
            {
                if (npc.Room.Id == 21
                    && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "King").Count > 0)
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
            EmptyKitchen,
            TakeBackToKing,
            PanicAndMakeCarrot,
            GiveToKingHasenpfeffer,
            GiveToKingCarrot

        }
    }
}
