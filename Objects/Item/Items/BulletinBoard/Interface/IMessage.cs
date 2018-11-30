using Objects.GameDateTime;
using Objects.Item.Items.Interface;

namespace Objects.Item.Items.BulletinBoard.Interface
{
    public interface IMessage : IRead
    {
        GameDateTime.GameDateTime PostedDate { get; set; }
        string Poster { get; set; }
        string Subject { get; set; }
        string Text { get; set; }
    }
}