using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TelnetCommunication.Interface;

namespace TelnetCommunication
{
    public class JsonMudMessage : IMudMessage
    {
        private static JsonSerializerSettings? _settings;

        public JsonMudMessage(string guid, string message)
        {
            Guid = guid;
            Message = message;
        }

        public static JsonSerializerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    var temp = new JsonSerializerSettings();
                    temp.TypeNameHandling = TypeNameHandling.Objects;
                    _settings = temp;
                }
                return _settings;
            }
        }

        [ExcludeFromCodeCoverage]
        public string Guid { get; }
        [ExcludeFromCodeCoverage]
        public string Message { get; }
        [ExcludeFromCodeCoverage]
        public string Serialize()
        {
            return StartMessageCharacter + JsonConvert.SerializeObject(this, Formatting.Indented, Settings) + EndMessageCharacter;
        }

        public IMudMessage StringToMessage(string stringMessage)
        {
            return JsonConvert.DeserializeObject<JsonMudMessage>(stringMessage, Settings);
        }

        public IMudMessage CreateNewInstance(string guid, string message)
        {
            return new JsonMudMessage(guid, message);
        }

        public Tuple<List<string>, string> ParseRawMessage(string rawMessage)
        {
            string[] messages = rawMessage.Split(StartMessageCharacter);
            List<string> parsedMessages = new List<string>();
            StringBuilder strBldr = new StringBuilder();

            foreach (string message in messages)
            {
                //we got a new message, clear out any old ones
                strBldr.Clear();
                string newMessage = message.Replace("\0", "");
                if (newMessage.EndsWith(EndMessageCharacter.ToString()))
                {
                    try
                    {
                        IMudMessage mudMessage = StringToMessage(newMessage.Remove(newMessage.Length - 1));
                        parsedMessages.Add(mudMessage.Message);
                    }
                    catch (Exception)
                    {
                        //message is invalid json object, throw it away
                    }
                }
                else
                {
                    strBldr.Append(StartMessageCharacter + newMessage);
                }
            }

            return new Tuple<List<string>, string>(parsedMessages, strBldr.ToString());
        }

        private Char StartMessageCharacter { get; } = 'ù';
        private Char EndMessageCharacter { get; } = 'ÿ';
    }
}
