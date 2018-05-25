using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelnetCommunication.Interface
{
    public interface IMudMessage
    {
        string Guid { get; set; }
        string Message { get; set; }

        string Serialize();

        IMudMessage StringToMessage(string stringMessage);

        IMudMessage CreateNewInstance();

        Tuple<List<string>, string> ParseRawMessage(string rawMessage);

    }
}
