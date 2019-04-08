namespace Objects.Material.Materials
{
    public class Bone : BaseMaterial
    {
        public Bone()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Weak();
            Necrotic = Weak();
            Psychic = Weak();
            Radiant = Strong();
            Thunder = Strong();

            Acid = Weak();
            Cold = Strong();
            Fire = Strong();
            Lightning = Strong();
            Poison = Weak();
        }
    }
}
