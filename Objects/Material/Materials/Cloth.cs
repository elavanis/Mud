namespace Objects.Material.Materials
{
    public class Cloth : BaseMaterial
    {
        public Cloth()
        {
            Bludgeon = Weak();
            Pierce = Weak();
            Slash = Weak();

            Force = Weak();
            Necrotic = Weak();
            Psychic = Weak();
            Radiant = Weak();
            Thunder = Weak();

            Acid = Weak();
            Cold = Strong();
            Fire = Weak();
            Lightning = Moderate();
            Poison = Weak();
        }
    }
}
