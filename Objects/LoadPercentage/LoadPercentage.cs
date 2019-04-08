using Objects.Global;
using Objects.Interface;
using Objects.LoadPercentage.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.LoadPercentage
{
    public class LoadPercentage : ILoadPercentage
    {
        [ExcludeFromCodeCoverage]
        public IBaseObject Object { get; set; }
        [ExcludeFromCodeCoverage]
        public int PercentageLoad { get; set; }

        public bool Load
        {
            get
            {
                return GlobalReference.GlobalValues.Random.PercentDiceRoll(PercentageLoad);
            }
        }
    }
}
