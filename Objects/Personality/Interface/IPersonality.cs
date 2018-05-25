using Objects.Mob;
using Objects.Mob.Interface;

namespace Objects.Personality.Interface
{
    public interface IPersonality
    {
        string Process(INonPlayerCharacter npc, string command);
    }
}