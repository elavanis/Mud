using Objects.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ServerTelnetCommunication
{
    public static class ConnectionAccessManager
    {
        private static Dictionary<IPAddress, List<DateTime>> failedLogins = new Dictionary<IPAddress, List<DateTime>>();
        private static HashSet<IPAddress> bannedIps = new HashSet<IPAddress>();

        static ConnectionAccessManager()
        {
            foreach (IPAddress address in GlobalReference.GlobalValues.Settings.BannedIps)
            {
                bannedIps.Add(address);
            }
        }

        public static void AddFailedLogin(IPAddress address)
        {
            lock (failedLogins)
            {
                if (!failedLogins.ContainsKey(address))
                {
                    failedLogins.Add(address, new List<DateTime>());
                }

                failedLogins[address].Add(DateTime.UtcNow);
            }
        }

        public static bool CanLogin(IPAddress address)
        {
            lock (failedLogins)
            {
                if (bannedIps.Contains(address))
                {
                    return false;
                }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                if (failedLogins.TryGetValue(address, out List<DateTime> failedDateTimes))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                {
                    failedDateTimes = RemoveOldItems(failedDateTimes);
                    if (failedDateTimes.Count >= 5)
                    {
                        return false;
                    }
                }

                return true;  //not in the list of failed logins or less than 5 failures in 5 minutes
            }
        }

        public static void FlushOldFailedAttempts()
        {
            lock (failedLogins)
            {
                List<IPAddress> ips = failedLogins.Keys.ToList();

                foreach (IPAddress address in ips)
                {
                    List<DateTime> failedDateTimes;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (failedLogins.TryGetValue(address, out failedDateTimes))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    {
                        failedDateTimes = RemoveOldItems(failedDateTimes);
                        if (failedDateTimes.Count == 0)
                        {
                            failedLogins.Remove(address);
                        }
                    }
                }
            }
        }

        private static List<DateTime> RemoveOldItems(List<DateTime> failedDateTimes)
        {
            for (int i = failedDateTimes.Count; i > 0; i--)
            {
                if (DateTime.UtcNow.Subtract(failedDateTimes[i - 1]).TotalMinutes > 4)
                {
                    failedDateTimes.RemoveAt(i - 1);
                }
            }

            return failedDateTimes;
        }
    }
}
