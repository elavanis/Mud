using System;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global.Commands.Interface;
using Objects.Global.DefaultValues.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Exp.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Global.GameDateTime.Interface;
using Objects.Global.Guild.Interface;
using Objects.Global.Language.Interface;
using Objects.Global.Logging.Interface;
using Objects.Global.MoneyToCoins.Interface;
using Objects.Global.MultiClassBonus.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.Serialization.Interface;
using Objects.Global.Settings.Interface;
using Objects.Global.TickTimes.Interface;
using Objects.Global.UpTime.Interface;
using Objects.World.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using Objects.Global.PerformanceCounters.Interface;
using Objects.Global.Map.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.ValidateAsset.Interface;
using Objects.Global.StringManuplation.Interface;

namespace Objects.Global.Interface
{
    public interface IGlobalValues
    {
        ICanMobDoSomething CanMobDoSomething { get; set; }
        ICommandList CommandList { get; set; }
        ICounters Counters { get; set; }
        IDefaultValues DefaultValues { get; set; }
        IEngine Engine { get; set; }
        IExperience Experience { get; set; }
        IFileIO FileIO { get; set; }
        IFindObjects FindObjects { get; set; }
        IInGameDateTime GameDateTime { get; set; }
        IGuildAbilities GuildAbilities { get; set; }
        ILogger Logger { get; set; }
        IMap Map { get; set; }
        IMoneyToCoins MoneyToCoins { get; set; }
        IMultiClassBonus MultiClassBonus { get; set; }
        INotify Notify { get; set; }
        IParser Parser { get; set; }
        IRandom Random { get; set; }
        IRandomDropGenerator RandomDropGenerator { get; set; }
        ISerialization Serialization { get; set; }
        ISettings Settings { get; set; }
        DateTime StartTime { get; set; }
        IStringManipulator StringManipulator { get; set; }
        ITagWrapper TagWrapper { get; set; }
        ulong TickCounter { get; set; }
        ITickTimes TickTimes { get; set; }
        ITime Time { get; set; }
        ITranslator Translator { get; set; }
        IUpTime UpTime { get; set; }
        IValidateAsset ValidateAsset { get; set; }
        IWorld World { get; set; }

        void Initilize();
    }
}