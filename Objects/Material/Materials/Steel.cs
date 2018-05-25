namespace Objects.Material.Materials
{
    public class Steel : BaseMaterial
    {
        public Steel()
        {
            Bludgeon = Strong();
            Pierce = Strong();
            Slash = Strong();

            Force = Weak();
            Necrotic = Weak();
            Psychic = Weak();
            Radiant = Weak();
            Thunder = Weak();

            Acid = Strong();
            Cold = Weak();
            Fire = Weak();
            Lightning = Weak();
            Poison = Moderate();
        }
    }
}
