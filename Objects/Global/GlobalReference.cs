using Objects.Global.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Global
{
    public static class GlobalReference
    {
        [ExcludeFromCodeCoverage]
        public static IGlobalValues GlobalValues { get; set; } = new GlobalValues();
    }
}
