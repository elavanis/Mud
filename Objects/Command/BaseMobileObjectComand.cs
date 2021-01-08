using Objects.Command.Interface;
using Objects.Mob.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Mob.MobileObject;

namespace Objects.Command
{
    public abstract class BaseMobileObjectComand
    {
        public string CommandName { get; }
        public HashSet<CharacterPosition> AllowedCharacterPositions { get; }

        public BaseMobileObjectComand(string commandName, HashSet<CharacterPosition> characterPositions)
        {
            CommandName = commandName;
            AllowedCharacterPositions = characterPositions;
        }

        public BaseMobileObjectComand(string commandName, ShortCutCharPositions shortCutCharPositions)
        {
            HashSet<CharacterPosition> characterPositions = new HashSet<CharacterPosition>();
            switch (shortCutCharPositions)
            {
                case ShortCutCharPositions.Any:
                    characterPositions.Add(CharacterPosition.Mounted);
                    characterPositions.Add(CharacterPosition.Relax);
                    characterPositions.Add(CharacterPosition.Sit);
                    characterPositions.Add(CharacterPosition.Sleep);
                    characterPositions.Add(CharacterPosition.Stand);
                    break;

                case ShortCutCharPositions.Awake:
                    characterPositions.Add(CharacterPosition.Mounted);
                    characterPositions.Add(CharacterPosition.Relax);
                    characterPositions.Add(CharacterPosition.Sit);
                    characterPositions.Add(CharacterPosition.Stand);
                    break;
                case ShortCutCharPositions.Standing:
                    characterPositions.Add(CharacterPosition.Mounted);
                    characterPositions.Add(CharacterPosition.Stand);
                    break;
            }

            CommandName = commandName;
            AllowedCharacterPositions = characterPositions;
        }

        public enum ShortCutCharPositions
        {
            Any,
            Awake,
            Standing

        }

        public IResult PerfomCommand(IMobileObject performer, ICommand command)
        {
            if (!AllowedCharacterPositions.Contains(performer.Position))
            {
                return new Result($"You can not {CommandName} while {performer.Position}.", true);
            }

            return null;
        }
    }
}
