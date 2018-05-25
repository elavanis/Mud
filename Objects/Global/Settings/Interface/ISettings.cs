namespace Objects.Global.Settings.Interface
{
    public interface ISettings
    {
        int AssignableStatPoints { get; set; }
        int BaseStatValue { get; set; }
        int MaxLevel { get; set; }
        double Multiplier { get; set; }
        string PlayerCharacterDirectory { get; set; }
        string ZoneDirectory { get; set; }
        string AssetsDirectory { get; set; }
        string AsciiArt { get; set; }
        int Port { get; set; }
        bool SendMapPosition { get; set; }
    }
}