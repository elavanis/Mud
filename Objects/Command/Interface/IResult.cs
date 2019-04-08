namespace Objects.Command.Interface
{
    public interface IResult
    {
        string ResultMessage { get; set; }
        bool AllowAnotherCommand { get; set; }
    }
}
