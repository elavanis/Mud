namespace Objects.Material.Materials
{
    public class Gold : BaseMaterial
    {
        public Gold()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Strong();
            Necrotic = Strong();
            Psychic = Strong();
            Radiant = Strong();
            Thunder = Strong();

            Acid = Strong();
            Cold = Weak();
            Fire = Weak();
            Lightning = Weak();
            Poison = Moderate();
        }
    }
}
