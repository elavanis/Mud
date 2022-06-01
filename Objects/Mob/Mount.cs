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
        private List<MountDescriptions> HorseDescriptions = new List<MountDescriptions>();
        private List<MountDescriptions> UnicornDescriptions = new List<MountDescriptions>();
        private List<MountDescriptions> NightmareDescriptions = new List<MountDescriptions>();
        private List<MountDescriptions> ElephantDescriptions = new List<MountDescriptions>();
        private List<MountDescriptions> ElkDescription = new List<MountDescriptions>();
        private List<MountDescriptions> PantherDescription = new List<MountDescriptions>();
        private List<MountDescriptions> GriffinDescription = new List<MountDescriptions>();

        #endregion Descriptions
        #endregion AnimalInfo


        public Mount(DefaultValues defaultValue, IRoom room) : base(room, "mount corpse description", "examine description", "look description", "sentience description", "short description")
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

            HorseDescriptions.Add(new MountDescriptions("The large black horse swats fly away with its tail.", "The horse catches your glance and quickly looks your direction.", "A large black horse."));
            HorseDescriptions.Add(new MountDescriptions("The horse's front right hoof has a small patch of white like its wearing a sock.", "The horse stands at attention waiting for directions.", "A brown horse with black mane."));
            HorseDescriptions.Add(new MountDescriptions("The brown spots are spread out on the horse in such a way that it looks like it has chocolate chips in a cookie.", "The horse ignores you can continues to contemplate the things horses contemplate.", "Almost like the horse is floating it trots around as if playing a game with you."));
            HorseDescriptions.Add(new MountDescriptions("The horse is a pure white that reminds you of freshly fallen snow.", "Almost like the horse is floating it trots around as if playing a game with you.", "The horse is white like snow."));

            UnicornDescriptions.Add(new MountDescriptions("The unicorns horn is an iridescent color that shimmers as it moves its head.", "The unicorn paws the ground as you look at it.", "The white unicorn looks at you."));

            NightmareDescriptions.Add(new MountDescriptions("As you approach the nightmare it briefly flies into the air leaving behind hoof prints in the air that burst into flames briefly before leaving a slowly fading red glow, this is truly is the stuff of nightmares.", "As you look at this nightmare it snorts at you and fire briefly flares out of its nostrils.", "Flames burn brightly from the mane and hooves of this black as nightmare."));
            NightmareDescriptions.Add(new MountDescriptions(todo, todo, "The elephants trunk reaches down toward the ground looking for food."));
            NightmareDescriptions.Add(new MountDescriptions(todo, todo, "The elk has a large rack with two reigns tied off on a saddle."));
            NightmareDescriptions.Add(new MountDescriptions(todo, todo, "Yellow eyes almost glow against the black panthers fur."));
            NightmareDescriptions.Add(new MountDescriptions(todo, todo, "A majestic griffin stands at the ready."));
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

        private PopulateRandomValues(List<string> names, List<string> examines, List<string> looks, List<string> shorts)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(names.Count);
            KeyWords.Add(names[pos]);

            pos = GlobalReference.GlobalValues.Random.Next(names.Count);
            ExamineDescription = examines[pos];
            LookDescription = looks[pos];
            ShortDescription = shorts[pos];
        }

        private string RandomValue(List<string> list)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(list.Count);
            return list[pos];
        }

        private record MountDescriptions
        {
            public MountDescriptions(string examineDescription, string lookDescription, string shortDescription)
            {
                ExamineDescription = examineDescription;
                LookDescription = lookDescription;
                ShortDescription = shortDescription;
            }

            public string ExamineDescription { get; }
            public string LookDescription { get; }
            public string ShortDescription { get; }
        }
    }
}
