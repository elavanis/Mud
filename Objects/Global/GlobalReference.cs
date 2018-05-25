using Objects.Global.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global
{
    public static class GlobalReference
    {
        [ExcludeFromCodeCoverage]
        public static IGlobalValues GlobalValues { get; set; } = new GlobalValues();
    }
}
