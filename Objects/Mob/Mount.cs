using Objects.Global;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public override int MaxStamina
        {
            get
            {
                if (_maxStamina == -1)
                {
                    _maxStamina = ConstitutionEffective * 10 * StaminaMultiplier;
                }
                return _maxStamina;
            }
            set
            {
                _maxStamina = value;
            }
        }

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


        public Mount(DefaultValues defaultValue, IRoom room): base(room, "mount corpse description", room.Id, room.Zone, "examine description", "look description", "sentience description", "short description")
        {
            LoadDefaultValues(defaultValue);
        }

        public Mount(DefaultValues defaultValue, IRoom room, string corpseDescription, int id, int zone, string examineDescription, string lookDescription, string sentienceDescription, string shortDescription) : this(defaultValue, room)
        {
            CorpseLookDescription = corpseDescription;
            ExamineDescription = examineDescription;
            LookDescription = lookDescription;
            SentenceDescription = sentienceDescription;
            ShortDescription = shortDescription;
        }

        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            base.FinishLoad(zoneObjectSyncValue);

            if (Level >= 1
                && StrengthStat == 0
                && DexterityStat == 0
                && ConstitutionStat == 0
                && IntelligenceStat == 0
                && WisdomStat == 0
                && CharismaStat == 0)
            {
                StrengthStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                DexterityStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                ConstitutionStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                IntelligenceStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                WisdomStat = GlobalReference.GlobalValues.Settings.BaseStatValue;
                CharismaStat = GlobalReference.GlobalValues.Settings.BaseStatValue;

                LevelPoints = GlobalReference.GlobalValues.Settings.AssignableStatPoints;

                while (LevelPoints > 0)
                {
                    LevelRandomStat();
                }

                for (int i = 1; i < Level; i++)
                {
                    LevelMobileObject();
                    //puts the level back to what it was before so this will eventually stop leveling 
                    //when we get to the desired level
                    Level--;
                    while (LevelPoints > 0)
                    {
                        LevelRandomStat();
                    }
                }
            }
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
                    ExamineDescription = "The horse is a majestic animal with a fine shinny coat.";
                    LookDescription = todo;
                    ShortDescription = RandomValue(HorseDescription);
                    SentenceDescription = "horse";

                    break;
                case DefaultValues.Unicorn:
                    Movement = 2;
                    StaminaMultiplier = 12;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(UnicornNames));
                    KeyWords.Add("Unicorn");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(UnicornDescription);
                    SentenceDescription = "unicorn";
                    break;
                case DefaultValues.Nightmare:
                    Movement = 3;
                    StaminaMultiplier = 15;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(NightmareNames));
                    KeyWords.Add("Nightmare");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(NightmareDescription);
                    SentenceDescription = "nightmare";
                    break;
                case DefaultValues.Elephant:
                    Movement = 1;
                    StaminaMultiplier = 20;
                    MaxRiders = 5;
                    KeyWords.Add(RandomValue(ElephantNames));
                    KeyWords.Add("Elephant");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(ElephantDescription);
                    SentenceDescription = "elephant";
                    break;
                case DefaultValues.Elk:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(ElkNames));
                    KeyWords.Add("Elk");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(ElkDescription);
                    SentenceDescription = "elk";
                    break;
                case DefaultValues.Panther:
                    Movement = 5;
                    StaminaMultiplier = 5;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(PantherNames));
                    KeyWords.Add("Panther");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(PantherDescription);
                    SentenceDescription = "panther";
                    break;
                case DefaultValues.Griffin:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(GriffinNames));
                    KeyWords.Add("Griffin");
                    ExamineDescription = todo;
                    LookDescription = todo;
                    ShortDescription = RandomValue(GriffinDescription);
                    SentenceDescription = "griffin";
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

        private string RandomValue(List<string> list)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(list.Count);
            return list[pos];
        }
    }
}
