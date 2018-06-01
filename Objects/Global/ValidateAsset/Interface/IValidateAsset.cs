namespace Objects.Global.ValidateAsset.Interface
{
    public interface IValidateAsset
    {
        void ClearValidations();
        string GetCheckSum(string asset);
    }
}