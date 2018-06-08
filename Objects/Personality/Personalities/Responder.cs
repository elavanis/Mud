using Objects.Language;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Personalities.ResponderMisc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Personality.Personalities
{
    public class Responder : IResponder
    {
        [ExcludeFromCodeCoverage]
        public List<Response> Responses { get; set; } = new List<Response>();

        [ExcludeFromCodeCoverage]
        public INonPlayerCharacter NonPlayerCharacter { get; set; }

        [ExcludeFromCodeCoverage]
        private static string CommunicationHeader { get; } = "<" + TagType.Communication.ToString() + ">";
        [ExcludeFromCodeCoverage]
        private static string CommunicationTrailer { get; } = "</" + TagType.Communication.ToString() + ">";

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                string lastCommunincation = null;
                string lastMessage = null;
                while ((lastMessage = npc.DequeueMessage()) != null)
                {
                    if (lastMessage.StartsWith(CommunicationHeader))
                    {
                        lastCommunincation = lastMessage;
                        break;
                    }
                }

                if (lastCommunincation != null)
                {
                    lastCommunincation = lastCommunincation.Replace(CommunicationHeader, "");
                    lastCommunincation = lastCommunincation.Replace(CommunicationTrailer, "");

                    string[] wordsRaw = lastCommunincation.Split(' ');

                    List<string> words = new List<string>();
                    int start;

                    string mobKeyword = wordsRaw[0];
                    string communicationCommand;

                    if (wordsRaw[1] == "tells")
                    {
                        //Mob tells you ...
                        start = 3;
                        communicationCommand = "tell";
                    }
                    else if (wordsRaw[1] == "says")
                    {
                        //Mob says ...
                        start = 2;
                        communicationCommand = "say";
                    }
                    else
                    {
                        //don't do anything for shouts etc
                        return command;
                    }

                    for (int i = start; i < wordsRaw.Length; i++)
                    {
                        words.Add(wordsRaw[i]);
                    }

                    foreach (Response response in Responses)
                    {
                        if (response.Match(words))
                        {
                            if (communicationCommand == "tell")
                            {
                                command = $"{communicationCommand} {mobKeyword} {response.Message}";
                            }
                            else if (communicationCommand == "say")
                            {
                                command = $"{communicationCommand} {response.Message}";
                            }
                        }
                    }
                }
            }

            return command;
        }
    }
}
