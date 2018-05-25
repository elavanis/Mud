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

        public void Add(string command)
        {
            if (previousCommands.Contains(command))
            {
                previousCommands.Remove(command);
            }
            previousCommands.Add(command);
        }

        public string Intelisense(string command)
        {
            return previousCommands.LastOrDefault(c => c.Contains(command));
        }
    }
}
