namespace Objects.Die.Interface
{
    public interface IDice
    {
        int Die { get; set; }
        int Sides { get; set; }

        int RollDice();
    }
}