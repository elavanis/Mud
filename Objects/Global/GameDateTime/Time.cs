using Objects.Global.GameDateTime.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.GameDateTime
{
    public class Time : ITime
    {
        [ExcludeFromCodeCoverage]
        public DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
