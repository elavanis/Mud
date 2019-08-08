using System;
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
using Objects.Personality.Personalities.GrandviewCastle;
using Objects.Personality.Personalities.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

namespace GenerateZones.Zones.GrandView
{
    public class GrandViewCastle : BaseZone, IZoneCode
    {
        public GrandViewCastle() : base(24)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GrandViewCastle);

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
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.West, Zone.Rooms[15]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.West, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.West, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.North, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.West, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.West, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.North, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[11], Direction.West, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[15], Direction.West, Zone.Rooms[16]);
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.West, Zone.Rooms[17]);
            ZoneHelper.ConnectRoom(Zone.Rooms[17], Direction.West, Zone.Rooms[18]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.North, Zone.Rooms[19]);
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.Up, Zone.Rooms[20]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.West, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.West, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.North, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[22], Direction.South, Zone.Rooms[24]);
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "The stone walls were carved in place from the side of the mountain.  This leads to their strength as it is on solid piece of stone.";
            room.LookDescription = "The original castle's stone gate still stands strong.";
            room.ShortDescription = "Front Gate";

            room.AddMobileObjectToRoom(Guard());
            room.AddMobileObjectToRoom(Guard());

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "Standing in the center of the barbican you get a sense of dread for anyone who get trapped here attacking the castle.";
            room.LookDescription = "Walls of stone rise up on all sides with places for guards to fire arrows as well as dump fire down on you if you were an attacker.";
            room.ShortDescription = "Inside the barbican";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "The inside of the castle court yard you begin to realize the amount of work that went into creating this castle.  Tons of raw stone was removed from the mountain side just to clear the area for this courtyard.";
            room.LookDescription = "The court yard extends a ways to the west before disappearing into the mountain.  The blacksmith and enchanter is to south.  The captains quarters, and stables are to the north.";
            room.ShortDescription = "The courtyard";

            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Squire());
            room.AddMobileObjectToRoom(Squire());

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "A rather large wooden structure stand here with a sign reading \"Ye Old Shoppe\" hangs above the doorway.";
            room.LookDescription = "A small ally is formed by the shops and the castle walls.";
            room.ShortDescription = "Side alley";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "As you stand in front of the enchanters shop a large boom can be heard from the back of shop.  Black smoke can be seen pouring out of the front door.";
            room.LookDescription = "The ally stretches around the corner of the enchanters shop.";
            room.ShortDescription = "Side alley";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "Different wares are hung from the wall.  Swords, axes, leggings and a kite shield with a potato painted on it...";
            room.LookDescription = "The sound of a fire and clanging can be heard in the back.";
            room.ShortDescription = "Ye Old Shoppe";

            room.AddMobileObjectToRoom(ShoppeKeep());

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The room surprisingly does not have any lights and is instead lit by the soft glow of the enchanted items for sale. ";
            room.LookDescription = "upon entering the room you notice the items for sale slowly drift around the room.";
            room.ShortDescription = "The Magic Circle";

            room.AddMobileObjectToRoom(Enchantress());
            room.AddItemToRoom(Enchantery());

            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "A large barn with rows of stalls used for keeping horses.";
            room.LookDescription = "Walking into the alley immediately tells you that you have found the horses stables.";
            room.ShortDescription = "Side alley";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "The captains quarters are testament to what can be done with superb craftsmanship.";
            room.LookDescription = "The captains building stands in front of you.";
            room.ShortDescription = "Side alley";

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "Each stall has two sets of doors.  The inside doors let the animals be lead to center of the stables and the outside doors lets the animals go outside and frolic in the area around the stables. ";
            room.LookDescription = "Walking up and down the isle you can see the name of each horse on a placard hanging above their stall. Rapidflame, Autumn, Maverick and Shadowbolt to name a few.";
            room.ShortDescription = "Stables";

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The floor has a beautiful inlay of different types of wood.  Each piece was carefully placed to form a crane in mid flight.";
            room.LookDescription = "You stand in the entrance of the captains quarters.  To the left is a meeting room and to the right is smaller waiting room.  In the back is a hall leading to the sleeping area.";
            room.ShortDescription = "Captains Quarters";

            room.AddMobileObjectToRoom(Captain());

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The table is made of oak and has a large vase of flowers in the center.  Several torches light the room causing shadows to dance on the walls.";
            room.LookDescription = "The large round table dominates the meeting room.";
            room.ShortDescription = "Captains Quarters";

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "A small set of desks are arranged in a u shape with a map of the mines on the far wall.";
            room.LookDescription = "The small room feels cramped with three desks filling up the room.";
            room.ShortDescription = "Captains Quarters";

            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "There is a writing desk with a couple of papers on it.  One of them lists the attacks on miners from monsters in the mine.";
            room.LookDescription = "A modest but decent size bed is in one side of the room under with a window over looking the court yard.";
            room.ShortDescription = "Captains Quarters";

            return room;
        }

        private IRoom GenerateRoom15()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "To the north you can see the horses running behind the fence.";
            room.LookDescription = "The court yard has been packed with lots of foot traffic.";
            room.ShortDescription = "Court Yard";

            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Squire());
            room.AddMobileObjectToRoom(Squire());

            return room;
        }

        private IRoom GenerateRoom16()
        {
            IRoom room = OutdoorRoom();

            room.ExamineDescription = "To the north you can see the horses running behind the fence.";
            room.LookDescription = "The court yard has been packed with lots of foot traffic.";
            room.ShortDescription = "Court Yard";

            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Page());
            room.AddMobileObjectToRoom(Squire());
            room.AddMobileObjectToRoom(Squire());

            return room;
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The door to the keep was made of heavy oak several inches thick.  In times of war it would withhold all but the fiercest attacks.";
            room.LookDescription = "Standing inside the entrance of the keep this is would be the last line of defense for would be defenders.";
            room.ShortDescription = "Keep Entrance";

            return room;
        }

        private IRoom GenerateRoom18()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "Swords, shields, pikes, bows and arrows are on rack at the ready to be used to defend the keep.";
            room.LookDescription = "Large racks of weapons line the walls.";
            room.ShortDescription = "Armor Rack";

            return room;
        }

        private IRoom GenerateRoom19()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "Swords, shields, pikes, bows and arrows are on rack at the ready to be used to defend the keep.";
            room.LookDescription = "Large racks of weapons line the walls.";
            room.ShortDescription = "Kitchen";

            room.AddMobileObjectToRoom(CookMan());
            room.AddMobileObjectToRoom(CookMan());
            room.AddMobileObjectToRoom(CookWoman());
            room.AddMobileObjectToRoom(CookWoman());

            return room;
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "Several bench seats are against the wall.  They are covered in plush red velvet giving a sharp contrast to the cool gray stone.";
            room.LookDescription = "The area has plenty of seating for guest while they wait to be seen.";
            room.ShortDescription = "Antechamber";

            return room;
        }

        private IRoom GenerateRoom21()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The throne room has large pillars of stone rising twenty feet into the air.  Large tapestries of times past line the walls giving some warmth to the otherwise cold echoing hall.";
            room.LookDescription = "A large throne made of many iron swords melted together sits in the center of the hall.";
            room.ShortDescription = "Throne Room";

            return room;
        }

        private IRoom GenerateRoom22()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "The room is actually modestly equipped for a king and queen's room.";
            room.LookDescription = "A large four post bed with a canopy dominates this room.";
            room.ShortDescription = "Bedroom Room";

            room.AddItemToRoom(Bed());
            room.AddMobileObjectToRoom(King());

            return room;
        }

        private IRoom GenerateRoom23()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "There is a story that this room was made because during a long siege the king and queen were unable to go out and see the stars.  The king never wanted to miss seeing the stars again so a special room was made of their bedroom where they formed a balcony and painted stars on the ceiling to simulate the stars at night.";
            room.LookDescription = "A balcony over looks mural of stars and grass.";
            room.ShortDescription = "Balcony";

            return room;
        }

        private IRoom GenerateRoom24()
        {
            IRoom room = IndoorRoomLight();

            room.ExamineDescription = "There is also a counter of sorts with a beaten sheet of tin used as a mirror.  In the far end of the room is hole in the floor and a curtain used to shield you when doing your business.";
            room.LookDescription = "A large claw tube sits in the corner with a bucket of soapy water.";
            room.ShortDescription = "Lavatory";

            return room;
        }


        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Guard()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 23);
            npc.ShortDescription = "A motionless guard.";
            npc.LookDescription = "The guard stands motionless while watching people move in and out of the castle.";
            npc.ExamineDescription = "The guard's face is blank but you almost detect a hint of boredom.";
            npc.SentenceDescription = "guard";
            npc.KeyWords.Add("guard");

            npc.Personalities.Add(new Guardian());

            return npc;
        }

        private INonPlayerCharacter ShoppeKeep()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 40);
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
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 40);
            npc.ShortDescription = "An enchantress works on enchanting an small medallion.";
            npc.LookDescription = "She is dressed in a blue dress that seems to be made of some type of material that is so light it almost hangs on her. ";
            npc.ExamineDescription = "She very intently stares at the work in front of her as she put says an incantation and pours a oil onto the medallion.";
            npc.SentenceDescription = "enchantress";
            npc.KeyWords.Add("enchantress");

            IMerchant merchant = new Merchant();
            npc.Personalities.Add(merchant);

            return npc;
        }

        private INonPlayerCharacter Captain()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 55);
            npc.ShortDescription = "The captain doesn't notice you at first as you walk in the room.  He quickly glances up at you and then returns to his job.";
            npc.LookDescription = "The captain is busy doing paper work and has a large pile of papers on his desk labeled in.";
            npc.ExamineDescription = "The captain is dressed in standard military garb.  Nothing indicates he is a high ranking official other than he was sitting behind the desk when you walked in.";
            npc.SentenceDescription = "captain";
            npc.KeyWords.Add("captain");

            return npc;
        }

        private INonPlayerCharacter Page()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 21);
            npc.ShortDescription = "A young page.";
            npc.LookDescription = "The page hurrys off attending to an assignment.";
            npc.ExamineDescription = "The page looks flustered as if he has more work then time to complete it.";
            npc.SentenceDescription = "page";
            npc.KeyWords.Add("page");

            npc.Personalities.Add(new Wanderer());

            return npc;
        }

        private INonPlayerCharacter Squire()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 22);
            npc.ShortDescription = "A squire.";
            npc.LookDescription = "The squire walks by attending to official business.";
            npc.ExamineDescription = "Dressed in fine clothing the squire hopes can be seen practicing swordsmanship as they go by.";
            npc.SentenceDescription = "squire";
            npc.KeyWords.Add("squire");

            npc.Personalities.Add(new Wanderer());

            return npc;
        }

        private INonPlayerCharacter CookMan()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 22);
            npc.ShortDescription = "A cook.";
            npc.LookDescription = "He is cutting some meet off a hanging pig.";
            npc.ExamineDescription = "His once white cloths are hopelessly cover in blood.";
            npc.SentenceDescription = "cook";
            npc.KeyWords.Add("cook");

            return npc;
        }

        private INonPlayerCharacter CookWoman()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 22);
            npc.ShortDescription = "A cook.";
            npc.LookDescription = "She is stirring a soup over a fire.";
            npc.ExamineDescription = "The cook waves you over and ask you to try the soup.";
            npc.SentenceDescription = "cook";
            npc.KeyWords.Add("cook");

            return npc;
        }

        private INonPlayerCharacter King()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 28);
            npc.ShortDescription = "The king.";
            npc.LookDescription = "king";
            npc.ExamineDescription = "king";
            npc.SentenceDescription = "king";
            npc.KeyWords.Add("king");

            npc.Personalities.Add(new King());

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
            item.ExamineDescription = "The sword is nothing special and appears to be a mass produced sword for the soldiers stationed at the castle.";
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
            item.SuccessRate = -.85M;
            item.CostToEnchantLevel1Item = (int)(1.1 * item.CostToEnchantLevel1Item);
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

        private IItem Bed()
        {
            IItem item = CreateItem<IItem>();
            item.Attributes.Add(ItemAttribute.NoGet);
            item.KeyWords.Add("bed");
            item.ShortDescription = "A canopy bed.";
            item.LookDescription = "The bed look very soft with lots of fluffy light blue pillows.";
            item.ExamineDescription = "The bed frame is made of a dark wood with carvings of the GrandView crest on the foot and headboards.";
            item.SentenceDescription = "bed";

            return item;
        }
        #endregion Items
    }
}
