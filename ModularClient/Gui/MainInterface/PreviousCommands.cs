using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MainInterface
{
    public class PreviousCommands
    {
        private List<string> previousCommands = new List<string>();
        private List<string> previousWords = new List<string>();

        public void Add(string command)
        {
            if (previousCommands.Contains(command))
            {
                previousCommands.Remove(command);
            }
            previousCommands.Add(command);

            string[] words = command.Split(' ');
            foreach (string word in words)
            {
                if (previousWords.Contains(word))
                {
                    previousWords.Remove(word);
                }
                previousWords.Add(word);
            }
        }

        public string Intelisense(string command)
        {
            string previousCommand = previousCommands.LastOrDefault(c => c.StartsWith(command));

            if (previousCommand == null)
            {
                string partialWord = command.Split(' ').LastOrDefault();
                int keepLength = command.Length - partialWord.Length;
                string completedCommand = "";
                if (keepLength != 0)
                {
                    completedCommand = command.Substring(0, keepLength);
                }

                string suggestedWord = previousWords.LastOrDefault(c => c.StartsWith(partialWord));

                if (suggestedWord != null)
                {
                    previousCommand = $"{completedCommand}{suggestedWord}";
                }
            }

            return previousCommand;
        }
    }
}
