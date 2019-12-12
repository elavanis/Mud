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
                int howManyKingsGuards = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "queens guard").Count;
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
                        if (message == "<Communication>King says Hello my beautify queen.</Communication>")
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
                        npc.LookDescription = "The queens hair falls gently down the back of her naked figure.";
                        return "Emote removes her dress.";
                    }
                }
            }
            else if (StateMachine == State.Undress)
            {
                if (Step % 5 == 0)
                {
                    StateMachine = State.InTub;
                    npc.LookDescription = "The queen relaxes in the tub almost floating with only her head above the water.";

                    return "Emote climbs into bath tub.";
                }

                if (GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour == 20)
                {
                    StateMachine = State.GetDress;
                    Step = 0;
                    npc.LookDescription = "The queens hair falls gently down the back of her naked figure.";
                    return "Emote slowly rises out of the tub.";
                }
            }
            else if (StateMachine == State.GetDress)
            {
                if (Step % 5 == 0)
                {
                    StateMachine = State.GotoSleep;
                    npc.LookDescription = "The is dressed in her white sleep gown.";

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
                INonPlayerCharacter npc = QueensGuard();
                npc.FinishLoad();
                room.Enter(npc);
            }
        }

        private INonPlayerCharacter QueensGuard()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.TypeOfMob = MobType.Humanoid;
            npc.Level = 45;
            npc.ShortDescription = "The queens guard.";
            npc.LookDescription = "Dressed in silver armor shaped like a female lions head on their breastplate they have sworn their life to protect the queen.";
            npc.ExamineDescription = "Each guard has under gone extensive training in both body in mind to ensure their loyalty unto death.";
            npc.SentenceDescription = "queens guard";
            npc.KeyWords.Add("queens guard");
            npc.KeyWords.Add("guard");

            npc.AddEquipment(BreastPlate());
            npc.AddEquipment(Helmet());
            npc.AddEquipment(Sword());

            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 22));
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 23));
            wanderer.NavigableRooms.Add(new BaseObjectId(24, 24));

            npc.Personalities.Add(new Aggressive());

            return npc;
        }

        private IArmor BreastPlate()
        {
            IArmor armor = Armor(AvalableItemPosition.Body);
            armor.KeyWords.Add("breastplate");
            armor.ShortDescription = "A breastplate made of silver.";
            armor.LookDescription = "A female lion head is embossed across the front of the breastplate.";
            armor.ExamineDescription = "This piece of armor appears to be made better than normal.";

            return armor;
        }

        private IArmor Helmet()
        {
            IArmor armor = Armor(AvalableItemPosition.Head);
            armor.KeyWords.Add("helmet");
            armor.ShortDescription = "A helmet made of silver.";
            armor.LookDescription = "The helmet is made to make the wearer look like a lioness.";
            armor.ExamineDescription = "This piece of armor appears to be made better than normal.";

            return armor;
        }

        private static IArmor Armor(AvalableItemPosition avalableItemPosition)
        {
            IArmor armor = new Armor();
            armor.ItemPosition = avalableItemPosition;
            armor.Level = 45;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level + 2);
            armor.Material = new Silver();

            return armor;
        }

        private IWeapon Sword()
        {
            IWeapon weapon = new Weapon();
            weapon.ItemPosition = AvalableItemPosition.Wield;
            weapon.Level = 45;
            weapon.DamageList.Add(new Damage.Damage(GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(weapon.Level + 2)) { Type = DamageType.Slash });
            weapon.KeyWords.Add("sword");
            weapon.ShortDescription = "A finely crafted sword that is light and quick.";
            weapon.LookDescription = "The sword handle has the head of a lioness for the pummel.";
            weapon.ExamineDescription = "This sword to be made better than normal.";

            return weapon;
        }
        #endregion Queens Guard
    }
}
