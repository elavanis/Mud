using Shared.Sound.Interface;

namespace Objects.Effect.Interface
{
    public interface IEffect
    {
        void ProcessEffect(IEffectParameter parameter);

        ISound Sound { get; set; }
    }
}