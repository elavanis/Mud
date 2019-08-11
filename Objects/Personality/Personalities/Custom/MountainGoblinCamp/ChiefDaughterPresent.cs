using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.Custom.MountainGoblinCamp
{
    public class ChiefDaughterPresent : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                foreach (INonPlayerCharacter otherNpc in npc.Room.NonPlayerCharacters)
                {
                    if (otherNpc.Zone == 23 && otherNpc.Id == 10)  //chief daughter info
                    {
                        if (otherNpc.FollowTarget != null) //don't keep giving the reward over and over
                        {
                            npc.EnqueueCommand("say You have brought my daughter back to me.  I am most grateful.");
                            npc.EnqueueCommand("say Please accept this gift as a reward.");


                            //generate a reward;
                            int level = GlobalReference.GlobalValues.Random.Next(18, 22);
                            int bonus = GlobalReference.GlobalValues.Random.Next(4);
                            IEquipment equipment = GlobalReference.GlobalValues.RandomDropGenerator.GenerateRandomEquipment(level, level + bonus);

                            npc.EnqueueCommand($"say Servant, fetch me my best {equipment.KeyWords[0]} for our hero.");
                            npc.EnqueueCommand("");
                            npc.EnqueueCommand("");

                            npc.Items.Add(equipment);
                            npc.EnqueueCommand($"say Please accept this {equipment.KeyWords[0]} as a reward for rescuing my daughter.");
                            npc.EnqueueCommand($"Give {equipment.KeyWords[0]} {otherNpc.FollowTarget.KeyWords[0]}");

                            otherNpc.FollowTarget = null; //make daughter stop flowing
                        }
                    }
                }
            }

            return command;
        }
    }
}
