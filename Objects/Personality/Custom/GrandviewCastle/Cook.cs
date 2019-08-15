using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Custom.GrandviewCastle
{
    public class Cook : IPersonality
    {
        private State StateMachine { get; set; } = State.Wait;
        private int Step;

        public string Process(INonPlayerCharacter npc, string command)
        {
            Step++;

            switch (StateMachine)
            {
                case State.Wait:
                    CheckForTrigger(npc);
                    break;
                case State.AskedForHasenpfeffer:
                    PerformSteps(npc);
                    break;


            }

            return null;
        }

        private void PerformSteps(INonPlayerCharacter npc)
        {
            List<INonPlayerCharacter> nonPlayerCharacters = new List<INonPlayerCharacter>(npc.Room.NonPlayerCharacters);
            for (int i = 0; i < nonPlayerCharacters.Count; i++)
            {
                if (nonPlayerCharacters[i] == npc)
                {
                    switch (i)
                    {
                        case 0:
                            switch (Step)
                            {
                                case 3:
                                    npc.EnqueueCommand("Emote begins to cut up a rabbit.");
                                    break;
                                case 8:
                                    npc.EnqueueCommand("Emote puts the rabbit in the pot.");
                                    StateMachine = State.Wait;
                                    break;
                            }
                            break;
                        case 1:
                            switch (Step)
                            {
                                case 3:
                                    npc.EnqueueCommand("Emote begins to cut up a potatoes.");
                                    break;
                                case 6:
                                    npc.EnqueueCommand("Emote puts the potatoes in the pot.");
                                    StateMachine = State.Wait;
                                    break;
                            }
                            break;
                        case 2:
                            switch (Step)
                            {
                                case 3:
                                    npc.EnqueueCommand("Emote begins to cut up a carrots");
                                    break;
                                case 7:
                                    npc.EnqueueCommand("Emote puts the carrots in the pot.");
                                    StateMachine = State.Wait;
                                    break;
                            }
                            break;
                        case 3:
                            switch (Step)
                            {
                                case 3:
                                    npc.EnqueueCommand("Emote stokes the cooking fire.");
                                    break;
                                case 5:
                                case 9:
                                case 13:
                                case 17:
                                    npc.EnqueueCommand("Emote stirs the pot.");
                                    break;
                                case 19:
                                    npc.EnqueueCommand("Say here you go.  Hasenpfeffer for the king.");
                                    StateMachine = State.Wait;
                                    break;
                            }
                            break;
                    }
                    break;
                }
            }
        }

        private void CheckForTrigger(INonPlayerCharacter npc)
        {
            string message = null;
            while ((message = npc.DequeueMessage()) != null)
            {
                if (message == "<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>")
                {
                    List<INonPlayerCharacter> nonPlayerCharacters = new List<INonPlayerCharacter>(npc.Room.NonPlayerCharacters);
                    for (int i = 0; i < nonPlayerCharacters.Count; i++)
                    {
                        if (nonPlayerCharacters[i] == npc)
                        {
                            switch (i)
                            {
                                case 0:
                                    npc.EnqueueCommand("Say Right away");
                                    break;
                                case 1:
                                    npc.EnqueueCommand("Say As the king wishes.");
                                    break;
                                case 2:
                                    npc.EnqueueCommand("Say Sure.");
                                    break;
                                case 3:
                                    npc.EnqueueCommand("Say As the king wishes.");
                                    break;
                            }
                            break;
                        }
                    }

                    StateMachine = State.AskedForHasenpfeffer;
                    Step = 0;
                }
            }
        }

        private static string NewMethod(INonPlayerCharacter npc)
        {
            string message;
            while ((message = npc.DequeueMessage()) != null)
            {
                if (message == "<Communication>kings servant says The king wants hasenpfeffer to eat.</Communication>")
                {

                    List<INonPlayerCharacter> nonPlayerCharacters = new List<INonPlayerCharacter>(npc.Room.NonPlayerCharacters);
                    for (int i = 0; i < nonPlayerCharacters.Count; i++)
                    {
                        if (nonPlayerCharacters[i] == npc)
                        {
                            switch (i)
                            {
                                case 0:
                                    npc.EnqueueCommand("Say Right away");
                                    EnqueueWait(npc, 2);
                                    npc.EnqueueCommand("Emote begins to cut up a rabbit.");
                                    EnqueueWait(npc, 5);
                                    npc.EnqueueCommand("Emote puts the rabbit in the pot.");
                                    break;
                                case 1:
                                    npc.EnqueueCommand("Say As the king wishes.");
                                    EnqueueWait(npc, 2);
                                    npc.EnqueueCommand("Emote begins to cut up a potatoes.");
                                    EnqueueWait(npc, 3);
                                    npc.EnqueueCommand("Emote puts the potatoes in the pot.");
                                    break;
                                case 2:
                                    npc.EnqueueCommand("Say Sure.");
                                    EnqueueWait(npc, 2);
                                    npc.EnqueueCommand("Emote begins to cut up a carrots.");
                                    EnqueueWait(npc, 4);
                                    npc.EnqueueCommand("Emote puts the carrots in the pot.");
                                    break;
                                case 3:
                                    npc.EnqueueCommand("Say As the king wishes.");
                                    EnqueueWait(npc, 2);
                                    npc.EnqueueCommand("Emote stokes the cooking fire.");
                                    EnqueueWait(npc, 4);
                                    npc.EnqueueCommand("Emote stirs the pot.");
                                    EnqueueWait(npc, 4);
                                    npc.EnqueueCommand("Emote stirs the pot.");
                                    EnqueueWait(npc, 4);
                                    npc.EnqueueCommand("Emote stirs the pot.");
                                    EnqueueWait(npc, 4);
                                    npc.EnqueueCommand("Emote stirs the pot.");
                                    EnqueueWait(npc, 2);
                                    npc.EnqueueCommand("Say here you go.  Hasenpfeffer for the king.");
                                    break;
                            }

                            break;
                        }
                    }
                }
            }

            return message;
        }

        private static void EnqueueWait(INonPlayerCharacter npc, int times)
        {
            for (int i = 0; i < times; i++)
            {
                npc.EnqueueCommand("Wait");

            }
        }

        private enum State
        {
            Wait,
            AskedForHasenpfeffer
        }
    }
}
