using Objects.Item.Interface;
using System.Collections.Generic;

namespace Objects.Item.Items.Interface
{
    public interface IContainer
    {
        List<IItem> Items { get; }
    }
}
