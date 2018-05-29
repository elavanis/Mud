using System.Collections.Generic;
using Objects.Language;

namespace Objects.Personality.Personalities.ResponderMisc.Interface
{
    public interface IResponse
    {
        string Message { get; set; }
        List<OptionalWords> RequiredWordSets { get; set; }
    }
}