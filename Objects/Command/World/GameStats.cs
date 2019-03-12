using Objects.Command.World.Interface;
using Objects.Global;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.World
{
    public class GameStats : IGameStats
    {
        public string GenerateGameStats()
        {
            StringBuilder strBldr = new StringBuilder();
            float memoryConsumption = Process.GetCurrentProcess().WorkingSet64 / (1024f * 1024f);
            memoryConsumption = (float)Math.Round(memoryConsumption, 2);

            strBldr.AppendLine(string.Format("Memory: {0} Megs", memoryConsumption));
            strBldr.AppendLine(string.Format("UpTime: {0}", GlobalReference.GlobalValues.UpTime.FormatedUpTime(GlobalReference.GlobalValues.StartTime)));
            strBldr.AppendLine(GlobalReference.GlobalValues.TickTimes.Times);

            List<ZoneInfo> zoneInfo = new List<ZoneInfo>();

            foreach (IZone zone in GlobalReference.GlobalValues.World.Zones.Values.OrderBy(z => z.Id))
            {
                ZoneInfo info = new ZoneInfo();
                info.ZoneId = zone.Id;
                info.ZoneName = zone.Name;
                info.Rooms = zone.Rooms.Values.Count;
                foreach (IRoom room in zone.Rooms.Values)
                {
                    info.Npc += room.NonPlayerCharacters.Count;
                    info.Pc += room.PlayerCharacters.Count;
                    info.Items += room.Items.Count;
                }
                zoneInfo.Add(info);
            }

            int zoneIdPadding = 0;
            int zoneNamePadding = 0;
            int npcPadding = 0;
            int itemsPadding = 0;
            int pcPaddig = 0;
            int roomsPadding = 0;

            foreach (ZoneInfo info in zoneInfo)
            {
                zoneIdPadding = Math.Max(zoneIdPadding, info.ZoneId.ToString().Length);
                zoneNamePadding = Math.Max(zoneNamePadding, info.ZoneName.Length);
                npcPadding = Math.Max(npcPadding, info.Npc.ToString().Length);
                itemsPadding = Math.Max(itemsPadding, info.Items.ToString().Length);
                pcPaddig = Math.Max(pcPaddig, info.Pc.ToString().Length);
                roomsPadding = Math.Max(roomsPadding, info.Rooms.ToString().Length);

                strBldr.AppendLine(string.Format("Id:{4} - {0} - Rooms:{5} - Mobs:{1} - Items:{2} - Players:{3}", info.ZoneName.PadRight(zoneNamePadding)
                                                                                                   , info.Npc.ToString().PadRight(npcPadding)
                                                                                                   , info.Items.ToString().PadRight(itemsPadding)
                                                                                                   , info.Pc.ToString().PadRight(pcPaddig)
                                                                                                   , info.ZoneId.ToString().PadRight(zoneIdPadding)
                                                                                                   , info.Rooms.ToString().PadRight(roomsPadding)));
            }

            return strBldr.ToString().Trim();
        }

        private class ZoneInfo
        {
            public int ZoneId { get; set; }
            public string ZoneName { get; set; }
            public int Rooms { get; set; }
            public int Npc { get; set; }
            public int Pc { get; set; }
            public int Items { get; set; }
        }
    }
}
