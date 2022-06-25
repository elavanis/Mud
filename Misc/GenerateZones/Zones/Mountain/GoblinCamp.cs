using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MiscShared;
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
using Objects.Personality;
using Objects.Personality.Custom.MountainGoblinCamp;
using Objects.Personality.Interface;
using Objects.Personality.ResponderMisc;
using Objects.Personality.ResponderMisc.Interface;
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

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            return Zone;
        }


        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "A hastily made wooden gate prevents attackers from riding into camp.";
            string lookDescription = "Long tree limbs have been sharpened and lashed into a form of gate that prevents outsiders from getting into camp.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom CampOutSide(string examineDescription, string lookDescription)
        {
            string shortDescription = "Goblin Camp";
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The camp walls rise up each side to the north and south.";
            string lookDescription = "The camp appears to be well used with lots of tracks in the dirt.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "In the dim light to the north you can see the frail figures of several human prisoners.";
            string lookDescription = "A prison has been carved out of the hillside to the north.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
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
            string examineDescription = "The pen is crudely constructed and like it would fall over if the animals wanted to get out.";
            string lookDescription = "A pen to the south is where the goblins hold their war pigs.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "The pen smells of foul animal waste.";
            string lookDescription = "It smells like the goblins do not clean the pens regularly.";
            IRoom room = CampOutSide(examineDescription, lookDescription);

            for (int i = 0; i < 5; i++)
            {
                room.AddMobileObjectToRoom(WarPig(room));
            }

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "The slightly larger hut lies to the north while the smaller to the south.  The size of the huts would indicate that someone important live there.";
            string lookDescription = "Two massive huts engulf your field of view and dwarfs the other huts to the east.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "A small table for eating sits to the west while a smaller room for sleeping is to the north.  Several swords and shields spaced evenly apart decorate the walls.";
            string lookDescription = "The large room is has animal hides for a floor with several torches for lighting the area nicely.";
            string shortDescription = "Goblin Chief Hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.AddMobileObjectToRoom(GoblinChief(room));
            room.AddItemToRoom(CreateRecallBeacon());
            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "Small totems of different animal spirits sit around the fire.";
            string lookDescription = "The hut is mostly empty save for a small fire in the middle of the hut.";
            string shortDescription = "Goblin Shaman Hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.AddMobileObjectToRoom(Shaman(room));
            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "A small cooking fire burns near each of the huts.";
            string lookDescription = "Two small huts flank the path through the camp.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "A small cooking fire burns near each of the huts.";
            string lookDescription = "Two small huts flank the path through the camp.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }

        private IRoom GenerateRoom11()
        {
            string examineDescription = "A the desk has several papers on it but they are so poorly written that it makes reading impossible.";
            string lookDescription = "The hut contains a small desk for writing as well a place to sleep.";
            string shortDescription = "A goblin hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom12()
        {
            string examineDescription = "Several strips of meat hang from the hut and are slowly becoming jerky in the smoke.";
            string lookDescription = "The hut is filled with smoke making it hard to see.";
            string shortDescription = "A goblin hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom13()
        {
            string examineDescription = "Five sets of bunks extend out from the table.  Who ever was the 3rd player would won the hand with a royal flush.";
            string lookDescription = "Several small bunks extend out past a table with cards on it.";
            string shortDescription = "A goblin hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

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
            string examineDescription = "There is a carving in one of the tables.  TJ + CJ";
            string lookDescription = "Several rows of tables are in line.  A small cooking area behind a counter is in the back.";
            string shortDescription = "A goblin hut";
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom15()
        {
            string examineDescription = "The gate has a few broken pieces where it looks like has held off some attacks.";
            string lookDescription = "A large gate to the east separates the camp from the outside world.";
            IRoom room = CampOutSide(examineDescription, lookDescription);
            return room;
        }
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter WarPig(IRoom room)
        {
            string examineDescription = "The pigs fur is matted with mud and manure.";
            string lookDescription = "A war pig snorts around looking for something to eat.";
            string sentenceDescription = "goblin";
            string shortDescription = "A goblin war pig.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Other, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 16);
            npc.KeyWords.Add("pig");
            npc.KeyWords.Add("war");

            npc.Personalities.Add(new Wanderer(100));

            return npc;
        }

        private INonPlayerCharacter Shaman(IRoom room)
        {
            string examineDescription = "Wearing a pair of deer antlers and the pelts of a bear the shaman would stand out from any member of the goblin camp.";
            string lookDescription = "The shaman sways gently as he communes with spirits.";
            string sentenceDescription = "goblin";
            string shortDescription = "The camps shaman.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 40);
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

        private INonPlayerCharacter GoblinChief(IRoom room)
        {
            string examineDescription = "Goblin Chief";
            string lookDescription = "Goblin Chief";
            string sentenceDescription = "goblin";
            string shortDescription = "The camps chief.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 25);
            npc.KeyWords.Add("goblin");
            npc.KeyWords.Add("chief");
            IItem focusingCrystal = FocusingCrystal();
            npc.Items.Add(focusingCrystal);

            npc.Personalities.Add(new ChiefDaughterPresent());
            //npc.Personalities.Add(new ChiefReceiveDaughterItem(null!, null!, null!));

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
            string examineDescription = "Upon closer examination of the crystal you notice a small figurine of a fairy in the center.";
            string lookDescription = "The crystal has been cut in such a way to focus all the light entering the crystal through the bottom.";
            string sentenceDescription = "crystal";
            string shortDescription = "A small clear crystal.";
            
            IItem focusingCrystal = CreateItem(examineDescription, lookDescription, sentenceDescription, shortDescription);
            focusingCrystal.KeyWords.Add("focus");
            focusingCrystal.KeyWords.Add("focusing");
            focusingCrystal.KeyWords.Add("crystal");

            return focusingCrystal;
        }

        private Container Chest()
        {
            string examineDescription = "The chest is a standard issue goblin warrior chest.";
            string lookDescription = "The chest is made of wood and reinforced with steel bands.";
            string sentenceDescription = "chest";
            string shortDescription = "A small chest for storing equipment.";
            string openMessage = "As you open the lid of the chest you wonder what treasures it might hold.";
            string closeMessage = "The lid of the chest closes with a slight creak.";

            Container chest = CreateContainer(openMessage, closeMessage, examineDescription, lookDescription, sentenceDescription, shortDescription);
            chest.KeyWords.Add("chest");
            chest.Attributes.Add(ItemAttribute.NoGet);

            return chest;
        }

        private IArmor Arms()
        {
            string examineDescription = "The lamb skin is flawless.  Maybe the original owner thought it would grant the wearer extra protection.";
            string lookDescription = "The sleeves are made in such a way as to have soft wool on the inside and out.";
            string sentenceDescription = "sleeves";
            string shortDescription = "A pair of lamb skin sleeves.";

            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("lamb");
            armor.KeyWords.Add("skin");
            armor.KeyWords.Add("sleeve");

            return armor;
        }

        private IArmor Body()
        {
            string examineDescription = "The bear skin is made in such a way as to have a hood of sorts that can be flipped up.";
            string lookDescription = "The bear skin is made from a hodgepodge of several different bears.";
            string sentenceDescription = "bearskin";
            string shortDescription = "A bearskin.";

            IArmor armor = CreateArmor( AvalableItemPosition.Body, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.ItemPosition = AvalableItemPosition.Body;
            armor.Material = new Leather();
            armor.KeyWords.Add("bear");
            armor.KeyWords.Add("skin");

            return armor;
        }

        private IArmor Feet()
        {
            string examineDescription = "The boots are have been dyed to a dark black color.";
            string lookDescription = "The boots are made of a type of leather sew together.";
            string sentenceDescription = "boots";
            string shortDescription = "A dark black pair of boats.";

            IArmor armor = CreateArmor(AvalableItemPosition.Feet, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("black");
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("boots");

            return armor;
        }

        private IArmor Finger()
        {
            string examineDescription = "The gem is made of a red stone with white veins that look like a swirl frozen in time.";
            string lookDescription = "The ring is made of a thick gold band with a red stone in the center.";
            string sentenceDescription = "ring";
            string shortDescription = "Gold Ring.";

            IArmor armor = CreateArmor( AvalableItemPosition.Finger, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Gold());
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
            string examineDescription = "The gloves are made of a delicate silver lace that sparkles in the light.";
            string lookDescription = "A pair of silver lace gloves that would make a grand statement at any ball.";
            string sentenceDescription = "lace gloves";
            string shortDescription = "A silver pair of ballroom lace gloves.";

            IArmor armor = CreateArmor(AvalableItemPosition.Hand, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Cloth());
            armor.KeyWords.Add("silver");
            armor.KeyWords.Add("lace");
            armor.KeyWords.Add("gloves");

            return armor;
        }

        private IArmor Head()
        {
            string examineDescription = "The leather skull cap has two holes for a goblins ears to stick through.";
            string lookDescription = "The leather skull cap is padded to help protect the wearer from blows.";
            string sentenceDescription = "skull cap";
            string shortDescription = "A leather skull cap.";

            IArmor armor = CreateArmor( AvalableItemPosition.Head, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("skull");
            armor.KeyWords.Add("cap");
            armor.KeyWords.Add("leather");

            return armor;
        }

        private IArmor Legs()
        {
            string examineDescription = "Delicately carved gold inlays decorate this piece of museum quality piece of armor.";
            string lookDescription = "The steel leggings look to be more decorative than protective but will do the job when needed.";
            string sentenceDescription = "leggings";
            string shortDescription = "A decorative pair of leggings.";

            IArmor armor = CreateArmor(AvalableItemPosition.Legs, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Steel());
            armor.KeyWords.Add("leggings");
            armor.KeyWords.Add("decorative");

            return armor;
        }

        private IArmor Neck()
        {
            string examineDescription = "The necklace bones appear to be of small animals, birds, squirrels and mice.";
            string lookDescription = "The necklace looks like it once belonged to a shaman and has several animal bones strung on it.";
            string sentenceDescription = "necklace";
            string shortDescription = "A bone necklace.";

            IArmor armor = CreateArmor( AvalableItemPosition.Neck, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Bone());
            armor.KeyWords.Add("necklace");
            armor.KeyWords.Add("bone");

            return armor;
        }

        private IArmor Waist()
        {
            string examineDescription = "The piece of rope looks unremarkable in every way.";
            string lookDescription = "A simple piece of rope for holding your trousers.";
            string sentenceDescription = "rope";
            string shortDescription = "A short piece of rope.";

            IArmor armor = CreateArmor(AvalableItemPosition.Waist, 17, examineDescription, lookDescription, sentenceDescription, shortDescription, new Cloth());
            armor.KeyWords.Add("rope");

            return armor;
        }
        #endregion Items

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.East, Zone.Rooms[2], new DoorInfo("gate", "The gate drags across the dirt as it opens.", "The gate drags across the dirt as it closes.", true, "The gate is made of sturdy wooden tree trunks and looks to be able to take a beating."));
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.South, Zone.Rooms[5], new DoorInfo("gate", "The gate drags across the dirt as it opens.", "The gate drags across the dirt as it closes.", true, "The gate is made of flimsy sticks and acts more of a mental barrier than a physical one."));
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
            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.West, 17, 11);
        }
    }
}
