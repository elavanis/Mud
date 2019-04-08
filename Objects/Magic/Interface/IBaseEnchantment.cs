using Objects.Effect.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Magic.Enchantment.DefeatbleInfo.Interface;
using System;

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