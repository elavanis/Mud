using System.Collections.Generic;

namespace Objects.Personality.Personalities.ResponderMisc.Interface
{
    public interface IResponse
    {
        string Message { get; set; }
        List<IOptionalWords> RequiredWordSets { get; set; }
        bool Match(List<string> communicationWords);
    }
}