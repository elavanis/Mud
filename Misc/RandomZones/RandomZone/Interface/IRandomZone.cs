using Objects.Zone.Interface;

namespace RandomZone.Interface
{
    public interface IRandomZone
    {
        void Generate(int x, int y, int fillPercent = 100, int randomSeed = -1);
        IZone ConvertToZone(int zoneId);
    }
}