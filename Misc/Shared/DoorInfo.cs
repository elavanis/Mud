namespace MiscShared
{
    public class DoorInfo
    {
        public string Name { get; set; }
        public string OpenMessage { get; set; }
        public bool Linked { get; set; }
        public string Description { get; set; }
        public bool Opened { get; set; }
        public bool Locked { get; set; }

        public DoorInfo(string name, string openMessage, bool linked, string description, bool opened = false, bool locked = false)
        {
            Name = name;
            OpenMessage = openMessage;
            Linked = linked;
            Description = description;
            Opened = opened;
            Locked = locked;
        }
    }
}
