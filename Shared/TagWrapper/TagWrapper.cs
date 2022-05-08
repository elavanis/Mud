using Shared.TagWrapper.Interface;

namespace Shared.TagWrapper
{
    public class TagWrapper : ITagWrapper
    {
        public string WrapInTag(string stringToWrap, TagType typeOfTag = TagType.Info)
        {
            ////check to see if the string is empty
            //if (!string.IsNullOrWhiteSpace(stringToWrap))
            //{
                return string.Format("<{0}>{1}</{0}>", typeOfTag.ToString(), stringToWrap);
            //}
            //else
            //{
            //    return null;
            //}
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
            FileValidation,
            Info,
            Item,
            Map,
            NonPlayerCharacter,
            PlayerCharacter,
            Mob,
            MountStamina,
            Room,
            Health,
            Mana,
            Sound,
            Stamina
        }
    }
}
