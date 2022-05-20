namespace Shared.TagWrapper.Interface
{
    public interface ITagWrapper
    {
        string? WrapInTag(string stringToWrapp, TagWrapper.TagType typeOfTag = TagWrapper.TagType.Info);
    }
}