using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Shared.TagWrapper.TagWrapper;

namespace MessageParser
{
    public static class Parser
    {
        private static Stack<TagType> tagTypeStack = new Stack<TagType>();

        private static Dictionary<TagType, Tuple<string, string>> _parsedTagTypes = null;
        private static Dictionary<TagType, Tuple<string, string>> ParsedTagTypes
        {
            get
            {
                if (_parsedTagTypes == null)
                {
                    _parsedTagTypes = new Dictionary<TagType, Tuple<string, string>>();

                    foreach (TagType tagType in Enum.GetValues(typeof(TagType)))
                    {
                        _parsedTagTypes.Add(tagType, new Tuple<string, string>($"<{tagType}>", $"</{tagType}>"));
                    }
                }

                return _parsedTagTypes;
            }
        }

        public static List<ParsedMessage> Parse(string line)
        {
            List<ParsedMessage> parsedInfo = new List<ParsedMessage>();

            int previousPos = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line.Substring(i, 1) == "<")
                {
                    string lineRemaining = line.Substring(i);

                    foreach (TagType key in Enum.GetValues(typeof(TagType)))
                    {
                        if (lineRemaining.StartsWith(ParsedTagTypes[key].Item1))
                        {
                            //found new tag type
                            if (previousPos != 0)
                            {
                                if (tagTypeStack.Count == 0)
                                {
                                    //this will be 0 if the previous tag type closed out
                                    //it doesn't really matter what type of value this is as long as it has something.
                                    //this should only be carriage return returns in theory
                                    tagTypeStack.Push(TagType.ClientCommand);
                                }

                                ParsedMessage parsedMessage = new ParsedMessage();
                                parsedMessage.TagType = tagTypeStack.Peek();
                                parsedMessage.Message = line.Substring(previousPos, i - previousPos);
                                parsedInfo.Add(parsedMessage);
                            }
                            previousPos = i + ParsedTagTypes[key].Item1.Length;
                            tagTypeStack.Push(key);
                            break;
                        }
                        else if (lineRemaining.StartsWith(ParsedTagTypes[key].Item2))
                        {
                            //found closing tag type
                            if (previousPos != 0)
                            {
                                if (tagTypeStack.Count == 0)
                                {
                                    //this will be 0 if the previous tag type closed out
                                    //it doesn't really matter what type of value this is as long as it has something.
                                    //this should only be carriage return returns in theory
                                    tagTypeStack.Push(TagType.ClientCommand);
                                }

                                ParsedMessage parsedMessage = new ParsedMessage();
                                parsedMessage.TagType = tagTypeStack.Peek();
                                parsedMessage.Message = line.Substring(previousPos, i - previousPos);
                                parsedInfo.Add(parsedMessage);
                            }
                            previousPos = i + ParsedTagTypes[key].Item2.Length;
                            previousPos = 0;
                            tagTypeStack.Pop();
                            break;
                        }
                    }
                }
            }

            tagTypeStack.Clear();


            parsedInfo = AddSpacing(parsedInfo);

            return parsedInfo;
        }

        private static List<ParsedMessage> AddSpacing(List<ParsedMessage> parsedInfo)
        {
            foreach (ParsedMessage parsedMessage in parsedInfo)
            {
                string appendment = "";

                if (parsedMessage.TagType == TagType.Stamina)
                {
                    if (parsedInfo.Where(e => e.TagType == TagType.MountStamina).Count() == 0)
                    {
                        appendment = "\r\n";
                    }
                    else
                    {
                        appendment += " ";
                    }
                }
                else if (parsedMessage.TagType == TagType.Health || parsedMessage.TagType == TagType.Mana)
                {
                    appendment += " ";
                }
                else
                {
                    appendment += "\r\n";
                }

                parsedMessage.Message += appendment;
            }

            return parsedInfo;
        }
    }
}
