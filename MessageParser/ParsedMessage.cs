using System;
using static Shared.TagWrapper.TagWrapper;

namespace MessageParser
{
    public class ParsedMessage
    {
        public string Message { get; set; }
        public TagType TagType { get; set; }
    }
}
