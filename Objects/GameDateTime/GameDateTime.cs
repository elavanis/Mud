using Objects.GameDateTime.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.GameDateTime
{
    public class GameDateTime : IGameDateTime
    {
        private static DateTime Start { get; } = new DateTime(2015, 11, 7, 16, 43, 0, DateTimeKind.Utc);
        //private static DateTime Start { get; } = new DateTime(2015, 11, 7, 16, 43, 0, DateTimeKind.Utc);
        private static int YearCount { get; } = Enum.GetValues(typeof(Years)).Length;
        private static int MonthCount { get; } = Enum.GetValues(typeof(Months)).Length;
        private static int DayCount { get; } = Enum.GetValues(typeof(Days)).Length;
        private static int weeksInMonth { get; } = 5;

        public Years YearName
        {
            get
            {
                int localYear = (Year - 1) % YearCount;

                return (Years)localYear;
            }
        }
        public Months MonthName
        {
            get
            {
                int localMonth = (Month - 1) % MonthCount;

                return (Months)localMonth;
            }
        }
        public Days DayName
        {
            get
            {
                int localDay = (Day - 1) % DayCount;

                return (Days)localDay;
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }


        public GameDateTime()
        {

        }
        public GameDateTime(DateTime now)
        {
            long totalInGameSecondsSinceBegining = (long)now.ToUniversalTime().Subtract(Start).TotalSeconds * 60;

            Year = CalculateYear(ref totalInGameSecondsSinceBegining) + 1;
            Month = CalculateMonth(ref totalInGameSecondsSinceBegining) + 1;
            Day = CalculateDay(ref totalInGameSecondsSinceBegining) + 1;
            Hour = CalculateHour(ref totalInGameSecondsSinceBegining);
            Minute = CalculateMinute(ref totalInGameSecondsSinceBegining);
        }


        public override string ToString()
        {
            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendLine($"{Pad(Month)}/{Pad(Day)}/{Pad(Year)} {Pad(Hour)}:{Pad(Minute)}:00");
            strBldr.AppendLine("Month: " + MonthName);
            strBldr.AppendLine("Day: " + DayName);
            strBldr.Append("Year: " + YearName);

            return strBldr.ToString();
        }

        public int CompareTo(object obj)
        {
            IGameDateTime gameDateTime = obj as IGameDateTime;

            if (gameDateTime == null)
            {
                throw new Exception("Unable to compare to non game date time object.");
            }

            if (Year != gameDateTime.Year)
            {
                return Year.CompareTo(gameDateTime.Year);
            }
            else if (Month != gameDateTime.Month)
            {
                return Month.CompareTo(gameDateTime.Month);
            }
            else if (Day != gameDateTime.Day)
            {
                return Day.CompareTo(gameDateTime.Day);
            }
            else if (Hour != gameDateTime.Hour)
            {
                return Hour.CompareTo(gameDateTime.Hour);
            }
            else if (Minute != gameDateTime.Minute)
            {
                return Minute.CompareTo(gameDateTime.Minute);
            }

            return 0;
        }

        public static bool operator <(GameDateTime gameDateTime1, IGameDateTime gameDateTime2)
        {
            return gameDateTime1.CompareTo(gameDateTime2) < 0;
        }

        public static bool operator >(GameDateTime gameDateTime1, IGameDateTime gameDateTime2)
        {
            return gameDateTime1.CompareTo(gameDateTime2) > 0;
        }


        private string Pad(int i)
        {
            return i.ToString().PadLeft(2, '0');
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
            return weeksInMonth * DayCount * secondsInDay();  //5 weeks of 6 days
        }
        private static long secondsInYear()
        {
            return 12 * secondsInMonth();
        }

        IGameDateTime IGameDateTime.AddDays(int days)
        {
            Day += days;
            int daysInMonth = weeksInMonth * DayCount;

            if (Day > daysInMonth)
            {
                Month += Day / (daysInMonth);
                Day = Day % daysInMonth;

                if (Month > MonthCount)
                {
                    Year += Month / MonthCount;
                    Month = Month % MonthCount;
                }
            }

            return this;
        }

        #endregion Private Calculations
    }

    public enum Days
    {
        Life,
        Earth,
        Fire,
        Air,
        Water,
        Death
    }

    public enum Months
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

    public enum Years
    {
        Charon,
        Calypso
    }
}