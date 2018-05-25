namespace Objects.Global.TickTimes.Interface
{
    public interface ITickTimes
    {
        string Times { get; }

        void Enqueue(long ms);

        decimal MaxTime { get; }

        decimal MedianTime { get; }
    }
}