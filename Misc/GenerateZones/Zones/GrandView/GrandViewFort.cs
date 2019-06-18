﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Objects.Damage;
using Objects.Damage.Interface;
using Objects.Global;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using Objects.Personality.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Room.Room;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewFort : BaseZone, IZoneCode
    {
        public GrandViewFort() : base(24)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewFort);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.West, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.West, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.South, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.West, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.West, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.North, Zone.Rooms[9]);
        }

        #region Rooms
        private IRoom OutSideRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(RoomAttribute.Outdoor);
            room.Attributes.Add(RoomAttribute.Weather);

            return room;
        }

        private IRoom InsideSideRoom()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(RoomAttribute.Indoor);
            room.Attributes.Add(RoomAttribute.Light);

            return room;
        }

        private IRoom GenerateRoom1()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "The stone walls were carved in place from the side of the mountain.  This leads to their strength as it is on solid piece of stone.";
            room.LookDescription = "The original fort's stone gate still stands strong.";
            room.ShortDescription = "Front Gate";

            room.AddMobileObjectToRoom(Guard());
            room.AddMobileObjectToRoom(Guard());

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "Standing in the center of the barbican you get a sense of dread for anyone who get trapped here attacking the fort.";
            room.LookDescription = "Walls of stone rise up on all sides with places for guards to fire arrows as well as dump fire down on you if you were an attacker.";
            room.ShortDescription = "Inside the barbican.";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "The inside of the fort court yard you begin to realize the amount of work that went into creating this fort.  Tons of raw stone was removed from the mountain side just to clear the area for this courtyard.";
            room.LookDescription = "The court yard extends a ways to the west before disappearing into the mountain.  The blacksmith and enchanter is to south.  The captains quarters, and stables are to the north.";
            room.ShortDescription = "The courtyard.";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "A rather large wooden structure stand here with a sign reading \"Ye Old Shoppe\" hangs above the doorway.";
            room.LookDescription = "A small ally is formed by the shops and the forts walls.";
            room.ShortDescription = "Side alley.";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "As you stand in front of the enchanters shop a large boom can be heard from the back of shop.  Black smoke can be seen pouring out of the front door.";
            room.LookDescription = "The ally stretches around the corner of the enchanters shop.";
            room.ShortDescription = "Side alley.";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = InsideSideRoom();

            room.ExamineDescription = "Different wares are hung from the wall.  Swords, axes, leggings and a kite shield with a potato painted on it...";
            room.LookDescription = "The sound of a fire and clanging can be heard in the back.";
            room.ShortDescription = "Ye Old Shoppe.";

            room.AddMobileObjectToRoom(ShoppeKeep());

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = InsideSideRoom();

            room.ExamineDescription = "The room surprisingly does not have any lights and is instead lit by the soft glow of the enchanted items for sale. ";
            room.LookDescription = "upon entering the room you notice the items for sale slowly drift around the room.";
            room.ShortDescription = "The Magic Circle.";

            room.AddMobileObjectToRoom(Enchantress());
            room.AddItemToRoom(Enchantery());

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "A large barn with rows of stalls used for keeping horses.";
            room.LookDescription = "Walking into the alley immediately tells you that you have found the horses stables.";
            room.ShortDescription = "Side alley.";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = OutSideRoom();

            room.ExamineDescription = "While a permanent structure the captains quarter is a quickly built wooden building.";
            room.LookDescription = "The captains building stands in front of you.";
            room.ShortDescription = "Side alley.";

            return room;
        }


        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Guard()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(Objects.Mob.NonPlayerCharacter.MobType.Humanoid, 21);
            npc.ShortDescription = "A motionless guard.";
            npc.LookDescription = "The guard stands motionless while watching people move in and out of the fort.";
            npc.ExamineDescription = "The guard's face is blank but you almost detect a hint of boredom.";
            npc.SentenceDescription = "guard";
            npc.KeyWords.Add("guard");

            npc.Personalities.Add(new Guardian());

            return npc;
        }

        private INonPlayerCharacter ShoppeKeep()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(Objects.Mob.NonPlayerCharacter.MobType.Humanoid, 40);
            npc.ShortDescription = "A shoppe keep tidies the place.";
            npc.LookDescription = "The shoppe keep is young.  He probably doesn't own the shop as much as work the front while the master makes the wares in the back.";
            npc.ExamineDescription = "Standing five feet tall with dusty blond hair you can tell the boy is young.  His hands are calloused which indicates he works in the back after the shop closes.";
            npc.SentenceDescription = "shoppekeep";
            npc.KeyWords.Add("shop");
            npc.KeyWords.Add("shoppe");
            npc.KeyWords.Add("keep");
            npc.KeyWords.Add("shopkeep");
            npc.KeyWords.Add("shoppekeep");

            IMerchant merchant = new Merchant();
            merchant.Sellables.Add(Shield());
            merchant.Sellables.Add(Sword());
            merchant.Sellables.Add(SplitMail());
            merchant.Sellables.Add(Gloves());

            npc.Personalities.Add(new Craftsman());
            npc.Personalities.Add(merchant);

            return npc;
        }

        private INonPlayerCharacter Enchantress()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(Objects.Mob.NonPlayerCharacter.MobType.Humanoid, 40);
            npc.ShortDescription = "An enchantress works on enchanting an small medallion.";
            npc.LookDescription = "She is dressed in a blue dress that seems to be made of some type of material that is so light it almost hangs on her. ";
            npc.ExamineDescription = "She  very intently stares at the work in front of her as she put says an incantation and pours a oil onto the medallion.";
            npc.SentenceDescription = "enchantress";
            npc.KeyWords.Add("enchantress");

            IMerchant merchant = new Merchant();
            npc.Personalities.Add(merchant);

            return npc;
        }
        #endregion NPC

        #region Items
        private IEquipment Shield()
        {
            IShield item = CreateShield(35, new Wood());
            item.KeyWords.Add("Shield");
            item.KeyWords.Add("Potato");
            item.ShortDescription = "A large wooden kite shield with a potato painted on the front.";
            item.LookDescription = "The potato seems rather out of place as if the maker really liked potatoes.";
            item.ExamineDescription = "Standing four and half feet tall this shield is almost as tall as the shoppekeep.";
            item.SentenceDescription = "potato kite shield";
            item.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(item.Level);
            item.FinishLoad();
            return item;
        }

        private IEquipment Sword()
        {
            IWeapon item = CreateWeapon(WeaponType.Sword, 28);
            item.KeyWords.Add("sword");
            item.ShortDescription = "A well balanced sword.";
            item.LookDescription = "The sword in remarkable if only in being unremarkable.";
            item.ExamineDescription = "The sword is nothing special and appears to be a mass produced sword for the soldiers stationed at the fort.";
            item.SentenceDescription = "sword";

            IDamage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(item.Level);
            damage.Type = Damage.DamageType.Slash;
            item.DamageList.Add(damage);
            item.FinishLoad();
            return item;
        }

        private IEquipment SplitMail()
        {
            IArmor item = CreateArmor(AvalableItemPosition.Body, 30);
            item.Material = new Steel();
            item.KeyWords.Add("splint");
            item.KeyWords.Add("mail");
            item.KeyWords.Add("green");
            item.ShortDescription = "A green splint mail.";
            item.LookDescription = "The green piece of splint mail still has the new look.";
            item.ExamineDescription = "Each piece of mail has been carefully place riveted into place.";
            item.SentenceDescription = "splint mail";
            item.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(item.Level);
            item.FinishLoad();
            return item;
        }

        private IEquipment Gloves()
        {
            IArmor item = CreateArmor(AvalableItemPosition.Hand, 26);
            item.Material = new Leather();
            item.KeyWords.Add("gloves");
            item.KeyWords.Add("eel");
            item.ShortDescription = "A pair of gloves.";
            item.LookDescription = "Each glove has a slight iridescent color that changes from green to brown and back again.";
            item.ExamineDescription = "The gloves are made of eel skin and have a green brown color that is hard to describe.";
            item.SentenceDescription = "gloves";
            item.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(item.Level);
            item.FinishLoad();
            return item;
        }

        private IItem Enchantery()
        {
            IEnchantery item = CreateItem<IEnchantery>();
            item.KeyWords.Add("table");
            item.KeyWords.Add("enchant");
            item.KeyWords.Add("enchanting");
            item.Attributes.Add(ItemAttribute.NoGet);
            item.ShortDescription = "An enchanting table.";
            item.LookDescription = "The table at one time was nothing more than some wood but has gain magical energy from hundreds nay thousands of enchantments.";
            item.ExamineDescription = "Green filaments of energy spark out from the table about an inch forming arches before falling back and being reabsorbed.";
            item.SentenceDescription = "enchanting table";

            return item;
        }
        #endregion Items
    }
}
