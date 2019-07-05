using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob
{
    public class Mount : MobileObject, IMount
    {
        public int Movement { get; set; } = 2;
        public int StaminaMultiplier { get; set; } = 10;
        public SummonType TypeOfSummon { get; set; } = SummonType.Call;
        public int MaxRiders { get; set; } = 1;
        public List<IMobileObject> Riders { get; set; } = new List<IMobileObject>();

        #region Names
        private List<string> Horse = new List<string>() { "Kisses", "Tang", "Fleetbolt", "Shadows", "Zephyr", "Snapdraon", "Sugar Blossom" };
        private List<string> Unicorn = new List<string>() { "Uni", "Arryn", "Julius" "Wrynn", "Lancelot", "Linus", "Mawu" };
        private List<string> Nightmare = new List<string>() { "Orkiz", "Brog'drallin", "Megmes", "Xeglomoth", "Uz'gonnath", "Tukillian," "Ezzorith", "Igdruzoth", "Boggun", "Xugthozog" };
        private List<string> Elephant = new List<string>() { "Skitters", "Hanno", "Jumbo", "Peanut", "Tiny", "Tusks" };
        private List<string> Elk = new List<string>() { "" };
        private List<string> Panther = new List<string>() { "" };
        private List<string> Gryphon = new List<string>() { "" };

        #endregion Names


        public void LoadDefaultValues(DefaultValues defaultValues)
        {
            switch (defaultValues)
            {
                case DefaultValues.Horse:
                    Movement = 2;
                    StaminaMultiplier = 10;
                    TypeOfSummon = SummonType.Call;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Horse));
                    break;
                case DefaultValues.Unicorn:
                    Movement = 2;
                    StaminaMultiplier = 12;
                    TypeOfSummon = SummonType.Call;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Unicorn));
                    break;
                case DefaultValues.Nightmare:
                    Movement = 3;
                    StaminaMultiplier = 15;
                    TypeOfSummon = SummonType.Summon;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Nightmare));
                    break;
                case DefaultValues.Elephant:
                    Movement = 1;
                    StaminaMultiplier = 20;
                    TypeOfSummon = SummonType.Call;
                    MaxRiders = 5;
                    KeyWords.Add(RandomName(Elephant));
                    break;
                case DefaultValues.Elk:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    TypeOfSummon = SummonType.Call;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Elk));
                    break;
                case DefaultValues.Panther:
                    Movement = 5;
                    StaminaMultiplier = 5;
                    TypeOfSummon = SummonType.Call;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Panther));
                    break;
                case DefaultValues.Gryphon:
                    Movement = 3;
                    StaminaMultiplier = 7;
                    TypeOfSummon = SummonType.Summon;
                    MaxRiders = 1;
                    KeyWords.Add(RandomName(Gryphon));
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
            Gryphon

        }

        public enum SummonType
        {
            Summon,
            Call,
            None
        }

        private string RandomName(List<string> names)
        {
            int pos = GlobalReference.GlobalValues.Random.Next(names.Count);
            return names[pos];
        }
    }
}
