using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Room.SpecificRoom.Interface
{
    public interface IHouse : IRoom
    {
        string Owner { get; set; }
        HashSet<string> Guests { get; set; }
    }
}
