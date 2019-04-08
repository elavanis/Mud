using Objects.LevelRange.Interface;
using System.Diagnostics.CodeAnalysis;

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
