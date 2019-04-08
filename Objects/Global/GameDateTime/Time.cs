using Objects.Global.GameDateTime.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

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
