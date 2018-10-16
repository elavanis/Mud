using Objects.Effect;
using Objects.Magic.Interface;
using Objects.Room.SpecificRoom.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Room.SpecificRoom
{
    public class House : Room, IHouse
    {
        public string Owner { get; set; }
        public HashSet<string> Guests { get; set; } = new HashSet<string>();
    }
}
