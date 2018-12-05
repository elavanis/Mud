using Objects.Global;
using Objects.Global.Language;
using Objects.Item.Items.BulletinBoard.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Item.Items.BulletinBoard
{
    public class Message : IMessage
    {
        [ExcludeFromCodeCoverage]
        public string Poster { get; set; }
        [ExcludeFromCodeCoverage]
        public string Subject { get; set; }
        [ExcludeFromCodeCoverage]
        public string Text { get; set; }
        [ExcludeFromCodeCoverage]
        public GameDateTime.GameDateTime PostedDate { get; set; } = new GameDateTime.GameDateTime(DateTime.Now);
        [ExcludeFromCodeCoverage]
        public Translator.Languages WrittenLanguage { get; set; } = Translator.Languages.Common;

        public string Read(IMobileObject mobileObject)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Subject);
            stringBuilder.AppendLine(Text);
            stringBuilder.AppendLine("Sincerely");
            stringBuilder.Append(Poster);

            if (mobileObject.KnownLanguages.Contains(WrittenLanguage))
            {
                return stringBuilder.ToString();
            }
            else
            {
                return GlobalReference.GlobalValues.Translator.Translate(WrittenLanguage, stringBuilder.ToString());
            }
        }


    }
}
