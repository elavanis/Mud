using Objects.Mob.Interface;

namespace Objects.Material.Materials
{
    public class NpcInnateArmor : BaseMaterial
    {
        public NpcInnateArmor(INonPlayerCharacter npc)
        {
            Bludgeon = npc.Race.Bludgeon;
            Pierce = npc.Race.Pierce;
            Slash = npc.Race.Slash;

            Force = npc.Race.Force;
            Necrotic = npc.Race.Necrotic;
            Psychic = npc.Race.Psychic;
            Radiant = npc.Race.Radiant;
            Thunder = npc.Race.Thunder;

            Acid = npc.Race.Acid;
            Cold = npc.Race.Cold;
            Fire = npc.Race.Fire;
            Lightning = npc.Race.Lightning;
            Poison = npc.Race.Poison;
        }
    }
}
