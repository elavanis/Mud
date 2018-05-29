using Objects.Language;
using Objects.Personality.Personalities.ResponderMisc.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Personality.Personalities.ResponderMisc
{
    public class Response : IResponse
    {
        [ExcludeFromCodeCoverage]
        public List<OptionalWords> RequiredWordSets { get; set; } = new List<OptionalWords>();

        [ExcludeFromCodeCoverage]
        public string Message { get; set; }

        public bool Match(List<string> communicationWords)
        {
            bool matched = true;
            foreach (OptionalWords optionalWords in RequiredWordSets)
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