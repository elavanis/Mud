using Objects.Personality.Interface;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Global;

namespace Objects.Personality.Personalities
{
    /// <summary>
    /// Will aid npc when engaged in combat with a pc
    /// </summary>
    public class Guardian : IPersonality
    {
        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                //see if there are both npc and pc in the room
                if (npc.Room.PlayerCharacters.Count > 0
                    && npc.Room.NonPlayerCharacters.Count > 0)
                {
                    //check to make sure this npc is not in combat, don't want to start 2 fights
                    if (!GlobalReference.GlobalValues.Engine.Combat.IsInCombat(npc))
                    {
                        //go through each pc in the room
                        foreach (IPlayerCharacter pc in npc.Room.PlayerCharacters)
                        {
                            //see if they are in combat
                            if (pc.IsInCombat)
                            {
                                //check to 
                                foreach (INonPlayerCharacter roomNpc in npc.Room.NonPlayerCharacters)
                                {
                                    if (npc != roomNpc) //don't bother checking if the guardian is in combat with the pc
                                    {
                                        if (pc.AreFighting(roomNpc))
                                        {
                                            command = "kill " + pc.KeyWords.FirstOrDefault();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return command;
        }
    }
}
