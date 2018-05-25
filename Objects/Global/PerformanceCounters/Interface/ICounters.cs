namespace Objects.Global.PerformanceCounters.Interface
{
    public interface ICounters
    {
        int ConnnectedPlayers { set; }
        int CPU { set; }
        int MaxTickTimeInMs { set; }
        int Memory { set; }
    }
}