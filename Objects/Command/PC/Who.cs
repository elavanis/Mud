using Objects.Command.Interface;
using Objects.Global;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects.Command.PC
{
    public class Who : IMobileObjectCommand
    {
        public IResult Instructions { get; } = new Result("Who", true);

        public IEnumerable<string> CommandTrigger { get; } = new List<string>() { "Who" };

        public IResult PerformCommand(IMobileObject performer, ICommand command)
        {
            int longestName = 0;
            List<IPlayerCharacter> gods = new List<IPlayerCharacter>();
            List<IPlayerCharacter> players = new List<IPlayerCharacter>();

            SortAndFindLongest(ref longestName, gods, players);

            StringBuilder stringBuilder = new StringBuilder();

            if (gods.Count > 0)
            {
                WriteSection("Gods", longestName, gods, stringBuilder);
                stringBuilder.AppendLine(); //better spacing
            }

            if (players.Count > 0)
            {
                WriteSection("Players", longestName, players, stringBuilder);
            }

            return new Result(stringBuilder.ToString().Trim(), true);
        }

        private static void WriteSection(string heading, int longestName, List<IPlayerCharacter> playerCharacters, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(heading);
            stringBuilder.AppendLine($"Level  {"Name".PadRight(longestName)}  Title");

            foreach (IPlayerCharacter pc in playerCharacters)
            {
                BuildPcLine(longestName, stringBuilder, pc);
            }
        }

        private static void BuildPcLine(int longestName, StringBuilder stringBuilder, IPlayerCharacter pc)
        {
            stringBuilder.AppendLine($"{pc.Level.ToString().PadRight(5)}  {pc.Name.PadRight(longestName)}  {pc.Title ?? ""}");
        }

        private void SortAndFindLongest(ref int longestName, List<IPlayerCharacter> gods, List<IPlayerCharacter> players)
        {
            foreach (IPlayerCharacter pc in GlobalReference.GlobalValues.World.CurrentPlayers)
            {
                if (pc.God)
                {
                    gods.Add(pc);
                }
                else
                {
                    players.Add(pc);
                }

                longestName = Math.Max(longestName, pc.Name.Length);
            }
        }
    }
}
