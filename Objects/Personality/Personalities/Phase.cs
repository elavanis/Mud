using Objects.Global;
using Objects.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Personalities.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Personality.Personalities
{
    public class Phase : IPhase
    {
        [ExcludeFromCodeCoverage]
        public int PhasePercent { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public List<IBaseObjectId> RoomsToPhaseTo { get; set; } = new List<IBaseObjectId>();

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                if (GlobalReference.GlobalValues.Random.PercentDiceRoll(PhasePercent))
                {
                    if (!npc.IsInCombat) //this is most expensive so do it last
                    {
                        IBaseObjectId baseObjectId = RoomsToPhaseTo[GlobalReference.GlobalValues.Random.Next(RoomsToPhaseTo.Count)];

                        string newCommand = $"Goto {baseObjectId.Zone} {baseObjectId.Id}";
                        return newCommand;
                    }
                }
            }

            return command;
        }
    }
}
