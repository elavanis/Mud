using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Item.Items.BulletinBoard.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objects.Item.Items.BulletinBoard
{
    public class BulletinBoard : Item, IBulletinBoard
    {
        private List<IMessage> messages { get; } = new List<IMessage>();

        private string FileLocation
        {
            get
            {
                return Path.Combine(GlobalReference.GlobalValues.Settings.BulletinBoardDirectory, $"{Zone}-{Id}.BulletinBoard");
            }
        }

        public BulletinBoard()
        {
            Attributes.Add(ItemAttribute.NoGet);
        }

        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            LoadMessages();
            base.FinishLoad(zoneObjectSyncValue);
        }

        private void LoadMessages()
        {
            List<IMessage> tempMessages = GlobalReference.GlobalValues.Serialization.Deserialize<List<IMessage>>(GlobalReference.GlobalValues.FileIO.ReadAllText(FileLocation));

            GameDateTime.GameDateTime toOld = new GameDateTime.GameDateTime(DateTime.Now.AddDays(-180));

            //remove old messages
            foreach (IMessage message in tempMessages)
            {
                if (message.PostedDate.IsGreaterThan(toOld))
                {
                    messages.Add(message);
                }
            }

        }

        private void SaveMessages()
        {
            GlobalReference.GlobalValues.FileIO.WriteFile(FileLocation, GlobalReference.GlobalValues.Serialization.Serialize(messages));
        }

        public void Post(IMobileObject performer, string subject, string text)
        {
            IMessage message = new Message() { Poster = performer.KeyWords[0], Subject = subject, Text = text };
            messages.Add(message);
            SaveMessages();
        }

        public IResult Remove(IMobileObject performer, int postNumber)
        {
            try
            {
                IMessage message = messages[postNumber - 1];

                if (message.Poster == performer.KeyWords[0])
                {
                    messages.Remove(message);

                    return new Result("You removed your post on the bulletin board.", true);
                }
                else if (performer.God)
                {
                    messages.Remove(message);
                    return new Result("You removed the post on the bulletin board.", true);
                }
                else
                {
                    return new Result("You can not remove someone else's post.", true);
                }
            }
            catch
            {
                return new Result($"There is no message at {postNumber}.", true);
            }
        }

        public IResult Read(IMobileObject performer, int postNumber)
        {
            string resultMessage;

            try
            {
                IMessage message = messages[postNumber - 1];

                resultMessage = message.Read(performer);
            }
            catch
            {
                resultMessage = $"Unable to find message {postNumber}.";
            }

            return new Result(resultMessage, true);
        }

        public ulong CalculateCost(IMobileObject performer)
        {
            ulong cost = 1;
            int existingMessageCount = 0;
            foreach (IMessage message in messages)
            {
                if (message.Poster == performer.KeyWords[0])
                {
                    cost *= 10;
                    existingMessageCount++;
                }
            }

            if (existingMessageCount >= 20)
            {
                cost = ulong.MaxValue;
            }

            return cost;
        }
    }
}
