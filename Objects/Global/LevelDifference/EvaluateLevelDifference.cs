using Objects.Global.LevelDifference.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.LevelDifference
{
    public class EvaluateLevelDifference : IEvaluateLevelDifference
    {
        public string Evalute(IMobileObject performer, IMobileObject mobToConsiderAttacking)
        {
            if (performer.Level == mobToConsiderAttacking.Level)
            {
                return "This should be a close match.";
            }

            int differnce = performer.Level - mobToConsiderAttacking.Level;
            string pronoun;
            if (differnce > 0)
            {
                pronoun = "You";
            }
            else
            {
                pronoun = "They";
            }
            switch (Math.Abs(differnce))
            {
                case 0:
                    return "This should be a close match.";
                case 1:
                    return $"{pronoun} should be victorious but badly wounded.";
                case 2:
                    return $"{pronoun} should win.";
                case 3:
                case 4:
                    if (differnce > 0)
                    {
                        return $"The house money is on you winning.";
                    }
                    else
                    {
                        return $"The house money is on them winning.";
                    }
                case 5:
                case 6:
                case 7:
                    return $"{pronoun} don't have a chance.";
                default:
                    if (differnce > 0)
                    {
                        return "There should be laws against attacking the helpless.";
                    }
                    else
                    {
                        return "MEDIC!!!";
                    }
            }
        }
    }
}
