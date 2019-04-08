using Objects.Die.Interface;
using Objects.Global;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Die
{
    public class Dice : IDice
    {
        //Needed for serialization
        [ExcludeFromCodeCoverage]
        public Dice()
        {

        }

        public Dice(int die, int sides)
        {
            Die = die;
            Sides = sides;
        }

        #region Properties
        [ExcludeFromCodeCoverage]
        public int Die { get; set; }
        [ExcludeFromCodeCoverage]
        public int Sides { get; set; }
        #endregion Properties

        public int RollDice()
        {
            int diceRoll = 0;
            for (int i = 0; i < Die; i++)
            {
                diceRoll += GlobalReference.GlobalValues.Random.Next(Sides) + 1;
            }
            return diceRoll;
        }
    }
}
