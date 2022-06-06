using Objects.Die.Interface;
using Objects.Global;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Material.Materials;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static Objects.Damage.Damage;
using static Objects.Item.Items.Equipment;
using static Objects.Mob.NonPlayerCharacter;

namespace Objects.Personality.Custom.GrandviewCastle
{
    public class Queen : IPersonality
    {
        private State StateMachine { get; set; } = State.Sleep;
        private int Step;
        private bool GreetedKing { get; set; }
        private List<string> GreetingForKing = new List<string>() { "Good morning honey.", "Good morning sweetie.", "Good morning moonbeam.", "Hello my handsome lion." };


        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command != null)
            {
                return command;
            }

            #region Combat
            if (npc.IsInCombat)
            {
                int howManyKingsGuards = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "Queen's guard").Count;
                if (howManyKingsGuards < 4)
                {
                    npc.EnqueueCommand("Say GUARDS!");
                    SummonQueensGuards(4 - howManyKingsGuards, npc.Room);
                }
                return "Flee";
            }
            #endregion Combat

            if (!GreetedKing && npc.Room.Id == 21)
            {
                if (GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "king").Count > 0)
                {
                    GreetedKing = true;
                    return GreetingForKing[GlobalReference.GlobalValues.Random.Next(GreetingForKing.Count)];
                }
            }

            Step++;

            int hour = GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour;

            if (hour < 13)
            {
                return DayTimeThings(npc);
                //return NightTimeThings(npc);
            }
            else
            {
                return NightTimeThings(npc);
            }
        }

        private string DayTimeThings(INonPlayerCharacter npc)
        {

            if (StateMachine == State.Sleep)
            {
                switch (Step)
                {
                    case 1:
                        return "Say Make the sun go away.  I officially decree it.  Go away.";
                    case 5:
                        return "Emote rolls out of bed.";
                    case 10:
                        return "Stand";
                    case 12:
                        return "Emote drinks coffee.";
                    case 14:
                        StateMachine = State.Up;
                        return "Say Much better.";
                }
            }
            else if (StateMachine == State.Up)
            {
                if (npc.Room.Zone == 24
                    && npc.Room.Id == 22)
                {
                    return "East";
                }
            }

            return null;
        }

        private string NightTimeThings(INonPlayerCharacter npc)
        {
            if (npc.Room.Id == 20)
            {
                string message = null;
                StateMachine = State.GotoBalcony;

                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>King says Court is closed for the day. Please come back tomorrow.</Communication>")
                    {
                        npc.EnqueueCommand("Say Its about time.");
                        return "West";
                    }
                }

                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour == 14)
                {
                    npc.EnqueueCommand("Say Court is closed for the day. Please come back tomorrow.");
                    return "West";
                }
            }

            if (StateMachine == State.GotoBalcony)
            {
                if (npc.Room.Zone == 24
                    && npc.Room.Id == 22)
                {
                    Step = 0;
                    return "North";
                }

                if (GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "King").Count > 0)
                {
                    StateMachine = State.SpendTimeWithKing;
                    Step = 0;
                    return "Say Hello dear.";
                }
            }
            else if (StateMachine == State.SpendTimeWithKing)
            {
                if (Step % 5 == 0)
                {
                    string message = null;

                    while ((message = npc.DequeueMessage()) != null)
                    {
                        if (message == "<Communication>King says Hello my beautify Queen.</Communication>")
                        {
                            return "Say I wish we could just leave this all behind.";
                        }
                        else if (message == "<Communication>King says That sounds nice.  We should take a trip to the country to get away for a while.</Communication>")
                        {
                            return "Say A trip to the country sounds great  We can goto the villa.";
                        }
                        else if (message == "<Communication>King says Lets plan to do this when the weather gets a little nicer.</Communication>")
                        {
                            return "Say Agreed.";
                        }
                        else if (message == "<Communication>King says Good night my love.</Communication>")
                        {
                            StateMachine = State.Bath;
                            Step = 0;
                            return "Say Goodnight my dear.";
                        }
                    }
                }
            }
            else if (StateMachine == State.Bath)
            {
                if (npc.Room.Zone == 24
                    && npc.Room.Id == 23)
                {
                    return "South";
                }
                else if (npc.Room.Zone == 24
                    && npc.Room.Id == 22)
                {
                    return "South";
                }
                else if (npc.Room.Zone == 24
                    && npc.Room.Id == 24)
                {
                    if (Step % 5 == 0)
                    {
                        StateMachine = State.Undress;
                        Step = 0;
                        npc.LookDescription = "The Queen's hair falls gently down the back of her naked figure.";
                        return "Emote removes her dress.";
                    }
                }
            }
            else if (StateMachine == State.Undress)
            {
                if (Step % 5 == 0)
                {
                    StateMachine = State.InTub;
                    npc.LookDescription = "The Queen relaxes in the tub almost floating with only her head above the water.";

                    return "Emote climbs into bath tub.";
                }
            }
            else if (StateMachine == State.InTub)
            {
                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour == 20)
                {
                    StateMachine = State.GetDress;
                    Step = 0;
                    npc.LookDescription = "The Queen's hair falls gently down the back of her naked figure.";
                    return "Emote slowly rises out of the tub.";
                }
            }
            else if (StateMachine == State.GetDress)
            {
                if (Step % 5 == 0)
                {
                    StateMachine = State.GotoSleep;
                    npc.LookDescription = "The Queen is dressed in her white sleep gown.";

                    return "Emote puts on her night gown.";
                }
            }
            else if (StateMachine == State.GotoSleep)
            {
                if (Step % 5 == 0)
                {
                    if (npc.Room.Zone == 24
                    && npc.Room.Id == 24)
                    {
                        return "North";
                    }
                    else if (npc.Room.Zone == 24
                    && npc.Room.Id == 22)
                    {
                        return "Sleep";
                    }
                }
            }

            return null;
        }


        private enum State
        {
            Sleep,
            Up,
            GotoBalcony,
            SpendTimeWithKing,
            Bath,
            Undress,
            InTub,
            GetDress,
            GotoSleep
        }


        #region Queens Guard
        private void SummonQueensGuards(int howMany, IRoom room)
        {
            for (int i = 0; i < howMany; i++)
            {
                INonPlayerCharacter npc = QueensGuard(room);
                npc.FinishLoad();
                room.Enter(npc);
            }
        }

        private INonPlayerCharacter QueensGuard(IRoom room)
        {
            string corpseDescription = "A look of fear is frozen upon the queens guard face.";
            string lookDescription = "Dressed in silver armor shaped like a female lions head on their breastplate they have sworn their life to protect the queen.";
            string examineDescription = "Each guard has under gone extensive training in both body in mind to ensure their loyalty unto death.";
            string sentenceDescription = "Queen's guard";
            string shortDescription = "The Queen's guard.";

            INonPlayerCharacter npc = new NonPlayerCharacter(room, corpseDescription, examineDescription, lookDescription, sentenceDescription, shortDescription);
            npc.TypeOfMob = MobType.Humanoid;
            npc.Level = 45;
            npc.KeyWords.Add("Queen's guard");
            npc.KeyWords.Add("guard");

            npc.AddEquipment(BreastPlate(npc));
            npc.AddEquipment(Helmet(npc));
            npc.AddEquipment(Sword(npc));

            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 22));
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 23));
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 24));

            npc.Personalities.Add(new Aggressive());

            return npc;
        }

        private IArmor BreastPlate(INonPlayerCharacter npc)
        {
            IDice dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(npc.Level+2);
            string examineDescription = "This piece of armor appears to be made better than normal.";
            string lookDescription = "A female lion head is embossed across the front of the breastplate.";
            string sentenceDescription = "beautiful silver breastplate";
            string shortDescription = "A breastplate made of silver.";

            IArmor armor = new Armor(dice, AvalableItemPosition.Body, examineDescription, lookDescription,  sentenceDescription, shortDescription);
            armor.KeyWords.Add("breastplate");
            armor.Material = new Silver();

            return armor;
        }

        private IArmor Helmet(INonPlayerCharacter npc)
        {
            IDice dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(npc.Level+2);
            string examineDescription = "This piece of armor appears to be made better than normal.";
            string lookDescription = "The helmet is made to make the wearer look like a lioness.";
            string sentenceDescription = "beautiful silver helmet with intricate lines carved in it";
            string shortDescription = "A helmet made of silver.";

            IArmor armor = new Armor(dice, AvalableItemPosition.Head, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.KeyWords.Add("helmet");
            armor.Material = new Silver();

            return armor;
        }

        private IWeapon Sword(INonPlayerCharacter npc)
        {
            string examineDescription = "This sword to be made better than normal.";
            string lookDescription = "The sword handle has the head of a lioness for the pummel.";
            string sentenceDescription = "a finely crafted sword";
            string shortDescription = "A finely crafted sword that is light and quick.";

            IWeapon weapon = new Weapon(AvalableItemPosition.Wield, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.Level = npc.Level;
            weapon.DamageList.Add(new Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(weapon.Level + 2)) { Type = DamageType.Slash });
            weapon.KeyWords.Add("sword");
            

            return weapon;
        }
        #endregion Queens Guard
    }
}
