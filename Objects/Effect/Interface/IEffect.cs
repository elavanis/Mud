using Objects.Command.Interface;
using Objects.Global.Direction;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Mob.Interface;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Effect.Interface
{
    public interface IEffect
    {
        void ProcessEffect(IEffectParameter parameter);

        ISound Sound { get; set; }
    }
}