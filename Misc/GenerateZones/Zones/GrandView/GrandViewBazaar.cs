using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Magic;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Zone;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Damage;
using Objects.Global.DefaultValues;
using Objects.Item.Items;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Material.Materials;
using Objects.Personality.Personalities;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Item.Items.Interface;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Weapon;
using static Objects.Item.Items.Equipment;

namespace GenerateZones.Zones
{
    public class GrandViewBazaar : BaseZone, IZoneCode
    {
        public GrandViewBazaar() : base(4)
        {
        }

        public IZone Generate()
        {
            Zone.InGameDaysTillReset = 1;
            Zone.Name = nameof(GrandViewBazaar);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            AddFlavorNpc(6);

            ConnectRooms();

            return Zone;
        }



        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Tapestries of every color adore the bazaar here.  The one on the right might be look good in your house.  Further in the back you can see the seamstresses work.";
            room.LongDescription = "Colorful tapestries and other cloth materials hang on display all around you.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = CreateRoom();
            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "There are all kinds of fish here.  Bass, trout and walleye as well as ocean imports swordfish and shark.  For the adventurous there are squid and eel.  In the north side of the fish market there is some live lobster and crab.";
            room.LongDescription = "The smell of fish assault your nostrils.  Everywhere you look you see fish.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Wheels of fresh Gouda and mozzarella are on display on the left.  Fresh baguettes and other loaves of breads are on the right.";
            room.LongDescription = "The smell of fresh breeds and cheeses emanate from the market.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Wooden bowls come in all shapes and sizes.  Carved from the most beautiful woods some have beautiful designs while other are the more economical choice variety.";
            room.LongDescription = "Wooden chairs tables, carts and other things lay littered on the ground.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "With so many things on display its actually not as bad to maneuver though here as you might think.  The organization leaves a bit to be desired but its not cluttered.  Pieces range in quality and in price.  While the life size statue of Charon might not be practical maybe a necklace with Charon's symbol on it might be good.";
            room.LongDescription = "This appears to be the metalworkers area.  Metal utensils, small figurines as well as swords, shields and entire suits of armor are on display here.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "Leather items range from furniture to pieces of armor.  They come are available in all shapes and sizes.";
            room.LongDescription = "Tanned leather pieces are hung for purchase as well as finished products.";
            room.ShortDescription = "Bazaar";

            return room;
        }

        private INonPlayerCharacter MalePatron()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 4);
            npc.KeyWords.Add("patron");
            npc.KeyWords.Add("male");
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "Dressed in a {adjective} tunic of {color1} and {color2} he wanders the bazaar looking for some {item} for his {target}.";
            npc.LongDescription = "He seems to have a list of items he is looking for.";
            npc.ShortDescription = "A male patron.";
            npc.SentenceDescription = "patron";

            List<string> adjective = new List<string>() { "fine", "nice", "worn", "tattered" };
            List<string> colors1 = new List<string>() { "red", "blue", "green", "white", "black", "brown" };
            List<string> colors2 = new List<string>() { "orange", "yellow", "gray", "tan" };
            List<string> item = new List<string>() { "food", "clothing", "bedding" };
            List<string> target = new List<string>() { "wife", "girlfriend", "horse" };

            npc.FlavorOptions.Add("adjective", adjective);
            npc.FlavorOptions.Add("colors1", colors1);
            npc.FlavorOptions.Add("colors2", colors2);
            npc.FlavorOptions.Add("item", item);
            npc.FlavorOptions.Add("target", target);

            return npc;
        }

        private INonPlayerCharacter FemalePatron()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 4);
            npc.KeyWords.Add("patron");
            npc.KeyWords.Add("female");
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "Dressed in a {adjective} tunic of {color1} and {color2} she wanders the bazaar looking for some {item} for her {target}.";
            npc.LongDescription = "She seems to have a list of items she is looking for.";
            npc.ShortDescription = "A female patron.";
            npc.SentenceDescription = "patron";

            List<string> adjective = new List<string>() { "fine", "nice", "worn", "tattered" };
            List<string> colors1 = new List<string>() { "red", "blue", "green", "white", "black", "brown" };
            List<string> colors2 = new List<string>() { "orange", "yellow", "gray", "tan" };
            List<string> item = new List<string>() { "food", "clothing", "bedding" };
            List<string> target = new List<string>() { "husband", "boyfriend", "maid" };

            npc.FlavorOptions.Add("adjective", adjective);
            npc.FlavorOptions.Add("colors1", colors1);
            npc.FlavorOptions.Add("colors2", colors2);
            npc.FlavorOptions.Add("item", item);
            npc.FlavorOptions.Add("target", target);

            return npc;
        }

        private INonPlayerCharacter Beggar()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 14);
            npc.KeyWords.Add("beggar");

            Speaker speaker = new Speaker();
            speaker.ThingsToSay.Add("Do you have any spare coins?");
            speaker.ThingsToSay.Add("Spare some coins for a old man?");
            speaker.ThingsToSay.Add("I used to be a prince in my homeland.");
            speaker.ThingsToSay.Add("Do you have any food?");
            npc.Personalities.Add(speaker);

            npc.ExamineDescription = "Dressed in a tattered tunic of that is mainly the color of mud the beggar raises a cup to you and asks for change.";
            npc.LongDescription = "Ignored my most people this beggar calls out for money to anyone who will listen.";
            npc.ShortDescription = "A beggar.";
            npc.SentenceDescription = "beggar";

            return npc;
        }
        private void AddFlavorNpc(int flavorRooms)
        {
            for (int pos = 1; pos <= flavorRooms; pos++)
            {
                IRoom room = Zone.Rooms[pos];

                for (int mob = 0; mob < 3; mob++)
                {
                    switch (mob)
                    {
                        case 0:
                            room.AddMobileObjectToRoom(MalePatron());
                            break;
                        case 1:
                            room.AddMobileObjectToRoom(FemalePatron());
                            break;
                        case 2:
                            room.AddMobileObjectToRoom(Beggar());
                            break;
                    }
                }
            }
        }

        #region BlackSmith
        private IRoom GenerateRoom7()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "Anything that you can could need for self protection exists in this shop.  It only a matter of finding something you like.";
            room.LongDescription = "Swords, shields and armor adorn the walls of the little shop.  While mannequins display items on the floor.";
            room.ShortDescription = "The Basic Dagger";

            INonPlayerCharacter npc = BlackSmith();

            room.AddMobileObjectToRoom(npc);

            return room;
        }

        private INonPlayerCharacter BlackSmith()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 5);

            Merchant merchant = new Merchant();

            merchant.Sellables.Add(BlackSmithDagger());
            merchant.Sellables.Add(BlackSmithBreastPlate());

            npc.Personalities.Add(merchant);
            npc.Personalities.Add(new Craftsman());

            npc.KeyWords.Add("Merchant");
            npc.KeyWords.Add("shop");
            npc.KeyWords.Add("keeper");
            npc.KeyWords.Add("shopkeeper");
            npc.KeyWords.Add("blacksmith");

            npc.ShortDescription = "A shop keeper.";
            npc.LongDescription = "The shop keeper continues to work on a sword.  Banging his hammer on a sword, quenching in the water than putting it back in the furnace.";
            npc.ExamineDescription = "The shop keeper looks to be in his twenties.  At this stage in life he probably hasn't had enough experience to make any master level equipment but we all have to start somewhere.";
            npc.SentenceDescription = "blacksmith";

            return npc;
        }

        private IWeapon BlackSmithDagger()
        {
            IWeapon dagger = CreateWeapon(WeaponType.Dagger, 1);
            dagger.KeyWords.Add("Dagger");
            dagger.ShortDescription = "A basic dagger.";
            dagger.LongDescription = "Made of steel it is a sharp and pointy dagger.";
            dagger.ExamineDescription = "The dagger lacks any intricate engravings, the blade is however is very sharp and is thick enough to attack an enemy without fear of breaking.  It is in all regards a basic dagger.";
            dagger.SentenceDescription = "small dagger";
            dagger.AttackerStat = Stats.Stat.Dexterity;
            dagger.DeffenderStat = Stats.Stat.Dexterity;

            Damage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(dagger.Level);
            damage.Type = Damage.DamageType.Slash;
            dagger.DamageList.Add(damage);
            dagger.FinishLoad();
            return dagger;
        }

        private IArmor BlackSmithBreastPlate()
        {
            IArmor breastPlate = CreateArmor(AvalableItemPosition.Body, 1, new Steel());
            breastPlate.KeyWords.Add("BreastPlate");
            breastPlate.KeyWords.Add("Breast");
            breastPlate.KeyWords.Add("Plate");
            breastPlate.ShortDescription = "A steel breast plate.";
            breastPlate.LongDescription = "Made of steel this breast plate is rather heavy but effective.";
            breastPlate.ExamineDescription = "Examining the plate closer you notice there is a knick in the front right, a dent in the upper left and the one of the straps for holding it together is starting to tear.  Maybe the reason this was such a good deal was because it was used.";
            breastPlate.SentenceDescription = "breastplate";
            breastPlate.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(breastPlate.Level);
            breastPlate.FinishLoad();
            return breastPlate;
        }
        #endregion BlackSmith


        #region Leather Worker
        private IRoom GenerateRoom8()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "The smell of fresh leather drifts through the shop.  The various shades of browns give the shop a warm look and in the evening the light of the setting sun can be seen entering through the front of the store.  It reflects off the leather samples in the store and creates a nice warm inviting atmosphere, so much so that the shop keeper says that over 20% of her business come at sunset.";
            room.LongDescription = "Different type of leather armor line one wall while raw materials line the other.";
            room.ShortDescription = "Hyde's Hides";

            INonPlayerCharacter npc = LeatherWorker();

            room.AddMobileObjectToRoom(npc);

            return room;
        }

        private INonPlayerCharacter LeatherWorker()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 5);
            Merchant merchant = new Merchant();

            merchant.Sellables.Add(LeatherSmithBracer());
            merchant.Sellables.Add(LeatherSmithLeggings());

            npc.Personalities.Add(merchant);
            npc.Personalities.Add(new Craftsman());

            npc.KeyWords.Add("Merchant");
            npc.KeyWords.Add("shop");
            npc.KeyWords.Add("keeper");
            npc.KeyWords.Add("shopkeeper");
            npc.KeyWords.Add("leather");
            npc.KeyWords.Add("worker");
            npc.KeyWords.Add("leather");
            npc.KeyWords.Add("leatherworker");

            npc.ShortDescription = "A leather worker.";
            npc.LongDescription = "The female leather worker stares intently at a piece of leather.  Turning it this way and that as if trying to imaging in her mind what the piece wants to be.";
            npc.ExamineDescription = "The leather worker has shoulder length auburn hair tied in a pony tail.  Her clothing is mostly fine leather, perhaps self made.";
            npc.SentenceDescription = "leather worker";

            return npc;
        }

        private IArmor LeatherSmithBracer()
        {
            IArmor bracer = CreateArmor(AvalableItemPosition.Arms, 1, new Leather());
            bracer.KeyWords.Add("Bracer");
            bracer.KeyWords.Add("Leather");
            bracer.ShortDescription = "A leather bracer.";
            bracer.LongDescription = "The leather is hard and will offer some basic protection from sharp objects.";
            bracer.ExamineDescription = "A small woven design is made weaves back and forth over the leather bracer.  It is a slightly darker color than the rich brown of the rest of the bracer.";
            bracer.SentenceDescription = "bracer";
            bracer.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(bracer.Level);
            bracer.FinishLoad();
            return bracer;
        }

        private IArmor LeatherSmithLeggings()
        {
            IArmor leggings = CreateArmor(AvalableItemPosition.Legs, 1, new Leather());
            leggings.KeyWords.Add("Leggings");
            leggings.KeyWords.Add("Leather");
            leggings.ShortDescription = "A pair of leather leggings.";
            leggings.LongDescription = "Simple but effective these leggings will protect you next time someone decides to kick you in the shins.  Take that 5 grade bully.";
            leggings.ExamineDescription = "The leather leggings stop just short of you knees leaving you needing some knee pads.  While this leaves you open to kicks to the knee caps it does give you a little more speed so hopefully you can dodge them.";
            leggings.SentenceDescription = "leggings";
            leggings.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(leggings.Level);
            leggings.FinishLoad();
            return leggings;
        }
        #endregion Leather Worker


        #region Tailor 
        private IRoom GenerateRoom9()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "The black tailored suit and the purple ball gown are most exquisite.  Both are tucked in just the right places to show of the wearers figure and show an eye for detail by the tailor.";
            room.LongDescription = "Fine tailored suits and formal gowns are displayed in the front window.  The armor is in the back half of the store.";
            room.ShortDescription = "The better than nothing armor shop";

            INonPlayerCharacter npc = Tailor();

            room.AddMobileObjectToRoom(npc);

            return room;
        }

        private INonPlayerCharacter Tailor()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 5);
            Merchant merchant = new Merchant();

            merchant.Sellables.Add(ClothBoots());
            merchant.Sellables.Add(ClothSash());

            npc.Personalities.Add(merchant);
            npc.Personalities.Add(new Craftsman());

            npc.KeyWords.Add("Merchant");
            npc.KeyWords.Add("shop");
            npc.KeyWords.Add("keeper");
            npc.KeyWords.Add("shopkeeper");
            npc.KeyWords.Add("tailor");
            npc.KeyWords.Add("Taylor");

            npc.ShortDescription = "A tailor.";
            npc.LongDescription = "Talking to the tailor you find out that her name is Taylor.  Its almost as if the gods that control the world though it would be funny to have her become a tailor in life.";
            npc.ExamineDescription = "Talking with Taylor some more you she tells you she wanted to be a seamstress when she grew up but when her dad became ill she had to take over the family, how the gods are cruel for not letting her fulfill her life dream to become a seamstress and since she has enjoyed your company if you buy today she'll give you a 10% discount.";
            npc.SentenceDescription = "tailor";

            return npc;
        }

        private IArmor ClothBoots()
        {
            IArmor boots = CreateArmor(AvalableItemPosition.Feet, 1, new Cloth());
            boots.KeyWords.Add("boots");
            boots.KeyWords.Add("cloth");
            boots.ShortDescription = "A pair of nice boots.";
            boots.LongDescription = "The boots are made of a thick black material.  Soft and subtle yet sturdy.";
            boots.ExamineDescription = "Made from a unknown type of cloth they offer a fair amount of protection while being bendable enough to not notice.  They come to about half way up the calf lending some stability especially when the laces are drawn tight.";
            boots.SentenceDescription = "boots";
            boots.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(boots.Level);
            boots.FinishLoad();
            return boots;
        }

        private IArmor ClothSash()
        {
            IArmor sash = CreateArmor(AvalableItemPosition.Waist, 1, new Cloth());
            sash.KeyWords.Add("sash");
            sash.KeyWords.Add("cloth");
            sash.ShortDescription = "A dashing red sash.";
            sash.LongDescription = "While it serves no practice purpose other than to cover up your waist it does look dashing.";
            sash.ExamineDescription = "Made of a silky red material it manages to do a good job of covering your waist while making the wearer look better.";
            sash.SentenceDescription = "sash";
            sash.Charisma = 2;
            sash.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(sash.Level);
            sash.FinishLoad();

            sash.Value = (ulong)(sash.Value * 1.2);
            return sash;
        }
        #endregion Tailor
        #endregion Rooms

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.North, 3, 1);
            ZoneHelper.ConnectZone(Zone.Rooms[6], Direction.East, 9, 61);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.South, Zone.Rooms[9]);
        }
    }
}
