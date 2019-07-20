using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Mob
{
    public class Mount : MobileObject, IMount
    {
        [ExcludeFromCodeCoverage]
        public int Movement { get; set; } = -1;
        [ExcludeFromCodeCoverage]
        public int StaminaMultiplier { get; set; } = -1;
        [ExcludeFromCodeCoverage]
        public int MaxRiders { get; set; } = -1;
        [ExcludeFromCodeCoverage]
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
        private List<string> HorseDescription = new List<string>() { "A large black horse.", "A brown horse with black mane.", "Standing at fifteen hands tall is a white horse with brown spots.", "A horse is white like snow." };
        private List<string> UnicornDescription = new List<string>() { "The white unicorn looks at you." };
        private List<string> NightmareDescription = new List<string>() { "Flames burn brightly from the mane and hooves of this black as night horse." };
        private List<string> ElephantDescription = new List<string>() { "The elephants trunk reaches down toward the ground looking for food." };
        private List<string> ElkDescription = new List<string>() { "The elk has a large rack with two reigns tied off on a saddle." };
        private List<string> PantherDescription = new List<string>() { "Yellow eyes almost glow against the black panthers fur." };
        private List<string> GriffinDescription = new List<string>() { "A majestic griffin stands at the ready." };
        #endregion Descriptions
        #endregion AnimalInfo

        public Mount()
        {
        }

        public Mount(DefaultValues defaultValue)
        {
            LoadDefaultValues(defaultValue);
        }

        private void LoadDefaultValues(DefaultValues defaultValue)
        {
            switch (defaultValue)
            {
                case DefaultValues.Horse:
                    Movement = 2;
                    StaminaMultiplier = 10;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(HorseNames));
                    KeyWords.Add("Horse");
                    ShortDescription = RandomValue(HorseDescription);
                    SentenceDescription = "horse";
                    break;
                case DefaultValues.Unicorn:
                    Movement = 2;
                    StaminaMultiplier = 12;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(UnicornNames));
                    KeyWords.Add("Unicorn");
                    ShortDescription = RandomValue(UnicornDescription);
                    SentenceDescription = "unicorn";
                    break;
                case DefaultValues.Nightmare:
                    Movement = 3;
                    StaminaMultiplier = 15;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(NightmareNames));
                    KeyWords.Add("Nightmare");
                    ShortDescription = RandomValue(NightmareDescription);
                    SentenceDescription = "nightmare";
                    break;
                case DefaultValues.Elephant:
                    Movement = 1;
                    StaminaMultiplier = 20;
                    MaxRiders = 5;
                    KeyWords.Add(RandomValue(ElephantNames));
                    KeyWords.Add("Elephant");
                    ShortDescription = RandomValue(ElephantDescription);
                    SentenceDescription = "elephant";
                    break;
                case DefaultValues.Elk:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(ElkNames));
                    KeyWords.Add("Elk");
                    ShortDescription = RandomValue(ElkDescription);
                    SentenceDescription = "elk";
                    break;
                case DefaultValues.Panther:
                    Movement = 5;
                    StaminaMultiplier = 5;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(PantherNames));
                    KeyWords.Add("Panther");
                    ShortDescription = RandomValue(PantherDescription);
                    SentenceDescription = "panther";
                    break;
                case DefaultValues.Griffin:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(GriffinNames));
                    KeyWords.Add("Griffin");
                    ShortDescription = RandomValue(GriffinDescription);
                    SentenceDescription = "griffin";
                    break;
            }

            ExamineDescription = "you should not see this";
            LookDescription = "you should not see this";
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

        private string RandomValue(List<string> list)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(list.Count);
            return list[pos];
        }
    }
}
