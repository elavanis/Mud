using Objects.Command.Interface;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Objects.Command
{
    public class Parameter : IParameter
    {
        private static Regex findParameterNumber = new Regex(@"\[\d+\]", RegexOptions.Compiled);
        private static Regex parameterNumber = new Regex(@"\d+", RegexOptions.Compiled);
        public Parameter(string parameterRaw)
        {
            Match match = findParameterNumber.Match(parameterRaw);
            if (match.Success)
            {
                ParameterValue = parameterRaw.Replace(match.Value, "");
                string paramNumber = parameterNumber.Match(parameterRaw).Value;
                int number;
                int.TryParse(paramNumber, out number);
                ParameterNumber = number;
            }
            else
            {
                ParameterValue = parameterRaw;
                ParameterNumber = 0;
            }
        }

        [ExcludeFromCodeCoverage]
        public string ParameterValue { get; set; }

        [ExcludeFromCodeCoverage]
        public int ParameterNumber { get; set; }
    }
}
