using Objects.Personality.ResponderMisc.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Personality.ResponderMisc
{
    public class OptionalWords : IOptionalWords
    {
        [ExcludeFromCodeCoverage]
        public List<string> TriggerWords { get; set; } = new List<string>();
    }
}
