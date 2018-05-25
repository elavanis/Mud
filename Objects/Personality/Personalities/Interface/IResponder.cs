using Objects.Personality.Interface;
using System.Collections.Generic;
using static Objects.Personality.Personalities.Responder;

namespace Objects.Personality.Personalities.Interface
{
    public interface IResponder : IPersonality
    {
        List<Response> Responses { get; set; }
    }
}