using Objects.Global.GameDateTime.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Global.GameDateTime
{
    public class GameDateTime : IGameDateTime
    {
        public static DateTime Start { get; } = new DateTime(2015, 11, 7);
        public static DateTime ZeroTime { get; } = new DateTime(1, 1, 1);

        private ITime _time;
        public GameDateTime(ITime time)
        {
            _time = time;
        }

        public DateTime InGameDateTime
        {
            get
            {
                return GetDateTime();
            }
        }

        public string InGameFormatedDateTime
        {
            get
            {
                return BuildFormatedDateTime();
            }
        }

        public string BuildFormatedDateTime()
        {
            DateTime inGameDateTime = GetDateTime();
            return BuildFormatedDateTime(inGameDateTime);
        }

        public string BuildFormatedDateTime(DateTime dateTime)
        {
            //string formatedDateTime = formatDateTime(dateTime).Trim();
            //formatedDateTime = GlobalReference.GlobalValues.TagWrapper.WrapInTag(formatedDateTime, TagType.Info);
            //return formatedDateTime;

            return formatDateTime(dateTime).Trim();
        }

        private DateTime CurrentDateTime
        {
            get
            {
                return _time.CurrentDateTime;
            }
        }

        public DateTime GetDateTime(DateTime dateTime)
        {
            TimeSpan difference = dateTime.Subtract(Start);
            TimeSpan spedUpTime = TimeSpan.FromSeconds(difference.TotalSeconds * 60);
            DateTime gameTime = ZeroTime + spedUpTime;
            return gameTime;
        }

        private DateTime GetDateTime()
        {
            return GetDateTime(CurrentDateTime);
        }

        private string formatDateTime(DateTime inGameDateTime)
        {
            string day = Day(inGameDateTime.DayOfWeek);
            string month = Month(inGameDateTime.Month);
            string year = Year(inGameDateTime.Year % YearNames.Count);

            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendLine(inGameDateTime.ToString("MM/dd/yyyy hh:mm tt"));
            strBldr.AppendLine("Month: " + month);
            strBldr.AppendLine("Day: " + day);
            strBldr.AppendLine("Year: " + year);
            return strBldr.ToString();
        }

        private string Day(DayOfWeek dayOfWeek)
        {
            return DayOfWeek[(int)dayOfWeek];
        }

        private string Month(int month)
        {
            return MonthNames[month - 1];
        }

        private string Year(int year)
        {
            return YearNames[year];
        }

        private List<string> DayOfWeek { get; } = CreateDays();
        private static List<string> CreateDays()
        {
            List<string> dayOfWeek = new List<string>();
            dayOfWeek.Add("Life");
            dayOfWeek.Add("Fire");
            dayOfWeek.Add("Air");
            dayOfWeek.Add("Earth");
            dayOfWeek.Add("Thunder");
            dayOfWeek.Add("Water");
            dayOfWeek.Add("Death");
            return dayOfWeek;
        }


        private List<string> MonthNames { get; } = CreateMonths();
        private static List<string> CreateMonths()
        {
            List<string> month = new List<string>();
            month.Add("Griffin");
            month.Add("Vampire");
            month.Add("Lich");
            month.Add("Unicorn");
            month.Add("Gargoyle");
            month.Add("Kobold");
            month.Add("Mermaid");
            month.Add("Minotaur");
            month.Add("Wyvern");
            month.Add("Cockatrice");
            month.Add("Dragon");
            month.Add("Ghost");
            return month;
        }

        public List<string> YearNames { get; } = CreateYears();
        private static List<string> CreateYears()
        {
            //TODO Make this load the God player character names

            List<string> year = new List<string>();
            //year.Add("Nehzno");
            //year.Add("Makkari");
            //year.Add("Tyrannus");
            //year.Add("DarkStar");
            //year.Add("Paibok");
            //year.Add("NorthStar");
            year.Add("Charon");
            //year.Add("Iiyana");
            //year.Add("Daken");
            //year.Add("Hepzibah");
            //year.Add("Zuras");
            //year.Add("Morlun");
            year.Add("Calypso");
            //year.Add("Araña");
            //year.Add("Shi'Ar");
            //year.Add("Ozymandias");

            return year;
        }
    }
}
