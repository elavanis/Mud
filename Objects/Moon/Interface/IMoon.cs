namespace Objects.Moon.Interface
{
    public interface IMoon
    {
        string Name { get; set; }
        //decimal MagicModifier { get; set; }
        int MoonPhaseCycleDays { get; set; }
        //decimal CurrentMagicModifier { get; }
    }
}