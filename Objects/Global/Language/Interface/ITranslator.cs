namespace Objects.Global.Language.Interface
{
    public interface ITranslator
    {
        string Translate(Translator.Languages language, string translateFrom);
    }
}