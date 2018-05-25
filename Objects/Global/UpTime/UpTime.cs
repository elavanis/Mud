using Objects.Global.UpTime.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.UpTime
{
    public class UpTime : IUpTime
    {
        public string FormatedUpTime(DateTime start)
        {
            return FormatedUpTime(TimeUp(start));
        }
        private static TimeSpan TimeUp(DateTime start)
        {
            return DateTime.Now.Subtract(start);
        }

        private static string FormatedUpTime(TimeSpan elapsedTime)
        {
            return String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
               elapsedTime.Days.ToString().PadLeft(2, ' '),
               elapsedTime.Hours.ToString().PadLeft(2, ' '),
               elapsedTime.Minutes.ToString().PadLeft(2, ' '),
               elapsedTime.Seconds.ToString().PadLeft(2, ' '));
        }
    }
}
