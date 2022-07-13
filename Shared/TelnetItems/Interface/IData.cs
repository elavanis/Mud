namespace Shared.TelnetItems.Interface
{
    public interface IData
    {
        string AssetName { get;}
        string Base64Encoding { get; }
        Data.DataType Type { get; }
    }
}