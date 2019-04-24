using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Objects.Die;
using Objects.Effect;
using Objects.Effect.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Language;
using Objects.Language.Interface;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Material.Materials;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.Interface;
using Objects.Personality.Personalities.ResponderMisc;
using Objects.Personality.Personalities.ResponderMisc.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using static Objects.Global.Direction.Directions;
using static Objects.Item.Item;
using static Objects.Item.Items.Equipment;
using static Objects.Mob.NonPlayerCharacter;
using static Shared.TagWrapper.TagWrapper;

namespace GenerateZones.Zones.Mountain
{
    public class GoblinCamp : BaseZone, IZoneCode
    {
        public GoblinCamp() : base(16)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(GoblinCamp);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (Room)method.Invoke(this, null);
                    room.Zone = Zone.Id;
                    ZoneHelper.AddRoom(Zone, room);
                }
            }

            ConnectRooms();

            return Zone;
        }


        #region Rooms
        private IRoom GenerateRoom1()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "A hastily made wooden gate prevents attackers from riding into camp.";
            room.LookDescription = "Long tree limbs have been sharpened and lashed into a form of gate that prevents outsiders from getting into camp.";

            return room;
        }

        private IRoom CampOutSide()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);


            room.ShortDescription = "Goblin Camp";
            return room;
        }

        private IRoom CampInside()
        {
            IRoom room = CreateRoom();
            room.Attributes.Add(Room.RoomAttribute.Indoor);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "The camp walls rise up each side to the north and south.";
            room.LookDescription = "The camp appears to be well used with lots of tracks in the dirt.";

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "In the dim light to the north you can see the frail figures of several human prisoners.";
            room.LookDescription = "A prison has been carved out of the hillside to the north.";

            room.Enchantments.Add(PrisonerEnter(Zone.Id, room.Id));
            return room;
        }

        private IEnchantment PrisonerEnter(int zoneId, int roomId)
        {
            IEnchantment enchantment = new EnterRoomEnchantment();
            enchantment.ActivationPercent = 100;

            IEffect effect = new Message();
            enchantment.Effect = effect;

            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.RoomMessage = new TranslationMessage("A prisoner shouts \"Let us out of here.\"");
            effectParameter.RoomId = new RoomId(zoneId, roomId);
            enchantment.Parameter = effectParameter;

            return enchantment;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "The pen is crudely constructed and like it would fall over if the animals wanted to get out.";
            room.LookDescription = "A pen to the south is where the goblins hold their war pigs.";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "The pen smells of foul animal waste.";
            room.LookDescription = "It smells like the goblins do not clean the pens regularly.";

            for (int i = 0; i < 5; i++)
            {
                room.AddMobileObjectToRoom(WarPig());
            }

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "The slightly larger hut lies to the north while the smaller to the south.  The size of the huts would indicate that someone important live there.";
            room.LookDescription = "Two massive huts engulf your field of view and dwarfs the other huts to the east.";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = CampInside();
            room.ShortDescription = "Goblin Chief Hut";

            room.ExamineDescription = "A small table for eating sits to the west while a smaller room for sleeping is to the north.  Several swords and shields spaced evenly apart decorate the walls.";
            room.LookDescription = "The large room is has animal hides for a floor with several torches for lighting the area nicely.";

            room.AddMobileObjectToRoom(GoblinChief());

            room.AddItemToRoom(CreateItem<IRecallBeacon>());
            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = CampInside();
            room.ShortDescription = "Goblin Shaman Hut";

            room.ExamineDescription = "Small totems of different animal spirits sit around the fire.";
            room.LookDescription = "The hut is mostly empty save for a small fire in the middle of the hut.";

            room.AddMobileObjectToRoom(Shaman());
            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "A small cooking fire burns near each of the huts.";
            room.LookDescription = "Two small huts flank the path through the camp.";

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "A small cooking fire burns near each of the huts.";
            room.LookDescription = "Two small huts flank the path through the camp.";

            return room;
        }

        private IRoom GenerateRoom11()
        {
            IRoom room = CampInside();
            room.ExamineDescription = "A the desk has several papers on it but they are so poorly written that it makes reading impossible.";
            room.LookDescription = "The hut contains a small desk for writing as well a place to sleep.";
            room.ShortDescription = "A goblin hut";

            return room;
        }

        private IRoom GenerateRoom12()
        {
            IRoom room = CampInside();
            room.ExamineDescription = "Several strips of meat hang from the hut and are slowly becoming jerky in the smoke.";
            room.LookDescription = "The hut is filled with smoke making it hard to see.";
            room.ShortDescription = "A goblin hut";

            return room;
        }

        private IRoom GenerateRoom13()
        {
            IRoom room = CampInside();
            room.ExamineDescription = "Five sets of bunks extend out from the table.  Who ever was the 3rd player would won the hand with a royal flush.";
            room.LookDescription = "Several small bunks extend out past a table with cards on it.";
            room.ShortDescription = "A goblin hut";

            Container container = Chest();
            container.Items.Add(Arms());
            container.Items.Add(Head());
            room.AddItemToRoom(container);

            container = Chest();
            container.Items.Add(Body());
            container.Items.Add(Legs());
            room.AddItemToRoom(container);

            container = Chest();
            container.Items.Add(Feet());
            container.Items.Add(Neck());
            room.AddItemToRoom(container);

            container = Chest();
            container.Items.Add(Finger());
            container.Items.Add(Waist());
            room.AddItemToRoom(container);

            container = Chest();
            container.Items.Add(Hand());
            room.AddItemToRoom(container);
            return room;
        }

        private IRoom GenerateRoom14()
        {
            IRoom room = CampInside();
            room.ExamineDescription = "There is a carving in one of the tables.  TJ + CJ";
            room.LookDescription = "Several rows of tables are in line.  A small cooking area behind a counter is in the back.";
            room.ShortDescription = "A goblin hut";

            return room;
        }

        private IRoom GenerateRoom15()
        {
            IRoom room = CampOutSide();
            room.ExamineDescription = "The gate has a few broken pieces where it looks like has held off some attacks.";
            room.LookDescription = "A large gate to the east separates the camp from the outside world.";

            return room;
        }
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter WarPig()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, 16);

            npc.ExamineDescription = "The pigs fur is matted with mud and manure.";
            npc.LookDescription = "A war pig snorts around looking for something to eat.";
            npc.ShortDescription = "A goblin war pig.";
            npc.SentenceDescription = "goblin";
            npc.KeyWords.Add("pig");
            npc.KeyWords.Add("war");

            npc.Personalities.Add(new Wanderer(100));

            return npc;
        }

        private INonPlayerCharacter Shaman()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 40);

            npc.ExamineDescription = "Wearing a pair of deer antlers and the pelts of a bear the shaman would stand out from any member of the goblin camp.";
            npc.LookDescription = "The shaman sways gently as he communes with spirits.";
            npc.ShortDescription = "The camps shaman.";
            npc.SentenceDescription = "goblin";
            npc.KeyWords.Add("goblin");
            npc.KeyWords.Add("shaman");

            IEnchantment enchantment = new EnterRoomEnchantment();
            IEffect say = new Message();
            IEffectParameter effectParameter = new EffectParameter();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = say;
            TranslationPair translationPair = new TranslationPair(Objects.Global.Language.Translator.Languages.Goblin, "The spirits said you would come.");
            List<ITranslationPair> translationPairs = new List<ITranslationPair>() { translationPair };
            effectParameter.RoomMessage = new TranslationMessage("The Shaman says \"{0}\"", TagType.Communication, translationPairs);
            effectParameter.RoomId = new RoomId(Zone.Id, 8);
            enchantment.Parameter = effectParameter;

            npc.Enchantments.Add(enchantment);

            return npc;
        }

        private INonPlayerCharacter GoblinChief()
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, 25);

            npc.ExamineDescription = "Goblin Chief";
            npc.LookDescription = "Goblin Chief";
            npc.ShortDescription = "The camps chief.";
            npc.SentenceDescription = "goblin";
            npc.KeyWords.Add("goblin");
            npc.KeyWords.Add("chief");
            IItem focusingCrystal = FocusingCrystal();
            npc.Items.Add(focusingCrystal);

            IResponder responder = new Responder();
            List<IOptionalWords> listOptionalWords = new List<IOptionalWords>();
            IOptionalWords optionalWords = new OptionalWords();
            optionalWords.TriggerWords.Add("daughter");
            responder.Responses.Add(new Response() { RequiredWordSets = listOptionalWords, Message = "The evil enchanter that lives at the top of the mountain has taken my daughter hostage.  If you should retrieve her for me there would be a reward for you." });
            npc.Personalities.Add(responder);

            return npc;
        }

        #endregion NPC

        #region Items
        private IItem FocusingCrystal()
        {
            IItem focusingCrystal = CreateItem<IItem>();
            focusingCrystal.ShortDescription = "A small clear crystal.";
            focusingCrystal.LookDescription = "The crystal has been cut in such a way to focus all the light entering the crystal through the bottom.";
            focusingCrystal.ExamineDescription = "Upon closer examination of the crystal you notice a small figurine of a fairy in the center.";
            focusingCrystal.SentenceDescription = "crystal";
            focusingCrystal.KeyWords.Add("focus");
            focusingCrystal.KeyWords.Add("focusing");
            focusingCrystal.KeyWords.Add("crystal");

            return focusingCrystal;
        }

        private Container Chest()
        {
            Container chest = CreateItem<Container>();
            chest.ExamineDescription = "The chest is a standard issue goblin warrior chest.";
            chest.LookDescription = "The chest is made of wood and reinforced with steel bands.";
            chest.ShortDescription = "A small chest for storing equipment.";
            chest.SentenceDescription = "chest";
            chest.KeyWords.Add("chest");
            chest.Attributes.Add(ItemAttribute.NoGet);

            return chest;
        }

        private IArmor Arms()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 17, new Leather());
            armor.ExamineDescription = "The lamb skin is flawless.  Maybe the original owner thought it would grant the wearer extra protection.";
            armor.LookDescription = "The sleeves are made in such a way as to have soft wool on the inside and out.";
            armor.ShortDescription = "A pair of lamb skin sleeves.";
            armor.SentenceDescription = "sleeves";
            armor.KeyWords.Add("lamb");
            armor.KeyWords.Add("skin");
            armor.KeyWords.Add("sleeve");

            return armor;
        }

        private IArmor Body()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Body, 17, new Leather());
            armor.ItemPosition = AvalableItemPosition.Body;
            armor.Material = new Leather();
            armor.ExamineDescription = "The bear skin is made in such a way as to have a hood of sorts that can be flipped up.";
            armor.LookDescription = "The bear skin is made from a hodgepodge of several different bears.";
            armor.ShortDescription = "A bearskin.";
            armor.SentenceDescription = "bearskin";
            armor.KeyWords.Add("bear");
            armor.KeyWords.Add("skin");

            return armor;
        }

        private IArmor Feet()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Feet, 17, new Leather());
            armor.ExamineDescription = "The boots are have been dyed to a dark black color.";
            armor.LookDescription = "The boots are made of a type of leather sew together.";
            armor.ShortDescription = "A dark black pair of boats.";
            armor.SentenceDescription = "boots";
            armor.KeyWords.Add("black");
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("boots");

            return armor;
        }

        private IArmor Finger()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Finger, 17, new Gold());
            armor.ExamineDescription = "The gem is made of a red stone with white veins that look like a swirl frozen in time.";
            armor.LookDescription = "The ring is made of a thick gold band with a red stone in the center.";
            armor.ShortDescription = "Gold Ring.";
            armor.SentenceDescription = "ring";
            armor.KeyWords.Add("red");
            armor.KeyWords.Add("stone");
            armor.KeyWords.Add("gold");
            armor.KeyWords.Add("ring");

            IEnchantment enchantment = new HeartbeatBigTickEnchantment();
            enchantment.Effect = new RecoverStamina();
            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.Dice = new Dice(1, 1);
            enchantment.Parameter = effectParameter;

            return armor;
        }

        private IArmor Hand()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Hand, 17, new Cloth());
            armor.ExamineDescription = "The gloves are made of a delicate silver lace that sparkles in the light.";
            armor.LookDescription = "A pair of silver lace gloves that would make a grand statement at any ball.";
            armor.ShortDescription = "A silver pair of ballroom lace gloves.";
            armor.SentenceDescription = "lace gloves";
            armor.KeyWords.Add("silver");
            armor.KeyWords.Add("lace");
            armor.KeyWords.Add("gloves");

            return armor;
        }

        private IArmor Head()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Head, 17, new Leather());
            armor.ExamineDescription = "The leather skull cap has two holes for a goblins ears to stick through.";
            armor.LookDescription = "The leather skull cap is padded to help protect the wearer from blows.";
            armor.ShortDescription = "A leather skull cap.";
            armor.SentenceDescription = "skull cap";
            armor.KeyWords.Add("skull");
            armor.KeyWords.Add("cap");
            armor.KeyWords.Add("leather");

            return armor;
        }

        private IArmor Legs()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Legs, 17, new Steel());
            armor.ExamineDescription = "Delicately carved gold inlays decorate this piece of museum quality piece of armor.";
            armor.LookDescription = "The steel leggings look to be more decorative than protective but will do the job when needed.";
            armor.ShortDescription = "A decorative pair of leggings.";
            armor.SentenceDescription = "leggings";
            armor.KeyWords.Add("leggings");
            armor.KeyWords.Add("decorative");

            return armor;
        }

        private IArmor Neck()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Neck, 17, new Bone());
            armor.ExamineDescription = "The necklace bones appear to be of small animals, birds, squirrels and mice.";
            armor.LookDescription = "The necklace looks like it once belonged to a shaman and has several animal bones strung on it.";
            armor.ShortDescription = "A bone necklace.";
            armor.SentenceDescription = "necklace";
            armor.KeyWords.Add("necklace");
            armor.KeyWords.Add("bone");

            return armor;
        }

        private IArmor Waist()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Waist, 17, new Cloth());
            armor.ExamineDescription = "The piece of rope looks unremarkable in every way.";
            armor.LookDescription = "A simple piece of rope for holding your trousers.";
            armor.ShortDescription = "A short piece of rope.";
            armor.SentenceDescription = "rope";
            armor.KeyWords.Add("rope");

            return armor;
        }
        #endregion Items

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2], new DoorInfo("gate", "The gate drags across the dirt as it opens.", true, "The gate is made of sturdy wooden tree trunks and looks to be able to take a beating."));
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[5], new DoorInfo("gate", "The gate drags across the dirt as it opens.", true, "The gate is made of flimsy sticks and acts more of a mental barrier than a physical one."));
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.South, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.East, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.North, Zone.Rooms[11]);
            ZoneHelper.ConnectRoom(Zone.Rooms[9], Direction.South, Zone.Rooms[12]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.North, Zone.Rooms[13]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.South, Zone.Rooms[14]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[15]);

            ZoneHelper.ConnectZone(Zone.Rooms[15], Direction.East, 19, 1);
        }
    }
}
