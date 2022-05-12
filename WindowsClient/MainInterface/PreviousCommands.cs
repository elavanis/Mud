using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClient.MainInterface
{
    public class PreviousCommands
    {
        private List<string> previousCommands = new List<string>();
        private List<string> previousWords = new List<string>();
        private List<string> onscreenWords = new List<string>();

        public void AddOnScreenWords(string onscreenInfo)
        {
            string[] words = onscreenInfo.Split(' ');
            foreach (string word in words)
            {
                UpdateList(onscreenWords, word);
            }
        }

        public void Add(string command)
        {
            UpdateList(previousCommands, command);

            string[] words = command.Split(' ');
            foreach (string word in words)
            {
                UpdateList(previousWords, word);
            }
        }

        public string Intelisense(string command)
        {
            string previousCommand = previousCommands.LastOrDefault(c => c.StartsWith(command, StringComparison.CurrentCultureIgnoreCase));

            if (previousCommand == null)
            {
                string partialWord = command.Split(' ').LastOrDefault();
                int keepLength = command.Length - partialWord.Length;
                string completedCommand = "";

                if (keepLength != 0)
                {
                    completedCommand = command.Substring(0, keepLength);
                }

                string suggestedWord = previousWords.LastOrDefault(c => c.StartsWith(partialWord, StringComparison.CurrentCultureIgnoreCase));

                if (suggestedWord == null)
                {
                    suggestedWord = onscreenWords.LastOrDefault(c => c.StartsWith(partialWord, StringComparison.CurrentCultureIgnoreCase));
                }

                if (suggestedWord != null)
                {
                    previousCommand = $"{completedCommand}{suggestedWord}";
                }

            }

            return previousCommand;
        }

        private List<string> UpdateList(List<string> list, string matchString)
        {
            matchString = matchString.Trim();

            int index = list.FindIndex(x => x.Equals(matchString, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1)
            {
                list.RemoveAt(index);
            }

            list.Add(matchString);

            return list;
        }
    }
}
