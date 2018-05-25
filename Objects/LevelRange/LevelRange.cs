using Objects.LevelRange.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.LevelRange
{
    public class LevelRange : ILevelRange
    {
        [ExcludeFromCodeCoverage]
        public int LowerLevel { get; set; }
        [ExcludeFromCodeCoverage]
        public int UpperLevel { get; set; }
    }
}
