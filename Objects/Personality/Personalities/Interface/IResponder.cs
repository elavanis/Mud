using Objects.Personality.Interface;
using Objects.Personality.Personalities.ResponderMisc;
using System.Collections.Generic;
using static Objects.Personality.Personalities.Responder;

namespace Objects.Personality.Personalities.Interface
{
    public interface IResponder : IPersonality
    {
        List<Response> Responses { get; set; }
    }
}