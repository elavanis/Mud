using Objects.Item.Items.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Key : Item, IKey
    {
        public Key(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        [ExcludeFromCodeCoverage]
        public int ZoneNumber { get; set; }

        [ExcludeFromCodeCoverage]
        public int KeyNumber { get; set; }
    }
}
