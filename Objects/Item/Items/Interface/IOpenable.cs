using Objects.Command.Interface;

namespace Objects.Item.Items.Interface
{
    public interface IOpenable
    {
        IResult Open();
        string OpenMessage { get; set; }
    }
}
