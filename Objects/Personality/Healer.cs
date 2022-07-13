using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using System.Linq;

namespace Objects.Personality
{
    public class Healer : IHealer
    {
        public Healer()
        {

        }

        public Healer(int castPercent)
        {
            CastPercent = castPercent;
        }

        public int CastPercent { get; set; } = 1;

        public string? Process(INonPlayerCharacter npc, string? command)
        {
            if (command == null)
            {
                if (GlobalReference.GlobalValues.Random.PercentDiceRoll(CastPercent))
                {
                    if (npc.Room.PlayerCharacters.Count > 0)
                    {
                        int pcToCastOn = GlobalReference.GlobalValues.Random.Next(npc.Room.PlayerCharacters.Count);
                        IPlayerCharacter pc = npc.Room.PlayerCharacters[pcToCastOn];
                        int spellPos = GlobalReference.GlobalValues.Random.Next(npc.SpellBook.Count);
                        string spellName = npc.SpellBook.Keys.ToList()[spellPos];

                        command = $"cast {spellName} {pc.KeyWords[0]}";
                    }
                }
            }

            return command;
        }
    }
}
