using Objects.Mob.Interface;

namespace Objects.Mob.SpecificNPC.Interface
{
    public interface IElemental : INonPlayerCharacter
    {
        ElementType ElementType { get; set; }
        WeatherDirection WeatherDirection { get; set; }
        int WeatherTrigger { get; set; }
        WeatherType WeatherType { get; set; }

        void ProcessElementalTick();
        void SetElement(ElementType elementType);
    }
}