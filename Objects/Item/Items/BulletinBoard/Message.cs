using Objects.Item.Items.BulletinBoard.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Item.Items.BulletinBoard
{
    public class Message : IMessage
    {
        public string Poster { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public GameDateTime.GameDateTime PostedDate { get; set; }

        public string Read()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Subject);
            stringBuilder.AppendLine(Text);
            stringBuilder.AppendLine("Sincerely");
            stringBuilder.Append(Poster);

            return stringBuilder.ToString();
        }
    }
}
