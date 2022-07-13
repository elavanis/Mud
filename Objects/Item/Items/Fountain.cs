using Objects.Item.Items.Interface;

namespace Objects.Item.Items
{
    public class Fountain : Item, IFountain
    {
        public Fountain(string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
            Attributes.Add(ItemAttribute.NoGet);
            KeyWords.Add("Fountain");
        }
    }
}
