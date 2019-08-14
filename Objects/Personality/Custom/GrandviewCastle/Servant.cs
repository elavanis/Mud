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

            //honorable royal majestic graciousness
            //honorable majestic magisty graciousness

            //give carrots

            return "Wait";
        }

        private enum State
        {
            Wait,
            KingAskedForFood,
            AskedWhatWanted,
            KingToldHasenpfeffer,
            AskCookForHasenpfeffer,
            TakeBackToKing,
            PanicAndMakeCarrot



        }
    }
}
