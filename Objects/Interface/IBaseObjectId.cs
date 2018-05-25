namespace Objects.Interface
{
    public interface IBaseObjectId
    {
        int Id { get; set; }
        int Zone { get; set; }

        string ToString();
    }
}