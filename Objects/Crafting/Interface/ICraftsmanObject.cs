using System;
using Objects.Interface;
using Objects.Item.Interface;

namespace Objects.Crafting.Interface
{
    public interface ICraftsmanObject
    {
        DateTime Completion { get; set; }
        DateTime NextNotifcation { get; set; }
        IBaseObjectId CraftsmanId { get; set; }
        string CraftmanDescripition { get; set; }
        IItem Item { get; set; }
    }
}