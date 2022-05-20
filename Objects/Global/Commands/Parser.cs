using Objects.Command;
using Objects.Command.Interface;
using Objects.Global.Commands.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Objects.Global.Commands
{
    public class Parser : IParser
    {
        private Regex parserRegex = new Regex(@"[\""].+?[\""]|[^ ]+", RegexOptions.Compiled);
        public ICommand Parse(string text)
        {
            List<string> words = parserRegex.Matches(text)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            for (int i = 0; i < words.Count; i++)
            {
                words[i] = words[i].Replace("\"", "");
            }

            string commandName = "";
            List<IParameter> commandParameters = new List<IParameter>();

            if (words.Count > 0)
            {
                commandName = words[0].ToUpper();
            }

            for (int i = 1; i < words.Count; i++)
            {
                commandParameters.Add(new Parameter(words[i]));
            }

            ICommand command = new Command.Command(commandName, commandParameters);

            return command;
        }
    }
}
