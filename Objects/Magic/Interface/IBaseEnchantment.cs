using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Stats;
using Objects.Interface;
using Objects.Magic.Enchantment.DefeatbleInfo;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;

namespace Objects.Magic.Interface
{
    public interface IEnchantment : IEvent
    {
        double ActivationPercent { get; set; }
        IEffect Effect { get; set; }
        BaseEnchantment.TargetOption Target { get; set; }
        IEffectParameter Parameter { get; set; }
        DateTime EnchantmentEndingDateTime { get; set; }
        IDefeatInfo DefeatInfo { get; set; }
    }
}