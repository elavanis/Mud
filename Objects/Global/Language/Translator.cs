using Objects.Global.Language.Interface;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Objects.Global.Language
{
    public class Translator : ITranslator
    {
        private ITranslatorAlgorithm algorithm { get; }
        public Translator(ITranslatorAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public enum Languages
        {
            AncientMagic,
            Common,
            Goblin,
            Magic
        }

        private Regex _regex;
        private Regex regex
        {
            get
            {
                if (_regex == null)
                {
                    _regex = new Regex("[^a-zA-Z]", RegexOptions.Compiled);
                }

                return _regex;
            }
        }

        public string Translate(Languages language, string translateFrom)
        {
            string translateFromWithoutSpaces = regex.Replace(translateFrom, "").ToLower();

            string languageName = language.ToString();
            string md5OfLanguage = algorithm.CalculateHash(languageName);
            string md5OfSentence = algorithm.CalculateHash(md5OfLanguage + translateFromWithoutSpaces);

            Dictionary<char, char> shiftedLanguage = CalculateShiftedAlphabit(md5OfSentence);

            StringBuilder newSentence = new StringBuilder();
            Queue<int> spaces = GetQueue(md5OfSentence);
            int pos = spaces.Dequeue();
            spaces.Enqueue(pos);

            foreach (char character in translateFromWithoutSpaces)
            {
                newSentence.Append(shiftedLanguage[character]);
                pos--;
                if (pos <= 0)
                {
                    while (pos <= 0
                        && spaces.Count > 0)
                    {
                        pos = spaces.Dequeue();
                    }

                    if (spaces.Count == 0)
                    {
                        //this would only happen on an all zero md5 hash
                        for (int i = 0; i < 32; i++)
                        {
                            spaces.Enqueue(GlobalReference.GlobalValues.Random.Next(1, 10));
                        }
                    }
                    spaces.Enqueue(pos);
                    newSentence.Append(" ");
                }
            }
            return newSentence.ToString().Trim();
        }

        private Queue<int> GetQueue(string md5OfSentence)
        {
            Queue<int> queue = new Queue<int>();
            foreach (char character in md5OfSentence)
            {
                queue.Enqueue(int.Parse(character.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier));
            }

            return queue;
        }

        private Dictionary<char, char> CalculateShiftedAlphabit(string md5OfSentence)
        {
            Dictionary<char, char> shiftedLanguage = new Dictionary<char, char>();
            int shiftAmount = CalculateShiftAmount(md5OfSentence);

            for (int i = 0; i < 26; i++)
            {
                int originalLetter = i + 97;
                int newLetter = originalLetter + shiftAmount;
                while (newLetter > 122)
                {
                    newLetter -= 26;
                }

                shiftedLanguage.Add((char)originalLetter, (char)newLetter);
            }

            return shiftedLanguage;
        }

        private int CalculateShiftAmount(string md5OfSentence)
        {
            int chars = 2;
            int shiftAmount = 0;
            while (shiftAmount % 26 == 0)
            {
                int temp = int.Parse(md5OfSentence.Substring(0, chars), System.Globalization.NumberStyles.AllowHexSpecifier) % 26;
                if (temp == 0)
                {
                    chars++;
                    if (chars == 32)
                    {
                        return 13;
                    }
                }
                else
                {
                    shiftAmount = temp;
                }
            }

            return shiftAmount;
        }
    }
}
