using Objects.Global;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Mob.SpecificNPC
{
    public class Elemental : NonPlayerCharacter
    {
        //Earth <-> Air 
        //Fire <-> Water
        public ElementType ElementType { get; set; }

        public WeatherType WeatherType { get; set; }
        public int WeatherTrigger { get; set; }
        public WeatherDirection WeatherDirection { get; set; }

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
            LevelChange levelChange = GetLevelChange();

            if (levelChange == LevelChange.Up)
            {
                if (Level < GlobalReference.GlobalValues.Settings.MaxLevel)
                {
                    Level++;
                    ResetStats();
                    FinishLoad();
                }
            }
            else if (levelChange == LevelChange.Down)
            {
                if (Level > 1)
                {
                    Level--;
                    ResetStats();
                    FinishLoad();
                }
                else
                {
                    ITranslationMessage translationMessage = new TranslationMessage($"The {KeyWords[0]} elemental has grown so weak it can no longer hold its form in this realm and slowly fades away.");
                    GlobalReference.GlobalValues.Notify.Room(this, null, Room, translationMessage, null, true);
                    Room.RemoveMobileObjectFromRoom(this);
                }
            }
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
            KeyWords.Add("earth");
            LookDescription = "";
            ExamineDescription = "";
            ShortDescription = "";
            SentenceDescription = "earth elemental";

            WeatherType = WeatherType.Wind;
            WeatherTrigger = 25;
            WeatherDirection = WeatherDirection.Low;
        }

        private void SetAir()
        {
            KeyWords.Add("air");
            LookDescription = "The air elemental rapidly increases in size and then goes back to normal.  This cycle repeats itself over and over.";
            ExamineDescription = "Thick clouds of swirling wind obscure flashes of light in what appears to be a small thunderstorm.";
            ShortDescription = "The air elemental swirls around knocking over loose items.";
            SentenceDescription = "air elemental";

            WeatherType = WeatherType.Wind;
            WeatherTrigger = 75;
            WeatherDirection = WeatherDirection.High;
        }

        private void SetFire()
        {
            KeyWords.Add("fire");
            LookDescription = "The fire elemental walks around leaving a scorched marks on the ground.";
            ExamineDescription = "Searing heat cause flammable items in the room to smolder.";
            ShortDescription = "A burning fire elemental glows red hot filling the area with smoke.";
            SentenceDescription = "fire elemental";

            WeatherType = WeatherType.Precipitation;
            WeatherTrigger = 25;
            WeatherDirection = WeatherDirection.Low;
        }

        private void SetWater()
        {
            KeyWords.Add("water");
            LookDescription = "A water elemental flows around leaving a trail of water behind.";
            ExamineDescription = "The innards of the water elemental churn and flow distorting the image of what is behind it.";
            ShortDescription = "A turbulent water elemental drips water everywhere.";
            SentenceDescription = "water elemental";

            WeatherType = WeatherType.Precipitation;
            WeatherTrigger = 75;
            WeatherDirection = WeatherDirection.High;
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
