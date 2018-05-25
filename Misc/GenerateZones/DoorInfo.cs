using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateZones
{
    public class DoorInfo
    {
        public string Name { get; set; }
        public string OpenMessage { get; set; }
        public bool Linked { get; set; }
        public string Description { get; set; }

        public DoorInfo(string name, string openMessage, bool linked, string description)
        {
            Name = name;
            OpenMessage = openMessage;
            Linked = linked;
            Description = description;
        }
    }
}
