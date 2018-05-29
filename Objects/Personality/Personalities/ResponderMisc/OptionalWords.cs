using Objects.Personality.Personalities.ResponderMisc.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Personality.Personalities.ResponderMisc
{
    public class OptionalWords : IOptionalWords
    {
        [ExcludeFromCodeCoverage]
        public List<string> TriggerWords { get; set; } = new List<string>();
    }
}
