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

        public Day DayName { get; }
        public Month MonthName { get; }
        public Year YearName { get; }

        public int Day { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
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

        private int CalculateYear(ref long totalSeconds)
        {
            int year = (int)(totalSeconds / secondsInYear());
            totalSeconds = totalSeconds - (year * secondsInYear());

            return year;
        }

        private int CalculateMonth(ref long totalSeconds)
        {
            int month = (int)(totalSeconds / secondsInMonth());
            totalSeconds = totalSeconds - (month * secondsInMonth());

            return month;
        }

        private int CalculateDay(ref long totalSeconds)
        {
            int day = (int)(totalSeconds / secondsInDay());
            totalSeconds = totalSeconds - (day * secondsInDay());

            return day;
        }

        private int CalculateHour(ref long totalSeconds)
        {
            int hour = (int)(totalSeconds / secondsInHour());
            totalSeconds = totalSeconds - (hour * secondsInHour());

            return hour;
        }

        private int CalculateMinute(ref long totalSeconds)
        {
            int minute = (int)(totalSeconds / secondsInMinute());
            totalSeconds = totalSeconds - (minute * secondsInMinute());

            return minute;
        }

        private long secondsInMinute()
        {
            return 60;
        }
        private long secondsInHour()
        {
            return 60 * secondsInMinute();
        }
        private long secondsInDay()
        {
            return 24 * secondsInHour();
        }
        private long secondsInMonth()
        {
            return 5 * DayCount * secondsInDay();  //5 weeks of 6 days
        }
        private long secondsInYear()
        {
            return 12 * secondsInMonth();
        }
    }

    public enum Day
    {
        Life,
        Fire,
        Air,
        Earth,
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
