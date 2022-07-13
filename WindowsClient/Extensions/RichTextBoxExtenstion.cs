using MessageParser;
using Newtonsoft.Json;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shared.TagWrapper.TagWrapper;

namespace WindowsClient.Extensions
{
    public static class RichTextBoxExtenstion
    {
        public static void AddFormatedText(this RichTextBox box, List<ParsedMessage> parsedMessages, IFileIO fileIO, Settings settings)
        {
            foreach (ParsedMessage parsedMessage in parsedMessages)
            {
                if (!settings.ColorDictionary.ContainsKey(parsedMessage.TagType))
                {
                    settings.ColorDictionary.Add(parsedMessage.TagType, Color.White );
                    settings.Save();
                }

                AppendText(box, parsedMessage.Message, settings.ColorDictionary[parsedMessage.TagType]);

            }
        }

        private static void AppendText(this RichTextBox box, string text, Color color)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Textbox may transform chars, so (end-start) != text.Length
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
                // could set box.SelectionBackColor, box.SelectionFont too.
            }
            box.SelectionLength = 0; // clear
        }
    }
}
