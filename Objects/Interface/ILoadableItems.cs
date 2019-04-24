using Objects.Item.Interface;
using Objects.LoadPercentage.Interface;
using System.Collections.Generic;

namespace Objects.Interface
{
    public interface ILoadableItems
    {
        List<ILoadPercentage> LoadableItems { get; }
    }
}
