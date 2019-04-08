using Objects.Crafting.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Crafting
{
    public class CraftsmanObject : ICraftsmanObject
    {
        [ExcludeFromCodeCoverage]
        public IItem Item { get; set; }
        [ExcludeFromCodeCoverage]
        public DateTime Completion { get; set; }
        [ExcludeFromCodeCoverage]
        public IBaseObjectId CraftsmanId { get; set; }
        [ExcludeFromCodeCoverage]
        public DateTime NextNotifcation { get; set; }
        [ExcludeFromCodeCoverage]
        public string CraftmanDescripition { get; set; }
    }
}
