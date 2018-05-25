using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Magic.Interface
{
    public interface IEnchantment : IEvent
    {
        int ActivationPercent { get; set; }
        IEffect Effect { get; set; }
        BaseEnchantment.TargetOption Target { get; set; }
        IEffectParameter Parameter { get; set; }
        DateTime EnchantmentEndingDateTime { get; set; }
    }
}