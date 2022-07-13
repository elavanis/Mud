using System;
using System.Collections.Generic;

namespace TelnetCommunication.Interface
{
    public interface IMudMessage
    {
        string Guid { get; }
        string Message { get; }

        string Serialize();

        IMudMessage? StringToMessage(string stringMessage);

        IMudMessage CreateNewInstance(string guid, string message);

        Tuple<List<string>, string> ParseRawMessage(string rawMessage);

    }
}
