using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob
{
    public class Mount : MobileObject, IMount
    {
        public int Movement { get; set; } = -1;
        public int StaminaMultiplier { get; set; } = -1;
        public CallType TypeOfCall { get; set; } = CallType.Track;
        public bool Called { get; set; }
        public IMobileObject PersonCalling { get; set; }
        public int MaxRiders { get; set; } = -1;
        public List<IMobileObject> Riders { get; set; } = new List<IMobileObject>();

        #region AnimalInfo
        #region Names
        private List<string> HorseNames = new List<string>() { "Kisses", "Tang", "Fleetbolt", "Shadows", "Zephyr", "Snapdraon", "Sugar Blossom" };
        private List<string> UnicornNames = new List<string>() { "Uni", "Arryn", "Julius", "Wrynn", "Lancelot", "Linus", "Mawu" };
        private List<string> NightmareNames = new List<string>() { "Orkiz", "Brog'drallin", "Megmes", "Xeglomoth", "Uz'gonnath", "Tukillian,", "Ezzorith", "Igdruzoth", "Boggun", "Xugthozog" };
        private List<string> ElephantNames = new List<string>() { "Skitters", "Hanno", "Jumbo", "Peanut", "Tiny", "Tusks" };
        private List<string> ElkNames = new List<string>() { "Addax", "Adder", "Buck", "Cabrilla", "Roe" };
        private List<string> PantherNames = new List<string>() { "Storm", "Fyre", "Axe", "Ghost", "Fang", "Reaper", "Domino", "Enigma", "Neko", "Maya", "Paws", "Rawr", "Smokey" };
        private List<string> GriffinNames = new List<string>() { "Apollo", "Hyperion", "Dreamwings", "Petalfeather", "Oswald", "Thanatos", "Brightbeak", "Ebonfeathers", "Torr", "Tiki" };
        #endregion Names

        #region Descriptions
        #endregion Descriptions
        #endregion AnimalInfo

        public void LoadDefaultValues(DefaultValues defaultValues)
        {
            switch (defaultValues)
            {
                case DefaultValues.Horse:
                    Movement = 2;
                    StaminaMultiplier = 10;
                    TypeOfCall = CallType.Track;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(HorseNames));
                    KeyWords.Add("Horse");
                    break;
                case DefaultValues.Unicorn:
                    Movement = 2;
                    StaminaMultiplier = 12;
                    TypeOfCall = CallType.Track;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(UnicornNames));
                    KeyWords.Add("Unicorn");
                    break;
                case DefaultValues.Nightmare:
                    Movement = 3;
                    StaminaMultiplier = 15;
                    TypeOfCall = CallType.Summon;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(NightmareNames));
                    KeyWords.Add("Nightmare");
                    break;
                case DefaultValues.Elephant:
                    Movement = 1;
                    StaminaMultiplier = 20;
                    TypeOfCall = CallType.Track;
                    MaxRiders = 5;
                    KeyWords.Add(RandomName(ElephantNames));
                    KeyWords.Add("Elephant");
                    break;
                case DefaultValues.Elk:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    TypeOfCall = CallType.Track;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(ElkNames));
                    KeyWords.Add("Elk");
                    break;
                case DefaultValues.Panther:
                    Movement = 5;
                    StaminaMultiplier = 5;
                    TypeOfCall = CallType.Track;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(PantherNames));
                    KeyWords.Add("Panther");
                    break;
                case DefaultValues.Griffin:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    TypeOfCall = CallType.Summon;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(GriffinNames));
                    KeyWords.Add("Griffin");
                    break;

            }
        }

        public enum DefaultValues
        {
            Horse,
            Unicorn,
            Nightmare,
            Elephant,
            Elk,
            Panther,
            Griffin
        }

        public enum CallType
        {
            Summon,
            Track
        }

        private string RandomName(List<string> names)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(names.Count);
            return names[pos];
        }
    }
}
