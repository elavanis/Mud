using Objects.Global;
using Objects.Interface;
using Objects.LoadPercentage.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
