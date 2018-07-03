using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Material.Materials
{
    public class AllModerate : BaseMaterial
    {
        public AllModerate()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Moderate();
            Necrotic = Moderate();
            Psychic = Moderate();
            Radiant = Moderate();
            Thunder = Moderate();

            Acid = Moderate();
            Cold = Moderate();
            Fire = Moderate();
            Lightning = Moderate();
            Poison = Moderate();
        }
    }
}