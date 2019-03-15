using Objects.Global;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Mob.SpecificNPC.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Objects.Mob.SpecificNPC
{
    public class Elemental : NonPlayerCharacter, IElemental
    {
        //Earth <-> Air 
        //Fire <-> Water
        [ExcludeFromCodeCoverage]
        public ElementType ElementType { get; set; }
        [ExcludeFromCodeCoverage]
        public WeatherType WeatherType { get; set; }
        [ExcludeFromCodeCoverage]
        public int WeatherTrigger { get; set; }
        [ExcludeFromCodeCoverage]
        public WeatherDirection WeatherDirection { get; set; }

        private int RoundTickCounter { get; set; } = 0;

        [ExcludeFromCodeCoverage]
        public Elemental()
        {

        }

        public Elemental(ElementType elementType)
        {
            SetElement(elementType);
            Personalities.Add(new Personality.Personalities.Elemental());
            TypeOfMob = MobType.Other;
        }

        public void SetElement(ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.Earth:
                    SetEarth();
                    break;
                case ElementType.Air:
                    SetAir();
                    break;
                case ElementType.Fire:
                    SetFire();
                    break;
                case ElementType.Water:
                    SetWater();
                    break;
            }

            KeyWords.Add("elemental");
        }

        public void ProcessElementalTick()
        {
            RoundTickCounter++;
            if (RoundTickCounter % 5 == 0) //only process every 5th round so we don't level up or down ever tick
            {
                LevelChange levelChange = GetLevelChange();

                if (levelChange == LevelChange.Up)
                {
                    if (Level < GlobalReference.GlobalValues.Settings.MaxLevel)
                    {
                        Level++;
                        ResetStats();
                        RemoveEquipment();
                        FinishLoad();
                        ITranslationMessage translationMessage = new TranslationMessage($"The {KeyWords[0]} elemental grows stronger.");
                        GlobalReference.GlobalValues.Notify.Room(this, null, Room, translationMessage, null, true);
                    }
                }
                else if (levelChange == LevelChange.Down)
                {
                    if (Level > 1)
                    {
                        Level--;
                        ResetStats();
                        RemoveEquipment();
                        FinishLoad();
                        ITranslationMessage translationMessage = new TranslationMessage($"The {KeyWords[0]} elemental grows weaker.");
                        GlobalReference.GlobalValues.Notify.Room(this, null, Room, translationMessage, null, true);
                    }
                    else
                    {
                        ITranslationMessage translationMessage = new TranslationMessage($"The {KeyWords[0]} elemental has grown so weak it can no longer hold its form in this realm and slowly fades away.");
                        GlobalReference.GlobalValues.Notify.Room(this, null, Room, translationMessage, null, true);
                        Room.RemoveMobileObjectFromRoom(this);
                    }
                }
            }
        }

        private void RemoveEquipment()
        {
            _npcEquipment?.Clear();
        }

        private void ResetStats()
        {
            Money = 0;
            StrengthStat = 0;
            DexterityStat = 0;
            ConstitutionStat = 0;
            IntelligenceStat = 0;
            WisdomStat = 0;
            CharismaStat = 0;
        }

        private LevelChange GetLevelChange()
        {
            int currentWeather = GetCurrentWeather();

            switch (WeatherDirection)
            {
                case WeatherDirection.High:
                    if (currentWeather >= 75)
                    {
                        return LevelChange.Up;
                    }
                    else if (currentWeather < 50)
                    {
                        return LevelChange.Down;
                    }
                    break;
                case WeatherDirection.Low:
                    if (currentWeather <= 25)
                    {
                        return LevelChange.Up;
                    }
                    else if (currentWeather > 50)
                    {
                        return LevelChange.Down;
                    }
                    break;
            }

            return LevelChange.None;
        }

        private int GetCurrentWeather()
        {
            int currentWeather = 0;
            switch (WeatherType)
            {
                case WeatherType.Precipitation:
                    currentWeather = GlobalReference.GlobalValues.World.Precipitation;
                    break;
                case WeatherType.Wind:
                    currentWeather = GlobalReference.GlobalValues.World.WindSpeed;
                    break;
            }

            return currentWeather;
        }

        private void SetEarth()
        {
            ElementType = ElementType.Earth;
            KeyWords.Add("earth");
            LookDescription = "The lumbering earth elemental looks slow but moves surprising fast for its size.";
            ExamineDescription = "Trace metals run through rock in the earth elemental's body giving the impression of veins.";
            ShortDescription = "A large earth elemental stands looming above you.";
            SentenceDescription = "earth elemental";

            WeatherType = WeatherType.Precipitation;
            WeatherTrigger = 25;
            WeatherDirection = WeatherDirection.Low;

            Race.Bludgeon = 1.5M;
            Race.Pierce = 1.5M;
            Race.Slash = 1.5M;

            Race.Poison = 0M;
        }

        private void SetAir()
        {
            ElementType = ElementType.Air;
            KeyWords.Add("air");
            LookDescription = "The air elemental rapidly increases in size and then goes back to normal.  This cycle repeats itself over and over.";
            ExamineDescription = "Thick clouds of swirling wind obscure flashes of light in what appears to be a small thunderstorm.";
            ShortDescription = "The air elemental swirls around knocking over loose items.";
            SentenceDescription = "air elemental";

            WeatherType = WeatherType.Wind;
            WeatherTrigger = 75;
            WeatherDirection = WeatherDirection.High;

            Race.Lightning = 1.5M;
            Race.Thunder = 1.5M;
            Race.Bludgeon = 1.5M;
            Race.Pierce = 1.5M;
            Race.Slash = 1.5M;

            Race.Poison = 0M;
        }

        private void SetFire()
        {
            ElementType = ElementType.Fire;
            KeyWords.Add("fire");
            LookDescription = "The fire elemental walks around leaving a scorched marks on the ground.";
            ExamineDescription = "Searing heat cause flammable items in the room to smolder.";
            ShortDescription = "A burning fire elemental glows red hot filling the area with smoke.";
            SentenceDescription = "fire elemental";

            WeatherType = WeatherType.Precipitation;
            WeatherTrigger = 25;
            WeatherDirection = WeatherDirection.Low;

            Race.Bludgeon = 1.5M;
            Race.Pierce = 1.5M;
            Race.Slash = 1.5M;

            Race.Fire = 0M;
            Race.Poison = 0M;
        }

        private void SetWater()
        {
            ElementType = ElementType.Water;
            KeyWords.Add("water");
            LookDescription = "A water elemental flows around leaving a trail of water behind.";
            ExamineDescription = "The innards of the water elemental churn and flow distorting the image of what is behind it.";
            ShortDescription = "A turbulent water elemental drips water everywhere.";
            SentenceDescription = "water elemental";

            WeatherType = WeatherType.Precipitation;
            WeatherTrigger = 75;
            WeatherDirection = WeatherDirection.High;

            Race.Acid = 1.5M;
            Race.Bludgeon = 1.5M;
            Race.Pierce = 1.5M;
            Race.Slash = 1.5M;

            Race.Poison = 0M;
        }
    }



    public enum ElementType
    {
        Earth, Air,
        Fire, Water
    }

    public enum WeatherType
    {
        Precipitation,
        Wind
    }
    public enum WeatherDirection
    {
        Low,
        High
    }

    public enum LevelChange
    {
        Up,
        None,
        Down
    }
}
