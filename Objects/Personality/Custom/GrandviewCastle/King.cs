using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Custom.GrandviewCastle
{
    public class King : IPersonality
    {
        private State StateMachine { get; set; } = State.Sleep;
        private int Step;
        private bool GreetQueen { get; set; }
        private List<string> GreetingForQueen = new List<string>() { "Good morning honey.", "Good morning buttercup.", "I hope you slept well.", "Good morning sunshine.", "You look as lovely as the first time I met you.", "Hello my desert lily." };

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command != null)
            {
                return command;
            }

            int hour = GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour;

            if (!GreetQueen && npc.Room.Id == 21)
            {
                if (GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "queen").Count > 1)
                {
                    GreetQueen = true;
                    return GreetingForQueen[GlobalReference.GlobalValues.Random.Next(GreetingForQueen.Count)];
                }
            }

            if (hour < 13)
            {
                return DayTimeThings(npc);
            }
            else
            {
                //return NightTimeThings(npc);
                return DayTimeThings(npc);
            }
        }


        private string DayTimeThings(INonPlayerCharacter npc)
        {
            Step++;

            if (StateMachine == State.Sleep)
            {
                switch (Step)
                {
                    case 1:
                        return "Emote stretches and yawns loudly.";
                    case 3:
                        return "Say Good morning world.";
                    case 4:
                        StateMachine = State.MoveToBathRoom;
                        Step = 0;
                        return "Stand";
                }
            }
            else if (StateMachine == State.MoveToBathRoom)
            {
                if (npc.Room.Id == 22)
                {
                    return "South";
                }
                else if (npc.Room.Id == 24)
                {
                    StateMachine = State.InBathRoom;
                    Step = 0;
                }
            }
            else if (StateMachine == State.InBathRoom)
            {
                switch (Step)
                {
                    case 1:
                        return "Emote brushes his teeth.";
                    case 3:
                        return "Emote uses the bathroom.";
                    case 5:
                        return "North";
                    case 6:
                        StateMachine = State.ThroneRoom;
                        return "East";
                }
            }
            else if (StateMachine == State.ThroneRoom)
            {
                if (Step % 5 == 0
                    && GlobalReference.GlobalValues.Random.PercentDiceRoll(50)
                    && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "servant").Count > 0)
                {
                    StateMachine = State.AskedForMeal;
                    Step = 0;
                    npc.EnqueueCommand("Say Servant, bring me my meal.");
                }
            }
            else if (StateMachine == State.AskedForMeal)
            {
                if (Step % 5 == 0
                   && GlobalReference.GlobalValues.Random.PercentDiceRoll(50)
                   && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "servant").Count > 0)
                {
                    bool foundAskedForWhat = false;
                    string message = null;
                    while ((message = npc.DequeueMessage()) != null)
                    {
                        if (message == "<Communication>kings servant says Your Honorable Majestic Majesty Graciousness, what would you like to eat?</Communication>")
                        {
                            foundAskedForWhat = true;
                            break;
                        }
                    }

                    if (foundAskedForWhat)
                    {
                        StateMachine = State.AskedForHasenpfeffer;
                        Step = 0;
                    }

                    return null;
                }
            }

            else if (StateMachine == State.AskedForHasenpfeffer)
            {
                switch (Step)
                {
                    case 1:
                        return "Say Bring me hasenpfeffer.";
                    case 20:
                        return "Say Where is my hasenpfeffer?";
                    case 30:
                        return "Say Guards bring me my servant!!!";
                }
            }

            return null;
        }

        private string NightTimeThings(INonPlayerCharacter npc)
        {
            if (npc.Room.Id == 20)
            {
                npc.EnqueueCommand("Say Court is closed for the day. Please leave.");
                return "West";
            }

            return "Wait";
        }


        private enum State
        {
            Sleep,
            MoveToBathRoom,
            InBathRoom,
            ThroneRoom,
            AskedForMeal,
            AskedForHasenpfeffer,



        }
    }
}
