using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MessageParser;
using static Shared.TagWrapper.TagWrapper;

namespace AndroidClient
{
    public abstract class DisplayFormatter
    {
        public static SpannableString FormatText(List<ParsedMessage> messagesToDisplay)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<MessageInfo> messageInfos = new List<MessageInfo>();

            foreach (ParsedMessage parsedMessage in messagesToDisplay)
            {
                MessageInfo messageInfo = new MessageInfo();
                messageInfo.Start = stringBuilder.Length;
                messageInfo.End = messageInfo.Start + parsedMessage.Message.Length;
                messageInfo.Color = SetColor(parsedMessage);
                messageInfos.Add(messageInfo);

                stringBuilder.Append(parsedMessage.Message);
            }

            SpannableString spannableString = new SpannableString(stringBuilder.ToString());

            foreach (MessageInfo messageInfo in messageInfos)
            {
                spannableString.SetSpan(new ForegroundColorSpan(messageInfo.Color), messageInfo.Start, messageInfo.End, 0);
            }

            return spannableString;
        }

        private static Color SetColor(ParsedMessage parsedMessage)
        {
            Color color;
            switch (parsedMessage.TagType)
            {
                case TagType.AsciiArt:
                    color = Color.Gainsboro;
                    break;
                case TagType.Communication:
                    color = Color.Purple;
                    break;
                case TagType.DamageDelt:
                    color = Color.Gold;
                    break;
                case TagType.DamageReceived:
                    color = Color.DarkRed;
                    break;
                case TagType.Info:
                    color = Color.LightBlue;
                    break;
                case TagType.Item:
                    color = Color.Yellow;
                    break;
                case TagType.NonPlayerCharacter:
                    color = Color.Gray;
                    break;
                case TagType.PlayerCharacter:
                    color = Color.Orange;
                    break;
                case TagType.Mob:
                    color = Color.Cyan;
                    break;
                case TagType.Room:
                    color = Color.Green;
                    break;
                case TagType.Health:
                    color = Color.Red;
                    break;
                case TagType.Mana:
                    color = Color.Blue;
                    break;
                case TagType.Stamina:
                    color = Color.DarkGreen;
                    break;
                default:
                    color = Color.White;
                    break;
            }

            return color;
        }

        private class MessageInfo
        {
            public int Start { get; set; }
            public int End { get; set; }
            public Color Color { get; set; }
        }
    }
}