namespace Objects.Material.Materials
{
    public class Silver : BaseMaterial
    {
        public Silver()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Moderate();
            Necrotic = Moderate();
            Psychic = Moderate();
            Radiant = Moderate();
            Thunder = Moderate();

            Acid = Strong();
            Cold = Weak();
            Fire = Weak();
            Lightning = Weak();
            Poison = Strong();
        }
    }
}
