using Objects.Global;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
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
        private List<MountDescription> HorseDescriptions = new List<MountDescription>();
        private List<MountDescription> UnicornDescriptions = new List<MountDescription>();
        private List<MountDescription> NightmareDescriptions = new List<MountDescription>();
        private List<MountDescription> ElephantDescriptions = new List<MountDescription>();
        private List<MountDescription> ElkDescriptions = new List<MountDescription>();
        private List<MountDescription> PantherDescriptions = new List<MountDescription>();
        private List<MountDescription> GriffinDescriptions = new List<MountDescription>();

        #endregion Descriptions
        #endregion AnimalInfo

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Obsolete("Needed for deserialization.", true)]
        public Mount() : base(null, null, null, null, null) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Mount(DefaultValues defaultValue, IRoom room) : base(room, "examine description", "look description", "sentience description", "short description")
        {
            HorseDescriptions.Add(new MountDescription("The large black horse swats fly away with its tail.", "The horse catches your glance and quickly looks your direction.", "A large black horse."));
            HorseDescriptions.Add(new MountDescription("The horse's front right hoof has a small patch of white like its wearing a sock.", "The horse stands at attention waiting for directions.", "A brown horse with black mane."));
            HorseDescriptions.Add(new MountDescription("The brown spots are spread out on the horse in such a way that it looks like it has chocolate chips in a cookie.", "The horse ignores you can continues to contemplate the things horses contemplate.", "Almost like the horse is floating it trots around as if playing a game with you."));
            HorseDescriptions.Add(new MountDescription("The horse is a pure white that reminds you of freshly fallen snow.", "Almost like the horse is floating it trots around as if playing a game with you.", "The horse is white like snow."));

            UnicornDescriptions.Add(new MountDescription("The unicorns horn is an iridescent color that shimmers as it moves its head.", "The unicorn paws the ground as you look at it.", "The white unicorn looks at you."));

            NightmareDescriptions.Add(new MountDescription("As you approach the nightmare it briefly flies into the air leaving behind hoof prints in the air that burst into flames briefly before leaving a slowly fading red glow, this is truly is the stuff of nightmares.", "As you look at this nightmare it snorts at you and fire briefly flares out of its nostrils.", "Flames burn brightly from the mane and hooves of this black as ink nightmare."));

            ElephantDescriptions.Add(new MountDescription("The elephants large flappy ears create a fanning effect as you get near.", "This elephant looks to have seen multiple battles.  It has multiple scars on its thick skin, a broken tusk and looks to be blind in one eye as well.", "The elephants trunk reaches down toward the ground looking for food."));

            ElkDescriptions.Add(new MountDescription("The elk barks and steps back as you approach it to get a closer look.", "The elk has taupe colored body with a that gets darker as it approaches the head.", "The elk has a large rack with two reigns tied off on a saddle."));

            PantherDescriptions.Add(new MountDescription("With soft paws this panther is able to silently stalk its prey and is know as a silent killer.", "Black as night you can see why if you didn't hear you would never know it was there.", "Yellow eyes almost glow against the black panthers fur."));

            GriffinDescriptions.Add(new MountDescription("Standing eighteen feet tall this giraffe towers above you.", "Brown and white spots allows it to hide from predators amongst nature.", "A majestic griffin stands at the ready."));

            LoadDefaultValues(defaultValue);
        }

        public Mount(DefaultValues defaultValue, IRoom room, string examineDescription, string lookDescription, string sentienceDescription, string shortDescription) : this(defaultValue, room)
        {
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
            MountDescription mountDescription = null!;
            switch (defaultValue)
            {
                case DefaultValues.Horse:
                    Movement = 2;
                    StaminaMultiplier = 10;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(HorseNames));
                    KeyWords.Add("Horse");
                    SentenceDescription = "horse";
                    mountDescription = RandomValue(HorseDescriptions);
                    break;
                case DefaultValues.Unicorn:
                    Movement = 2;
                    StaminaMultiplier = 12;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(UnicornNames));
                    KeyWords.Add("Unicorn");
                    SentenceDescription = "unicorn";
                    mountDescription = RandomValue(UnicornDescriptions);
                    break;
                case DefaultValues.Nightmare:
                    Movement = 3;
                    StaminaMultiplier = 15;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(NightmareNames));
                    KeyWords.Add("Nightmare");
                    SentenceDescription = "nightmare";
                    mountDescription = RandomValue(NightmareDescriptions);
                    break;
                case DefaultValues.Elephant:
                    Movement = 1;
                    StaminaMultiplier = 20;
                    MaxRiders = 5;
                    KeyWords.Add(RandomValue(ElephantNames));
                    KeyWords.Add("Elephant");
                    SentenceDescription = "elephant";
                    mountDescription = RandomValue(ElephantDescriptions);
                    break;
                case DefaultValues.Elk:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(ElkNames));
                    KeyWords.Add("Elk");
                    SentenceDescription = "elk";
                    mountDescription = RandomValue(ElkDescriptions);
                    break;
                case DefaultValues.Panther:
                    Movement = 5;
                    StaminaMultiplier = 5;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(PantherNames));
                    KeyWords.Add("Panther");
                    SentenceDescription = "panther";
                    mountDescription = RandomValue(PantherDescriptions);
                    break;
                case DefaultValues.Griffin:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    MaxRiders = 1;
                    KeyWords.Add(RandomValue(GriffinNames));
                    KeyWords.Add("Griffin");
                    SentenceDescription = "griffin";
                    mountDescription = RandomValue(GriffinDescriptions);
                    break;
            }

            ExamineDescription = mountDescription.ExamineDescription;
            LookDescription = mountDescription.LookDescription;
            ShortDescription = mountDescription.ShortDescription;
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

        private T RandomValue<T>(List<T> list)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(list.Count);
            return list[pos];
        }

        private record MountDescription
        {
            public MountDescription(string examineDescription, string lookDescription, string shortDescription)
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
