using Objects.Effect.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    public class Blindness : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        [ExcludeFromCodeCoverage]
        public void ProcessEffect(IEffectParameter parameter)
        {
            //do nothing, its just a status effect
        }
    }
}
