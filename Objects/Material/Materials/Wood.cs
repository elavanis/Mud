using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Material.Materials
{
    public class Wood : BaseMaterial
    {
        public Wood()
        {
            Bludgeon = Moderate();
            Pierce = Moderate();
            Slash = Moderate();

            Force = Weak();
            Necrotic = Weak();
            Psychic = Strong();
            Radiant = Strong();
            Thunder = Strong();

            Acid = Weak();
            Cold = Moderate();
            Fire = Weak();
            Lightning = Weak();
            Poison = Moderate();
        }
    }
}
