using Objects.Global;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Mob.Interface;
using Objects.Personality.Interface;

namespace Objects.Personality.Custom.GrandViewGraveYard
{
    public class GroundsKeeper : IPersonality
    {
        private int step;
        private State stateMachine = State.LookForCorpse;

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command == null)
            {
                //check if it is night
                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour >= 12)
                {
                    //check to see if they are not in the house
                    if (npc.Room.Id != 26)
                    {
                        if (npc.Room.South != null)
                        {
                            npc.EnqueueCommand("South");
                        }
                        else if (npc.Room.East != null)
                        {
                            npc.EnqueueCommand("East");
                        }
                    }
                }
                else
                {
                    foreach (IItem item in npc.Room.Items)
                    {
                        if (item is ICorpse)
                        {
                            step++;
                            if (stateMachine == State.LookForCorpse)
                            {
                                stateMachine = State.BuryingCorpse;
                                return $"Get {item.KeyWords[0]}";
                            }
                            else if (stateMachine == State.BuryingCorpse)
                            {
                                switch (step)
                                {
                                    case 2:
                                        return "Emote starts digging a grave for the corpse.";
                                    case 4:
                                        return "Emote places the body in the grave.";
                                    case 6:
                                        step = 0;
                                        return "Say And stay there this time.";
                                    default:
                                        return "Wait";
                                }
                            }


                            return "";
                        }
                    }
                }
            }

            return command;
        }

        private enum State
        {
            LookForCorpse,
            BuryingCorpse,
        }
    }
}
