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
        private int cookId;

        public Cook(int id)
        {
            cookId = id;
        }

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                CheckForTrigger(npc);
                return null;
            }
            else
            {
                return command;
            }
        }


        private string CheckForTrigger(INonPlayerCharacter npc)
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
                            switch (cookId)
                            {
                                case 0:
                                    npc.EnqueueCommand("Say Right away.");
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
    }
}
