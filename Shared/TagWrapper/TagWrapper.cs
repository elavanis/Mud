using Shared.TagWrapper.Interface;

namespace Shared.TagWrapper
{
    public class TagWrapper : ITagWrapper
    {
        public string WrapInTag(string stringToWrapp, TagType typeOfTag = TagType.Info)
        {
            //check to see if the string is empty
            if (!string.IsNullOrWhiteSpace(stringToWrapp))
            {
                return string.Format("<{0}>{1}</{0}>", typeOfTag.ToString(), stringToWrapp);
            }
            else
            {
                return null;
            }
        }

        public enum TagType
        {
            AsciiArt,
            ClientCommand,
            Communication,
            Connection,
            Data,
            DamageDelt,
            DamageReceived,
            Exception,
            File,
            Info,
            Item,
            Map,
            NonPlayerCharacter,
            PlayerCharacter,
            Room,
            Health,
            Mana,
            Sound,
            Stamina
        }
    }
}
