using Objects.Global.Direction;
using Objects.Personality.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Personality.Personalities.Interface
{
    public interface IGuard : IPersonality
    {
        HashSet<Directions.Direction> GuardDirections { get; set; }

        string BlockLeaveMessage { get; set; }
    }
}
