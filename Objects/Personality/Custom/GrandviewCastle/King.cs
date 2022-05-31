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
    public class King : IPersonality
    {
        private State StateMachine { get; set; } = State.Sleep;
        private int Step;
        private bool GreetedQueen { get; set; }
        private List<string> GreetingForQueen = new List<string>() { "Good morning honey.", "Good morning buttercup.", "I hope you slept well.", "Good morning sunshine.", "You look as lovely as the first time I met you.", "Hello my desert lily." };

        public string Process(INonPlayerCharacter npc, string command)
        {
            if (command != null)
            {
                return command;
            }

            #region Combat
            if (npc.IsInCombat)
            {
                int howManyKingsGuards = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "King's guard").Count;
                if (howManyKingsGuards < 4)
                {
                    npc.EnqueueCommand("Say GUARDS!");
                    SummonKingsGuards(4 - howManyKingsGuards, npc.Room);
                }
                return "Flee";
            }
            #endregion Combat

            if (!GreetedQueen && npc.Room.Id == 21)
            {
                if (GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "Queen").Count > 0)
                {
                    GreetedQueen = true;
                    return GreetingForQueen[GlobalReference.GlobalValues.Random.Next(GreetingForQueen.Count)];
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
                        return "Emote stretches and yawns loudly.";
                    case 3:
                        return "Say Good morning world.";
                    case 4:
                        StateMachine = State.MoveToBathRoom;
                        Step = 0;
                        return "Stand";
                }
            }
            else if (StateMachine == State.MoveToBathRoom)
            {
                if (npc.Room.Id == 22)
                {
                    return "South";
                }
                else if (npc.Room.Id == 24)
                {
                    StateMachine = State.InBathRoom;
                    Step = 0;
                }
            }
            else if (StateMachine == State.InBathRoom)
            {
                switch (Step)
                {
                    case 1:
                        return "Emote brushes his teeth.";
                    case 3:
                        return "Emote uses the bathroom.";
                    case 5:
                        return "North";
                    case 6:
                        StateMachine = State.ThroneRoom;
                        return "East";
                }
            }
            else if (StateMachine == State.ThroneRoom)
            {
                if (Step % 5 == 0
                    && GlobalReference.GlobalValues.Random.PercentDiceRoll(50)
                    && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "servant").Count > 0)
                {
                    StateMachine = State.AskedForMeal;
                    Step = 0;
                    return "Say Servant, bring me my meal.";
                }
            }
            else if (StateMachine == State.AskedForMeal)
            {
                if (Step % 5 == 0
                   && GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "servant").Count > 0)
                {
                    bool foundAskedForWhat = false;
                    string message = null;
                    while ((message = npc.DequeueMessage()) != null)
                    {
                        if (message == "<Communication>Kings servant says Your Honorable Majestic Majesty Graciousness, what would you like to eat?</Communication>")
                        {
                            foundAskedForWhat = true;
                            break;
                        }
                    }

                    if (foundAskedForWhat)
                    {
                        StateMachine = State.AskedForHasenpfeffer;
                        Step = 0;
                    }

                    return null;
                }
            }
            else if (StateMachine == State.AskedForHasenpfeffer)
            {
                string message = null;
                while ((message = npc.DequeueMessage()) != null)
                {
                    if (message == "<Communication>Kings servant says Bon appetit Most Gracious Majesty.</Communication>")
                    {
                        StateMachine = State.ReceivedHasenpfeffer;
                        Step = 0;
                        break;
                    }
                    else if (message == "<Communication>Kings servant says Your hasenpfeffer Your Magisty.</Communication>")
                    {
                        StateMachine = State.ReceivedCarrot;
                        Step = 0;
                        break;
                    }
                }

                if (StateMachine == State.AskedForHasenpfeffer)
                {
                    switch (Step)
                    {
                        case 1:
                            return "Say Bring me hasenpfeffer.";
                        case 20:
                            return "Say Where is my hasenpfeffer?";
                    }
                }
            }
            else if (StateMachine == State.ReceivedHasenpfeffer)
            {
                StateMachine = State.ThroneRoom;
                return "Emote eats hasenpfeffer.";
            }
            else if (StateMachine == State.ReceivedCarrot)
            {
                switch (Step)
                {
                    case 1:
                        return "Emote eats hasenpfeffer.";
                    case 3:
                        StateMachine = State.ThroneRoom;
                        return "Say If I didn't know this was hasenpfeffer I'd swear it was carrots.";
                }
            }

            return null;
        }

        private string NightTimeThings(INonPlayerCharacter npc)
        {
            if (npc.Room.Id == 21)
            {
                npc.EnqueueCommand("Say Court is closed for the day. Please come back tomorrow.");
                StateMachine = State.SpendTimeWithQueen;
                return "West";
            }

            if (npc.Room.Id != 21 && npc.Room.Zone == 24)
            {
                if (npc.Room.PlayerCharacters.Count > 0)
                {
                    foreach (IPlayerCharacter pc in npc.Room.PlayerCharacters)
                    {
                        if (GlobalReference.GlobalValues.CanMobDoSomething.SeeObject(npc, pc))
                        {
                            int howManyKingsGuards = GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "King's guard").Count;
                            if (howManyKingsGuards < 4)
                            {
                                SummonKingsGuards(4 - howManyKingsGuards, npc.Room);
                                return "Say GUARDS!";
                            }
                        }
                    }
                }
            }

            if (StateMachine == State.SpendTimeWithQueen)
            {
                if (npc.Room.Id == 22)
                {
                    Step = 0;
                    return "North";
                }

                if (Step % 5 == 0)
                {
                    string message = null;

                    while ((message = npc.DequeueMessage()) != null)
                    {
                        if (message == "<Communication>Queen says Hello dear.</Communication>")
                        {
                            return "Say Hello my beautify queen.";
                        }
                        else if (message == "<Communication>Queen says I wish we could just leave this all behind.</Communication>")
                        {
                            return "Say That sounds nice.  We should take a trip to the country to get away for a while.";
                        }
                        else if (message == "<Communication>Queen says A trip to the country sounds great  We can goto the villa.</Communication>")
                        {
                            return "Say Lets plan to do this when the weather gets a little nicer.";
                        }
                        else if (message == "<Communication>Queen says Agreed.</Communication>")
                        {
                            return "Say Good night my love.";
                        }
                        else if (message == "<Communication>Queen says Goodnight my dear.</Communication>")
                        {
                            StateMachine = State.Sleep;
                            return "South";
                        }
                    }
                }
            }
            else if (StateMachine == State.Sleep)
            {
                if (npc.Room.Zone == 24
                    && npc.Room.Id == 14)
                {
                    if (npc.Position != MobileObject.CharacterPosition.Sleep)
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
            MoveToBathRoom,
            InBathRoom,
            ThroneRoom,
            AskedForMeal,
            AskedForHasenpfeffer,
            ReceivedHasenpfeffer,
            ReceivedCarrot,
            SpendTimeWithQueen,
            GetReadForBed,
        }

        #region Kings Guard
        private void SummonKingsGuards(int howMany, IRoom room)
        {
            for (int i = 0; i < howMany; i++)
            {
                INonPlayerCharacter npc = KingsGuard(room);
                npc.FinishLoad();
                room.Enter(npc);
            }
        }

        private INonPlayerCharacter KingsGuard(IRoom room)
        {
            string corpseDeath = "The guard has given his life to try to save the King.  There is no nobler death for a guard.";
            string examineDescription = "Each guard has under gone extensive training in both body in mind to ensure their loyalty unto death.";
            string lookDescription = "Dressed in golden armor shaped like a male lions head on their breastplate they have sworn their life to protect the King.";
            string sentenceDescription = "King's guard";
            string shortDescription = "The King's guard.";

            INonPlayerCharacter npc = new NonPlayerCharacter(room, corpseDeath, examineDescription,  lookDescription, sentenceDescription, shortDescription);
            npc.TypeOfMob = MobType.Humanoid;
            npc.Level = 45;
            npc.KeyWords.Add("King's guard");
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
            string examineDescription = "This piece of armor appears to be made better than normal.";
            string lookDescription = "A male lion head is embossed across the front of the breastplate.";
            string sentenceDescription = "golden breastplate";
            string shortDescription = "A breastplate made of gold.";


            IArmor armor = Armor(AvalableItemPosition.Body, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.KeyWords.Add("breastplate");
            

            return armor;
        }

        private IArmor Helmet()
        {
            string examineDescription = "This piece of armor appears to be made better than normal.";
            string lookDescription = "The helmet is made to make the wearer look like a lion.";
            string sentenceDescription = "golden helmet";
            string shortDescription = "A helmet made of gold.";
            IArmor armor = Armor(AvalableItemPosition.Head, examineDescription, lookDescription, sentenceDescription, shortDescription);
            armor.KeyWords.Add("helmet");
            

            return armor;
        }

        private static IArmor Armor(AvalableItemPosition avalableItemPosition, string examinePosition, string lookDescription, string sentenceDescription, string shortDescription)
        {
            IDice dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(45);
            IArmor armor = new Armor(dice, avalableItemPosition, examinePosition, lookDescription, sentenceDescription, shortDescription);
            armor.Level = 45;
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level + 2);
            armor.Material = new Gold();

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
            weapon.LookDescription = "The sword handle has the head of a lion for the pummel.";
            weapon.ExamineDescription = "This sword to be made better than normal.";

            return weapon;
        }

        #endregion Kings Guard
    }
}
