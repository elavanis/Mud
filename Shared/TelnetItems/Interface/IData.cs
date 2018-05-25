namespace Shared.TelnetItems.Interface
{
    public interface IData
    {
        string AssetName { get; set; }
        string Base64Encoding { get; set; }
        Data.DataType Type { get; set; }
    }
}