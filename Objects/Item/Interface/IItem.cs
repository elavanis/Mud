using Objects.Interface;
using Objects.Mob;
using System.Collections.Generic;

namespace Objects.Item.Interface
{
    public interface IItem : IBaseObject, ILoadable, ICloneable
    {
        List<Item.ItemAttribute> Attributes { get; }
        List<MobileObject.MobileAttribute> AttributesForMobileObjectsWhenEquiped { get; }
        int Level { get; set; }
        ulong Value { get; set; }
        int Weight { get; set; }
    }
}