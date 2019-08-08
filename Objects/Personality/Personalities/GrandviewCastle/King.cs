using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Personality.Personalities.GrandviewCastle
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
                        return "Emote The king stretches and yawns loudly.";
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
                        return "Emote The brushes his teeth.";
                    case 3:
                        return "Emote Uses the bathroom.";
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
                    StateMachine = State.CalledForFood;
                    Step = 0;

                    npc.EnqueueCommand("Say Servant, bring me my meal.");
                    npc.EnqueueCommand("");
                    npc.EnqueueCommand("");
                    npc.EnqueueCommand("");
                    npc.EnqueueCommand("");
                    npc.EnqueueCommand("");
                    npc.EnqueueCommand("Say Bring me hasenpfeffer.");

                    return null;
                }
            }
            else if (StateMachine == State.CalledForFood)
            {

            }


            return null;
        }
        private string NightTimeThings(INonPlayerCharacter npc)
        {
            return null;
        }


        private enum State
        {
            Sleep,
            MoveToBathRoom,
            InBathRoom,
            ThroneRoom,
            CalledForFood,


        }
    }
}
