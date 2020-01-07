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
            string previousCommand = previousCommands.LastOrDefault(c => c.Contains(command));

            if (previousCommand == null)
            {
                string partialWord = command.Split(' ').LastOrDefault();
                int removeLength = command.Trim().Length - partialWord.Length;
                string completedCommand = "";
                if (removeLength != 0)
                {
                    completedCommand = command.Remove(removeLength).Trim();
                }

                string suggestedWord = previousWords.LastOrDefault(c => c.Contains(partialWord));

                if (suggestedWord != null)
                {
                    previousCommand = $"{completedCommand} {suggestedWord}";
                }
            }

            return previousCommand;
        }
    }
}
