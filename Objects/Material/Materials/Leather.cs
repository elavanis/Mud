namespace Objects.Material.Materials
{
    public class Leather : BaseMaterial
    {
        public Leather()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Weak();
            Necrotic = Weak();
            Psychic = Weak();
            Radiant = Weak();
            Thunder = Weak();

            Acid = Moderate();
            Cold = Moderate();
            Fire = Moderate();
            Lightning = Strong();
            Poison = Moderate();
        }
    }
}
