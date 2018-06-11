using Objects.Personality.Interface;
using Objects.Personality.Personalities.ResponderMisc;
using Objects.Personality.Personalities.ResponderMisc.Interface;
using System.Collections.Generic;
using static Objects.Personality.Personalities.Responder;

namespace Objects.Personality.Personalities.Interface
{
    public interface IResponder : IPersonality
    {
        List<IResponse> Responses { get; set; }
    }
}