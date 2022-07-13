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
using Objects.Personality;
using Objects.Personality.Custom.GrandviewCastle;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;
using MiscShared;

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

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }

        private void ConnectRooms()
        {
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
            string examineDescription = "The stone walls were carved in place from the side of the mountain.  This leads to their strength as it is on solid piece of stone.";
            string lookDescription = "The original castle's stone gate still stands strong.";
            string shortDescription = "Front Gate";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Guard(room));
            room.AddMobileObjectToRoom(Guard(room));

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "Standing in the center of the barbican you get a sense of dread for anyone who get trapped here attacking the castle.";
            string lookDescription = "Walls of stone rise up on all sides with places for guards to fire arrows as well as dump fire down on you if you were an attacker.";
            string shortDescription = "Inside the barbican";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The inside of the castle court yard you begin to realize the amount of work that went into creating this castle.  Tons of raw stone was removed from the mountain side just to clear the area for this courtyard.";
            string lookDescription = "The court yard extends a ways to the west before disappearing into the mountain.  The blacksmith and enchanter is to south.  The captains quarters, and stables are to the north.";
            string shortDescription = "The courtyard";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Squire(room));
            room.AddMobileObjectToRoom(Squire(room));

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "A rather large wooden structure stand here with a sign reading \"Ye Old Shoppe\" hangs above the doorway.";
            string lookDescription = "A small ally is formed by the shops and the castle walls.";
            string shortDescription = "Side alley";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "As you stand in front of the enchanters shop a large boom can be heard from the back of shop.  Black smoke can be seen pouring out of the front door.";
            string lookDescription = "The ally stretches around the corner of the enchanters shop.";
            string shortDescription = "Side alley";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "Different wares are hung from the wall.  Swords, axes, leggings and a kite shield with a potato painted on it...";
            string lookDescription = "The sound of a fire and clanging can be heard in the back.";
            string shortDescription = "Ye Old Shoppe";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(ShoppeKeep(room));

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "The room surprisingly does not have any lights and is instead lit by the soft glow of the enchanted items for sale. ";
            string lookDescription = "upon entering the room you notice the items for sale slowly drift around the room.";
            string shortDescription = "The Magic Circle";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Enchantress(room));
            room.AddItemToRoom(Enchantery());

            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "A large barn with rows of stalls used for keeping horses.";
            string lookDescription = "Walking into the alley immediately tells you that you have found the horses stables.";
            string shortDescription = "Side alley";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "The captains quarters are testament to what can be done with superb craftsmanship.";
            string lookDescription = "The captains building stands in front of you.";
            string shortDescription = "Side alley";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "Each stall has two sets of doors.  The inside doors let the animals be lead to center of the stables and the outside doors lets the animals go outside and frolic in the area around the stables. ";
            string lookDescription = "Walking up and down the isle you can see the name of each horse on a placard hanging above their stall. Rapidflame, Autumn, Maverick and Shadowbolt to name a few.";
            string shortDescription = "Stables";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "The floor has a beautiful inlay of different types of wood.  Each piece was carefully placed to form a crane in mid flight.";
            string lookDescription = "You stand in the entrance of the captains quarters.  To the left is a meeting room and to the right is smaller waiting room.  In the back is a hall leading to the sleeping area.";
            string shortDescription = "Captains Quarters";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Captain(room));

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "The table is made of oak and has a large vase of flowers in the center.  Several torches light the room causing shadows to dance on the walls.";
            string lookDescription = "The large round table dominates the meeting room.";
            string shortDescription = "Captains Quarters";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "A small set of desks are arranged in a u shape with a map of the mines on the far wall.";
            string lookDescription = "The small room feels cramped with three desks filling up the room.";
            string shortDescription = "Captains Quarters";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom14()
        {
            string examineDescription = "There is a writing desk with a couple of papers on it.  One of them lists the attacks on miners from monsters in the mine.";
            string lookDescription = "A modest but decent size bed is in one side of the room under with a window over looking the court yard.";
            string shortDescription = "Captains Quarters";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "To the north you can see the horses running behind the fence.";
            string lookDescription = "The court yard has been packed with lots of foot traffic.";
            string shortDescription = "Court Yard";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Squire(room));
            room.AddMobileObjectToRoom(Squire(room));

            return room;
        }

        private IRoom GenerateRoom16()
        {
            string examineDescription = "To the north you can see the horses running behind the fence.";
            string lookDescription = "The court yard has been packed with lots of foot traffic.";
            string shortDescription = "Court Yard";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Page(room));
            room.AddMobileObjectToRoom(Squire(room));
            room.AddMobileObjectToRoom(Squire(room));

            return room;
        }

        private IRoom GenerateRoom17()
        {
            string examineDescription = "The door to the keep was made of heavy oak several inches thick.  In times of war it would withhold all but the fiercest attacks.";
            string lookDescription = "Standing inside the entrance of the keep this is would be the last line of defense for would be defenders.";
            string shortDescription = "Keep Entrance";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom18()
        {
            string examineDescription = "Swords, shields, pikes, bows and arrows are on rack at the ready to be used to defend the keep.";
            string lookDescription = "Large racks of weapons line the walls.";
            string shortDescription = "Armor Rack";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom19()
        {
            string examineDescription = "The large fireplace has a giant iron pot used for cooking soups for the entire castle staff.";
            string lookDescription = "Pots and pans hand from the wall above a large fire place.";
            string shortDescription = "Kitchen";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(CookMan(room, 1));
            room.AddMobileObjectToRoom(CookMan(room, 2));
            room.AddMobileObjectToRoom(CookWoman(room, 3));
            room.AddMobileObjectToRoom(CookWoman(room, 4));

            return room;
        }

        private IRoom GenerateRoom20()
        {
            string examineDescription = "Several bench seats are against the wall.  They are covered in plush red velvet giving a sharp contrast to the cool gray stone.";
            string lookDescription = "The area has plenty of seating for guest while they wait to be seen.";
            string shortDescription = "Antechamber";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom21()
        {
            string examineDescription = "The throne room has large pillars of stone rising twenty feet into the air.  Large tapestries of times past line the walls giving some warmth to the otherwise cold echoing hall.";
            string lookDescription = "A large throne made of many iron swords melted together sits in the center of the hall.";
            string shortDescription = "Throne Room";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(RoyalGuard(room));
            room.AddMobileObjectToRoom(RoyalGuard(room));
            room.AddMobileObjectToRoom(RoyalGuard(room));
            room.AddMobileObjectToRoom(RoyalGuard(room));
            room.AddMobileObjectToRoom(Servant(room));

            return room;
        }

        private IRoom GenerateRoom22()
        {
            string examineDescription = "The room is actually modestly equipped for a king and queen's room.";
            string lookDescription = "A large four post bed with a canopy dominates this room.";
            string shortDescription = "Bedroom Room";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Bed());
            room.AddMobileObjectToRoom(King(room));
            room.AddMobileObjectToRoom(Queen(room));

            return room;
        }

        private IRoom GenerateRoom23()
        {
            string examineDescription = "There is a story that this room was made because during a long siege the king and queen were unable to go out and see the stars.  The king never wanted to miss seeing the stars again so a special room was made of their bedroom where they formed a balcony and painted stars on the ceiling to simulate the stars at night.";
            string lookDescription = "A balcony over looks mural of stars and grass.";
            string shortDescription = "Balcony";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom24()
        {
            string examineDescription = "There is also a counter of sorts with a beaten sheet of tin used as a mirror.  In the far end of the room is hole in the floor and a curtain used to shield you when doing your business.";
            string lookDescription = "A large claw tube sits in the corner with a bucket of soapy water.";
            string shortDescription = "Lavatory";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Guard(IRoom room)
        {
            string examineDescription = "The guard's face is blank but you almost detect a hint of boredom.";
            string lookDescription = "The guard stands motionless while watching people move in and out of the castle.";
            string sentenceDescription = "guard";
            string shortDescription = "A motionless guard.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 23);
            npc.KeyWords.Add("guard");
            npc.Personalities.Add(new Guardian());

            return npc;
        }

        private INonPlayerCharacter ShoppeKeep(IRoom room)
        {
            string examineDescription = "Standing five feet tall with dusty blond hair you can tell the boy is young.  His hands are calloused which indicates he works in the back after the shop closes.";
            string lookDescription = "The shoppe keep is young.  He probably doesn't own the shop as much as work the front while the master makes the wares in the back.";
            string sentenceDescription = "shoppekeep";
            string shortDescription = "A shoppe keep tidies the place.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 40);
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

        private INonPlayerCharacter Enchantress(IRoom room)
        {
            string examineDescription = "She very intently stares at the work in front of her as she put says an incantation and pours a oil onto the medallion.";
            string lookDescription = "She is dressed in a blue dress that seems to be made of some type of material that is so light it almost hangs on her. ";
            string sentenceDescription = "enchantress";
            string shortDescription = "An enchantress works on enchanting an small medallion.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 40);
            npc.KeyWords.Add("enchantress");

            IMerchant merchant = new Merchant();
            npc.Personalities.Add(merchant);

            return npc;
        }

        private INonPlayerCharacter Captain(IRoom room)
        {
            string examineDescription = "The captain is dressed in standard military garb.  Nothing indicates he is a high ranking official other than he was sitting behind the desk when you walked in.";
            string lookDescription = "The captain is busy doing paper work and has a large pile of papers on his desk labeled in.";
            string sentenceDescription = "captain";
            string shortDescription = "The captain doesn't notice you at first as you walk in the room.  He quickly glances up at you and then returns to his job.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 55);
            npc.KeyWords.Add("captain");

            return npc;
        }

        private INonPlayerCharacter Page(IRoom room)
        {
            string examineDescription = "The page looks flustered as if he has more work then time to complete it.";
            string lookDescription = "The page hurrys off attending to an assignment.";
            string sentenceDescription = "page";
            string shortDescription = "A young page.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 21);
            npc.KeyWords.Add("page");
            npc.Personalities.Add(new Wanderer());

            return npc;
        }

        private INonPlayerCharacter Squire(IRoom room)
        {
            string examineDescription = "Dressed in fine clothing the squire hopes can be seen practicing swordsmanship as they go by.";
            string lookDescription = "The squire walks by attending to official business.";
            string sentenceDescription = "squire";
            string shortDescription = "A squire.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 22);
            npc.KeyWords.Add("squire");
            npc.Personalities.Add(new Wanderer());

            return npc;
        }

        private INonPlayerCharacter CookMan(IRoom room, int id)
        {
            string examineDescription = "His once white cloths are hopelessly cover in blood.";
            string lookDescription = "He is cutting some meet off a hanging pig.";
            string sentenceDescription = "cook";
            string shortDescription = "A cook.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 22);
            npc.KeyWords.Add("cook");
            npc.Personalities.Add(new Cook(id));

            return npc;
        }

        private INonPlayerCharacter CookWoman(IRoom room, int id)
        {
            string examineDescription = "The cook waves you over and ask you to try the soup.";
            string lookDescription = "She is stirring a soup over a fire.";
            string sentenceDescription = "cook";
            string shortDescription = "A cook.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 22);
            npc.KeyWords.Add("cook");
            npc.Personalities.Add(new Cook(id));

            return npc;
        }

        private INonPlayerCharacter King(IRoom room)
        {
            string examineDescription = "The king is still young but is getting some gray in his beard.";
            string lookDescription = "Dressed in a dark red cloak and goes about his day.";
            string sentenceDescription = "king";
            string shortDescription = "The king.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 28);
            npc.KeyWords.Add("king");
            npc.Personalities.Add(new King());

            return npc;
        }

        private INonPlayerCharacter Queen(IRoom room)
        {
            string examineDescription = "The queen is still young and beautiful.";
            string lookDescription = "Dressed in a navy blue dress and goes about her day.";
            string sentenceDescription = "queen";
            string shortDescription = "The queen.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 28);
            npc.KeyWords.Add("queen");
            npc.Personalities.Add(new Queen());

            return npc;
        }

        private INonPlayerCharacter RoyalGuard(IRoom room)
        {
            string examineDescription = "The guard watches you closely ready to attack at a moments notice.";
            string lookDescription = "Dressed in a white armor the royal guard has sworn an oath to protect the royal family with their life.";
            string sentenceDescription = "royal guard";
            string shortDescription = "The royal guard.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 38);
            npc.KeyWords.Add("guard");
            npc.KeyWords.Add("royal");

            IGuard guard = new Guard(Direction.West);
            guard.BlockLeaveMessage = "The guard blocks you from entering and tells you this is the king and queens personal chambers and no one is allowed to enter.";
            npc.Personalities.Add(guard);
            npc.Personalities.Add(new Guardian());

            return npc;
        }

        private INonPlayerCharacter Servant(IRoom room)
        {
            string examineDescription = "They wear an dark aqua shirt and a brown sleeveless tunic.";
            string lookDescription = "Dressed in plain clothing this servant attends to the needs of their king.";
            string sentenceDescription = "kings servant";
            string shortDescription = "The kings servant.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 22);
            npc.KeyWords.Add("servant");
            npc.Personalities.Add(new Servant());

            return npc;
        }

        #endregion NPC

        #region Items
        private IEquipment Shield()
        {
            string examineDescription = "Standing four and half feet tall this shield is almost as tall as the shoppekeep.";
            string lookDescription = "The potato seems rather out of place as if the maker really liked potatoes.";
            string sentenceDescription = "potato kite shield";
            string shortDescription = "A large wooden kite shield with a potato painted on the front.";

            IShield item = CreateShield(35, examineDescription, lookDescription, sentenceDescription, shortDescription, new Wood());
            item.KeyWords.Add("Shield");
            item.KeyWords.Add("Potato");
            item.FinishLoad();
            return item;
        }

        private IEquipment Sword()
        {
            string examineDescription = "The sword is nothing special and appears to be a mass produced sword for the soldiers stationed at the castle.";
            string lookDescription = "The sword in remarkable if only in being unremarkable.";
            string sentenceDescription = "sword";
            string shortDescription = "A well balanced sword.";

            IWeapon item = CreateWeapon(WeaponType.Sword, 28, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.FinishLoad();
            return item;
        }

        private IEquipment SplitMail()
        {
            string examineDescription = "Each piece of mail has been carefully place riveted into place.";
            string lookDescription = "The green piece of splint mail still has the new look.";
            string sentenceDescription = "splint mail";
            string shortDescription = "A green splint mail.";

            IArmor item = CreateArmor(AvalableItemPosition.Body, 30, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Material = new Steel();
            item.KeyWords.Add("splint");
            item.KeyWords.Add("mail");
            item.KeyWords.Add("green");
            item.FinishLoad();
            return item;
        }

        private IEquipment Gloves()
        {
            string examineDescription = "The gloves are made of eel skin and have a green brown color that is hard to describe.";
            string lookDescription = "Each glove has a slight iridescent color that changes from green to brown and back again.";
            string sentenceDescription = "gloves";
            string shortDescription = "A pair of gloves.";

            IArmor item = CreateArmor(AvalableItemPosition.Hand, 26, examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Material = new Leather();
            item.KeyWords.Add("gloves");
            item.KeyWords.Add("eel");
            item.FinishLoad();
            return item;
        }

        private IItem Enchantery()
        {
            string examineDescription = "Green filaments of energy spark out from the table about an inch forming arches before falling back and being reabsorbed.";
            string lookDescription = "The table at one time was nothing more than some wood but has gain magical energy from hundreds nay thousands of enchantments.";
            string sentenceDescription = "enchanting table";
            string shortDescription = "An enchanting table.";

            IEnchantery item =CreateEnchantery(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.SuccessRate = -.85M;
            item.CostToEnchantLevel1Item = (int)(1.1 * item.CostToEnchantLevel1Item);
            item.KeyWords.Add("table");
            item.KeyWords.Add("enchant");
            item.KeyWords.Add("enchanting");
            item.Attributes.Add(ItemAttribute.NoGet);

            return item;
        }

        private IItem Bed()
        {
            string examineDescription = "The bed frame is made of a dark wood with carvings of the GrandView crest on the foot and headboards.";
            string lookDescription = "The bed look very soft with lots of fluffy light blue pillows.";
            string sentenceDescription = "bed";
            string shortDescription = "A canopy bed.";

            IItem item = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            item.Attributes.Add(ItemAttribute.NoGet);
            item.KeyWords.Add("bed");

            return item;
        }
        #endregion Items
    }
}
