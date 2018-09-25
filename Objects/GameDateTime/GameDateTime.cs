using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.GameDateTime
{
    public class GameDateTime
    {
        private static DateTime Start { get; } = new DateTime(2015, 11, 7);
        private static DateTime ZeroTime { get; } = new DateTime(1, 1, 1);
        private static int YearCount { get; } = Enum.GetValues(typeof(Year)).Length;
        private static int MonthCount { get; } = Enum.GetValues(typeof(Month)).Length;
        private static int DayCount { get; } = Enum.GetValues(typeof(Day)).Length;

        public Year YearName
        {
            get
            {
                int localYear = (Year - 1) % YearCount;

                return (Objects.GameDateTime.Year)localYear;
            }
        }
        public Month MonthName
        {
            get
            {
                int localMonth = (Month - 1) % MonthCount;

                return (Objects.GameDateTime.Month)localMonth;
            }
        }
        public Day DayName
        {
            get
            {
                int localDay = (Day - 1) % DayCount;

                return (Objects.GameDateTime.Day)localDay;
            }
        }

        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Hour { get; private set; }
        public int Minute { get; private set; }

        public GameDateTime(DateTime now)
        {
            long totalInGameSecondsSinceBegining = (long)now.ToUniversalTime().Subtract(Start.ToUniversalTime()).TotalSeconds * 60;

            Year = CalculateYear(ref totalInGameSecondsSinceBegining) + 1;
            Month = CalculateMonth(ref totalInGameSecondsSinceBegining) + 1;
            Day = CalculateDay(ref totalInGameSecondsSinceBegining) + 1;
            Hour = CalculateHour(ref totalInGameSecondsSinceBegining) + 1;
            Minute = CalculateMinute(ref totalInGameSecondsSinceBegining);
        }

        #region Private Calculations
        private static int CalculateYear(ref long totalSeconds)
        {
            int year = (int)(totalSeconds / secondsInYear());
            totalSeconds = totalSeconds - (year * secondsInYear());

            return year;
        }
        private static int CalculateMonth(ref long totalSeconds)
        {
            int month = (int)(totalSeconds / secondsInMonth());
            totalSeconds = totalSeconds - (month * secondsInMonth());

            return month;
        }
        private static int CalculateDay(ref long totalSeconds)
        {
            int day = (int)(totalSeconds / secondsInDay());
            totalSeconds = totalSeconds - (day * secondsInDay());

            return day;
        }
        private static int CalculateHour(ref long totalSeconds)
        {
            int hour = (int)(totalSeconds / secondsInHour());
            totalSeconds = totalSeconds - (hour * secondsInHour());

            return hour;
        }
        private static int CalculateMinute(ref long totalSeconds)
        {
            int minute = (int)(totalSeconds / secondsInMinute());
            totalSeconds = totalSeconds - (minute * secondsInMinute());

            return minute;
        }

        private static long secondsInMinute()
        {
            return 60;
        }
        private static long secondsInHour()
        {
            return 60 * secondsInMinute();
        }
        private static long secondsInDay()
        {
            return 24 * secondsInHour();
        }
        private static long secondsInMonth()
        {
            return 5 * DayCount * secondsInDay();  //5 weeks of 6 days
        }
        private static long secondsInYear()
        {
            return 12 * secondsInMonth();
        }
        #endregion Private Calculations
    }

    public enum Day
    {
        Life,
        Earth,
        Fire,
        Air,
        Water,
        Death
    }

    public enum Month
    {
        Griffin,
        Vampire,
        Lich,
        Unicorn,
        Gargoyle,
        Kobold,
        Mermaid,
        Minotaur,
        Wyvern,
        Cockatrice,
        Dragon,
        Ghost
    }

    public enum Year
    {
        Charon,
        Calypso
    }
}
