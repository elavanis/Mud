using System.Collections.Generic;

namespace Objects.Global.StringManuplation.Interface
{
    public interface IStringManipulator
    {
        string Manipulate(List<KeyValuePair<string, string>> replacementKeyValuePair, string stringToBeManipulated);

        string UpdateTargetPerformer(string performer, string target, string message);

        string CapitalizeFirstLetter(string input);
    }
}