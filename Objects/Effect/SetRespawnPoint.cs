using Objects.Effect.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Objects.Global.Logging.LogSettings;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Effect
{
    public class SetRespawnPoint : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        public void ProcessEffect(IEffectParameter parameter)
        {
            if (parameter.Target is IPlayerCharacter pc)
            {
                pc.RespawnPoint = parameter.ObjectId;

                GlobalReference.GlobalValues.Logger.Log(pc, LogLevel.DEBUG, string.Format("{0} respawn point was reset to {1}.", pc.Name, pc.RespawnPoint));
            }
        }
    }
}
