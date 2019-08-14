using Objects.Personality.Interface;
using Objects.Personality.ResponderMisc.Interface;
using System.Collections.Generic;

namespace Objects.Personality.Interface
{
    public interface IResponder : IPersonality
    {
        List<IResponse> Responses { get; set; }
    }
}