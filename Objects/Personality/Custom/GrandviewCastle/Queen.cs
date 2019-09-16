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
        private bool GreetKing { get; set; }
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
                    npc.EnqueueCommand("Shout GUARDS!");
                    SummonQueensGuards(4 - howManyKingsGuards, npc.Room);
                }
                return "Flee";
            }
            #endregion Combat

            int hour = GlobalReference.GlobalValues.GameDateTime.GameDateTime.Hour;

            if (!GreetKing && npc.Room.Id == 21)
            {
                if (GlobalReference.GlobalValues.FindObjects.FindNpcInRoom(npc.Room, "queen").Count > 1)
                {
                    GreetKing = true;
                    return GreetingForKing[GlobalReference.GlobalValues.Random.Next(GreetingForKing.Count)];
                }
            }

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
            Step++;

            if (StateMachine == State.Sleep)
            {
                switch (Step)
                {
                    case 1:
                        return "Say Make the sun go away.  I officially decree it.  Go away.";
                    case 5:
                        return "Emote rolls out of bed.";
                    case 10:
                        StateMachine = State.Up;
                        Step = 0;
                        return "Stand";
                }
            }
            else if (StateMachine == State.Up)
            {
                if (npc.Room.Id == 22)
                {
                    return "South";
                }
            }

            return null;
        }

        private string NightTimeThings(INonPlayerCharacter npc)
        {


            return null;
        }


        private enum State
        {
            Sleep,
            Up
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
