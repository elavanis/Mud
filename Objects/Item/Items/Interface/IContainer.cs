using Objects.Command.Interface;
using Objects.Item.Interface;
using Objects.LoadPercentage.Interface;
using System.Collections.Generic;

namespace Objects.Item.Items.Interface
{
    public interface IContainer
    {
        List<IItem> Items { get; }
    }
}
