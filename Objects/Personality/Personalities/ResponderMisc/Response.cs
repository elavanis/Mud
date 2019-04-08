using Objects.Personality.Personalities.ResponderMisc.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Personality.Personalities.ResponderMisc
{
    public class Response : IResponse
    {
        [ExcludeFromCodeCoverage]
        public List<IOptionalWords> RequiredWordSets { get; set; } = new List<IOptionalWords>();

        [ExcludeFromCodeCoverage]
        public string Message { get; set; }

        public bool Match(List<string> communicationWords)
        {
            bool matched = true;
            foreach (IOptionalWords optionalWords in RequiredWordSets)
            {
                bool optionalWordsMatched = false;

                foreach (string optionalWord in optionalWords.TriggerWords)
                {
                    if (optionalWordsMatched == false)
                    {
                        foreach (string word in communicationWords)
                        {
                            if (word.Equals(optionalWord, StringComparison.CurrentCultureIgnoreCase))
                            {
                                optionalWordsMatched = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (optionalWordsMatched == false)
                {
                    matched = false;
                    break;
                }
            }

            return matched;
        }
    }
}