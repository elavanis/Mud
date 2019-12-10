using Objects.Global.Map.Interface;
using Objects.Language;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using System.IO;
using static Shared.FileIO.Interface.CachedThings.FileExits;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Global.Map
{
    public class Map : IMap
    {
        private static Dictionary<string, string> _mapPositionCache = new Dictionary<string, string>();

        public void SendMapPosition(IMobileObject mob)
        {
            if (GlobalReference.GlobalValues.Settings.SendMapPosition
                && GlobalReference.GlobalValues.CanMobDoSomething.SeeDueToLight(mob))  //only send map data if mobs can see
            {
                IRoom room = mob.Room;

                string formatedRespone = FindRoomPosition(room);

                if (formatedRespone != null)
                {
                    GlobalReference.GlobalValues.Notify.Mob(mob, new TranslationMessage(formatedRespone, TagType.Map));
                }
            }
        }

        private static string FindRoomPosition(IRoom room)
        {
            string roomKey = room.Zone + "|" + room.Id;

            string result;

            _mapPositionCache.TryGetValue(roomKey, out result);
            if (result == null)
            {
                string file = Path.Combine(GlobalReference.GlobalValues.Settings.AssetsDirectory, "Maps", room.Zone + ".MapConversion");
                if (GlobalReference.GlobalValues.FileIO.Exists(file))
                {
                    foreach (string line in GlobalReference.GlobalValues.FileIO.ReadLines(file))
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string[] splitLine = line.Split('|');
                            if (splitLine[0] == room.Id.ToString())
                            {
                                result = $"{room.Zone}|{splitLine[1]}|{splitLine[2]}|{splitLine[3]}";
                                _mapPositionCache.Add(roomKey, result);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
