using Objects.Command;
using Objects.Command.Interface;
using Objects.Global.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            ICommand command = new Command.Command();

            if (words.Count > 0)
            {
                command.CommandName = words[0].ToUpper();
            }

            for (int i = 1; i < words.Count; i++)
            {
                command.Parameters.Add(new Parameter(words[i]));
            }

            return command;
        }
    }
}
