namespace Objects.Interface
{
    public interface ILoadable
    {
        void FinishLoad(int zoneObjectSyncValue = -1);
        void ZoneObjectSyncLoad(int syncValue);
    }
}
