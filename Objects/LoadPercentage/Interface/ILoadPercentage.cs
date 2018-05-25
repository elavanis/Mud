using Objects.Interface;

namespace Objects.LoadPercentage.Interface
{
    public interface ILoadPercentage
    {
        bool Load { get; }
        IBaseObject Object { get; set; }
        int PercentageLoad { get; set; }
    }
}