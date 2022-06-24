using System.Collections.Generic;
using System.Linq;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Room.Interface;
using Objects.Room;
using Objects.Trap.Interface;
using Objects.Trap;
using Objects.Magic.Interface;
using Objects.Magic.Enchantment;
using static Objects.Global.Direction.Directions;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Material.Materials;
using Objects.Global;
using Objects.Damage.Interface;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Mob.Interface;
using Objects.Personality;
using static Objects.Damage.Damage;
using Objects.Die.Interface;
using Objects;
using Objects.Language;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;
using MiscShared;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class KoboldLair : BaseZone, IZoneCode
    {
        public KoboldLair() : base(12)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(KoboldLair);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();
            //AddSounds();

            return Zone;
        }

        private void ConnectRooms()
        {
            ZoneHelper.ConnectRoom(Zone.Rooms[1], Direction.South, Zone.Rooms[2]);
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.East, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.East, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.East, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.East, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.North, Zone.Rooms[7]);
            ZoneHelper.ConnectRoom(Zone.Rooms[7], Direction.North, Zone.Rooms[8]);
            ZoneHelper.ConnectRoom(Zone.Rooms[8], Direction.North, Zone.Rooms[9]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.East, Zone.Rooms[10]);
            ZoneHelper.ConnectRoom(Zone.Rooms[10], Direction.East, Zone.Rooms[11]);

            ZoneHelper.ConnectZone(Zone.Rooms[1], Direction.North, 8, 80);

            #region Mine
            #region Mine Shaft
            for (int i = 11; i < 21; i++)
            {
                ZoneHelper.ConnectRoom(Zone.Rooms[i], Direction.Down, Zone.Rooms[i + 1]);
            }
            #endregion Mine Shaft

            #region Level 2
            ZoneHelper.ConnectRoom(Zone.Rooms[12], Direction.East, Zone.Rooms[25]);
            ZoneHelper.ConnectRoom(Zone.Rooms[25], Direction.East, Zone.Rooms[26]);
            ZoneHelper.ConnectRoom(Zone.Rooms[26], Direction.North, Zone.Rooms[27]);
            ZoneHelper.ConnectRoom(Zone.Rooms[27], Direction.North, Zone.Rooms[28]);
            ZoneHelper.ConnectRoom(Zone.Rooms[28], Direction.West, Zone.Rooms[29]);
            #endregion Level 2

            #region Level 3
            ZoneHelper.ConnectRoom(Zone.Rooms[14], Direction.West, Zone.Rooms[30]);
            ZoneHelper.ConnectRoom(Zone.Rooms[30], Direction.West, Zone.Rooms[31]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.West, Zone.Rooms[32]);
            ZoneHelper.ConnectRoom(Zone.Rooms[32], Direction.West, Zone.Rooms[33]);
            ZoneHelper.ConnectRoom(Zone.Rooms[33], Direction.South, Zone.Rooms[34]);
            ZoneHelper.ConnectRoom(Zone.Rooms[34], Direction.West, Zone.Rooms[35]);
            ZoneHelper.ConnectRoom(Zone.Rooms[31], Direction.North, Zone.Rooms[36]);
            ZoneHelper.ConnectRoom(Zone.Rooms[36], Direction.North, Zone.Rooms[37]);
            ZoneHelper.ConnectRoom(Zone.Rooms[37], Direction.East, Zone.Rooms[38]);
            ZoneHelper.ConnectRoom(Zone.Rooms[38], Direction.South, Zone.Rooms[39]);
            #endregion Level 3

            #region Level 4
            ZoneHelper.ConnectRoom(Zone.Rooms[16], Direction.South, Zone.Rooms[40]);
            ZoneHelper.ConnectRoom(Zone.Rooms[40], Direction.South, Zone.Rooms[41]);
            ZoneHelper.ConnectRoom(Zone.Rooms[41], Direction.West, Zone.Rooms[42]);
            ZoneHelper.ConnectRoom(Zone.Rooms[42], Direction.West, Zone.Rooms[43]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.North, Zone.Rooms[44]);
            ZoneHelper.ConnectRoom(Zone.Rooms[44], Direction.North, Zone.Rooms[45]);
            ZoneHelper.ConnectRoom(Zone.Rooms[45], Direction.North, Zone.Rooms[46]);
            ZoneHelper.ConnectRoom(Zone.Rooms[43], Direction.South, Zone.Rooms[47]);
            ZoneHelper.ConnectRoom(Zone.Rooms[47], Direction.South, Zone.Rooms[48]);
            ZoneHelper.ConnectRoom(Zone.Rooms[48], Direction.South, Zone.Rooms[49]);
            #endregion Level 4

            #region Level 5
            ZoneHelper.ConnectRoom(Zone.Rooms[18], Direction.East, Zone.Rooms[50]);
            ZoneHelper.ConnectRoom(Zone.Rooms[50], Direction.East, Zone.Rooms[51]);
            ZoneHelper.ConnectRoom(Zone.Rooms[51], Direction.South, Zone.Rooms[52]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.South, Zone.Rooms[53]);
            ZoneHelper.ConnectRoom(Zone.Rooms[53], Direction.East, Zone.Rooms[54]);
            ZoneHelper.ConnectRoom(Zone.Rooms[52], Direction.West, Zone.Rooms[55]);
            #endregion Level 5

            #region Level 6
            //Abandoned Dwarven City
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.North, Zone.Rooms[21]);
            ZoneHelper.ConnectRoom(Zone.Rooms[21], Direction.North, Zone.Rooms[22]);
            ZoneHelper.ConnectRoom(Zone.Rooms[20], Direction.West, Zone.Rooms[23]);
            ZoneHelper.ConnectRoom(Zone.Rooms[23], Direction.North, Zone.Rooms[24]);

            #endregion Level 6
            #endregion Mine
        }


        #region Rooms

        private static ITrap BuildTrap(DamageType damageType, List<string> disarmWords, string effectMessage)
        {
            ITrap trap = new Trap();
            trap.DisarmWord = disarmWords;
            //take the dice that we would use for damage and use those values for the disarm value
            //we pass 50% so that the pc has a 50/50 odds of disarming
            IDice tempDice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 50);
            trap.DisarmSuccessRoll = tempDice.Die * tempDice.Sides;

            IEnchantment enchantment = new LeaveRoomEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new Objects.Effect.Damage();

            IEffectParameter effectParameter = new EffectParameter();
            IDamage damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 100);
            damage.Type = damageType;
            effectParameter.Damage = damage;
            effectParameter.TargetMessage = new TranslationMessage(effectMessage);

            enchantment.Parameter = effectParameter;
            trap.Enchantments.Add(enchantment);
            return trap;
        }

        #region Leading To Mine Shaft
        private IRoom GenerateRoom1()
        {
            string examineDescription = "The cave walls are slimy and damp with water.";
            string lookDescription = "You can hear water dripping far off in the cave.";
            string shortDescription = "The entrance of the Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The room is about eight feet tall and fifteen feet wide.  Two smaller tunnels lead off to the east and west.";
            string lookDescription = "The cave is opens up a little here to a small room.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(WanderingGuard(room));

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The craftsmanship is pretty good.  Each brick is a little off but they were able to fit them all together to make a sturdy wall.";
            string lookDescription = "This tunnel is not part of the original cave system.  It have been carved out and built back up with bricks.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(WanderingGuard(room));

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The walls are lined with busts of an unknown dragon god.  Each bust has three holes, two for the eyes and one for the mouth.  Some of the holes seem to be scratched as if something comes out or something goes in them.";
            string lookDescription = "The tunnel continues on with a slight bend to the east and west.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Pierce, new List<string>() { "Bust", "Dragon" }, "As you attempt to leave you hear a click and then the air is filled with the whooshing of arrows flying toward you.");
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "The walls of the tunnel depict kobolds going into a tunnel and mining.";
            string lookDescription = "Dim light can be seen flickering to the east.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(WanderingGuard(room));

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "While the walls of the chamber are well lit the center is hard to see.  There is a large statue of a kobold god named Krutulmak.";
            string lookDescription = "Torches line the walls of the cavern here.  It appears to be some kind of chamber to hold a statue of the god of mining.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomLight(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);
            room.AddItemToRoom(Statue());
            return room;
        }

        private IItem Statue()
        {
            IItem item = CreateItem<IItem>();
            item.KeyWords.Add("statue");
            item.KeyWords.Add("Krutulmak");
            item.Attributes.Add(Item.ItemAttribute.NoGet);
            item.ShortDescription = "Statue of Krutulmak";
            item.SentenceDescription = "statue";
            item.LookDescription = "The statue stands twenty or thirty feet tall almost touching the top of the chamber.  He holds a pick in one hand and a shovel in the other.";
            item.ExamineDescription = "The statue is carved from a the natural stone in the cave.  The statue appears some how follow you with its eyes as you walk around the chamber.";

            return item;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "The walls of the tunnel are rough with the scars of pick axes.  Unsure what the kobolds were mining here it will impossible to tell as the walls have been picked clean of any thing that might give a clue.";
            string lookDescription = "This tunnel has been carved out of the ground and leads in a slightly down fashion.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomLight(examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(Statue());
            return room;
        }

        private IRoom GenerateRoom8()
        {
            string examineDescription = "The tunnel is very roughly carved with signs of more than one cave in.";
            string lookDescription = "The tunnel continues its path weaving slightly back and forth following what ever the kobolds were mining.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom9()
        {
            string examineDescription = "It appears at some point the miners hit water and flooded this tunnel.  Other than that it would be interesting to know what is creating that light.  Perhaps a miners globe that got left behind in when the tunnel flooded.";
            string lookDescription = "The tunnel continues into a downward into water.  Light can be seen coming from deep below the waters surface.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom10()
        {
            string examineDescription = "The equipment looks a little dusty from all the mining dust but is otherwise in good shape.";
            string lookDescription = "The room is filled with mining equipment.  Hard hats and mining picks are lined on the wall.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddItemToRoom(MiningHelmet());
            room.AddItemToRoom(MiningHelmet());
            room.AddItemToRoom(MiningHelmet());
            room.AddItemToRoom(MiningHelmet());
            room.AddItemToRoom(MiningPick());
            room.AddItemToRoom(MiningPick());
            room.AddItemToRoom(MiningPick());
            room.AddItemToRoom(MiningPick());

            return room;
        }

        private IArmor MiningHelmet()
        {
            string examineDescription = "The helmet appears to be made of steal and has two small cut outs for ears of a kobold.";
            string lookDescription = "The helmet looks to be in good shape.  It has a place for a small candle for light in the front.  Right behind where the candle would go is a name.  {Name}";
            string sentenceDescription = "mining helmet";
            string shortDescription = "A miners helmet.";

            IArmor armor = CreateArmor(AvalableItemPosition.Head, 11, examineDescription, lookDescription, sentenceDescription, shortDescription, new Steel());
            armor.KeyWords.Add("Mining");
            armor.KeyWords.Add("Helmet");
            armor.FlavorOptions.Add("{Name}", new List<string>() { "Jaap", "Qrink", "Grun", "Gregho", "Eezrark", "Jelo", "Kedzod", "Vokzoxild", "Nuneato", "Rardikzu" });
            return armor;
        }

        private IWeapon MiningPick()
        {
            string examineDescription = "The shaft of the pick has the name {Name} carved into the handle but is almost worn smooth.";
            string lookDescription = "A well balanced pick this tool has seen a lot of use over the years.  The handle is worn smooth and the pick is slightly bent.";
            string sentenceDescription = "miner pick";
            string shortDescription = "A miners pick.";

            IWeapon weapon = CreateWeapon(WeaponType.Pick, 11, examineDescription, lookDescription, sentenceDescription, shortDescription);
            weapon.KeyWords.Add("Mining");
            weapon.FlavorOptions.Add("{Name}", new List<string>() { "Jaap", "Qrink", "Grun", "Gregho", "Eezrark", "Jelo", "Kedzod", "Vokzoxild", "Nuneato", "Rardikzu" });

            return weapon;
        }
        #endregion Leading To Mine Shaft

        #region Mine Shaft
        #region Shaft
        private IRoom GenerateRoom11()
        {
            string examineDescription = "There is a wooden ladder attached to the wall of the shaft as well as a bucket and pulley for raising and lowering items from the pit.";
            string lookDescription = "The room is barren except for a single shaft leading down into the abyss.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom12()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom13()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }
        private IRoom GenerateRoom14()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom15()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom16()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom17()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom18()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom19()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom20()
        {
            string examineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            string lookDescription = "The ladder continues up and down the mine shaft.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private INonPlayerCharacter Miner(IRoom room)
        {
            string examineDescription = "The miner ignores you at first then notices you looking at them.  Started at first it decides the best thing to do is ignore you and hope you go away.";
            string lookDescription = "The miner is hard at work building new tunnels and mining materials.";
            string sentenceDescription = "Kobold miner";
            string shortDescription = "A Kobold miner.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 11);
            npc.Personalities.Add(new Wanderer());
            npc.KeyWords.Add("kobold");
            npc.KeyWords.Add("miner");
            npc.AddEquipment(MiningPick());
            npc.AddEquipment(MiningHelmet());

            return npc;
        }

        #endregion Shaft

        #region Level 2
        private IRoom GenerateRoom25()
        {
            string examineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            string lookDescription = "A mining cart track extends off into the darkness.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom26()
        {
            string examineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            string lookDescription = "A mining cart track extends off into the darkness.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom27()
        {
            string examineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            string lookDescription = "A mining cart track extends off into the darkness.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom28()
        {
            string examineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            string lookDescription = "A mining cart track extends off into the darkness.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom29()
        {
            string examineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            string lookDescription = "A mining cart track extends off into the darkness.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Level 2

        #region Level 3
        private IRoom GenerateRoom30()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom31()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }
        private IRoom GenerateRoom32()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom33()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom34()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom35()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom36()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom37()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom38()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));
            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }

        private IRoom GenerateRoom39()
        {
            string examineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            string lookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Level 3

        #region Level 4
        private IRoom GenerateRoom40()
        {
            string examineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom41()
        {
            string examineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom42()
        {
            string examineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom43()
        {
            string examineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }

        private IRoom GenerateRoom44()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));
            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }

        private IRoom GenerateRoom45()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom46()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom47()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom48()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }

        private IRoom GenerateRoom49()
        {
            string examineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            string lookDescription = "Smooth walls form a natural tunnel.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Level 4

        #region Level 5
        private IRoom GenerateRoom50()
        {
            string examineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            string lookDescription = "A small tunnel extends off into the distance making several turns.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom51()
        {
            string examineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            string lookDescription = "A small tunnel extends off into the distance making several turns.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }

        private IRoom GenerateRoom52()
        {
            string examineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            string lookDescription = "Inside the tunnel the sharp obsidian presses against you.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom53()
        {
            string examineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            string lookDescription = "Inside the tunnel the sharp obsidian presses against you.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom54()
        {
            string examineDescription = "The tunnel opens up to a room that seems to shimmer with reflective gems.";
            string lookDescription = "A small shimmering room.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom55()
        {
            string examineDescription = "The tunnel opens up to a room that seems to shimmer with reflective gems.";
            string lookDescription = "A small shimmering room.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Level 5

        #region Level 6
        private IRoom GenerateRoom21()
        {
            string examineDescription = "The boulders are big enough to let light through.  Through the gaps you can see the tunnel continues on into the darkness.  Occasionally you can see the faint shimmer of light coming from down the tunnel but there is no way to get through and find out what it is.";
            string lookDescription = "A cave in has blocked the path to the North.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom22()
        {
            string examineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            string lookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom23()
        {
            string examineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            string lookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        private IRoom GenerateRoom24()
        {
            string examineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            string lookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Miner(room));

            return room;
        }
        #endregion Level 6
        #endregion Mine Shaft

        #region Other Part of Cave
        private IRoom GenerateRoom56()
        {
            string examineDescription = "A natural tunnel that lead deep into the depths of the earth.  The ground is covered in foot prints except for several spots on the ground.";
            string lookDescription = "This part of the tunnel has received more foot traffic leading the floor to be worn smooth.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Pierce, new List<string>() { "ground", "foot", "print" }, "As you step forward your foot falls through the ground onto a spike in the floor.");
            room.Traps.Add(trap);

            room.AddMobileObjectToRoom(WanderingGuard(room));

            return room;
        }

        private INonPlayerCharacter WanderingGuard(IRoom room)
        {
            INonPlayerCharacter npc = Guard(room);
            Wanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 2));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 3));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 4));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 5));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 56));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 57));
            wanderer.NavigableRooms.Add(new BaseObjectId(Zone.Id, 58));
            npc.Personalities.Add(wanderer);

            return npc;
        }

        private INonPlayerCharacter Guard(IRoom room)
        {
            string examineDescription = "The guard looks tougher than a normal kobold, like its been trained extra hard and will fight to defend the tribe.";
            string lookDescription = "The kobold is slightly taller than most at a little over four feet tall.";
            string sentenceDescription = "guard";
            string shortDescription = "A kobold guard.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 13);

            npc.KeyWords.Add("guard");
            npc.KeyWords.Add("kobold");
            npc.Personalities.Add(new Aggressive());
            npc.AddEquipment(Spear());
            npc.AddEquipment(Bracer());
            npc.AddEquipment(BreastPlate());
            npc.AddEquipment(Gloves());
            npc.AddEquipment(Mask());
            npc.AddEquipment(Greaves());

            return npc;
        }

        private IWeapon Spear()
        {
            string examineDescription = "The spear is about three feet long and made of wood.  It had a point of cobalt on it that is very sharp to the touch.";
            string lookDescription = "A spear about three feet long with a sharp point of cobalt on the end.";
            string sentenceDescription = "small spear";
            string shortDescription = "A small shoddily made spear.";

            IWeapon weapon = CreateWeapon(WeaponType.Spear, 13, examineDescription, lookDescription, sentenceDescription, shortDescription);
            return weapon;
        }

        private IArmor Bracer()
        {
            string examineDescription = "The bracer is made of several smaller bones sewn on a strip of leather wrapped around the wearers arm.";
            string lookDescription = "A leather bracer made with bone for extra protection.";
            string sentenceDescription = "bone bracer";
            string shortDescription = "A bone bracer.";

            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 13, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("bone");
            armor.KeyWords.Add("bracer");

            return armor;
        }

        private IArmor BreastPlate()
        {
            string examineDescription = "The chest plate looks to like someone took a bears rib cage and set it before you and told you to wear it for armor.";
            string lookDescription = "You look at what looks to be a bears rib cage, a set of breastplate armor made of bone.";
            string sentenceDescription = "bone breastplate";
            string shortDescription = "A breastplate made of bone.";

            IArmor armor = CreateArmor(AvalableItemPosition.Body, 13, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("bone");
            armor.KeyWords.Add("breastplate");
            armor.KeyWords.Add("breast");
            armor.KeyWords.Add("plate");

            return armor;
        }

        private IArmor Gloves()
        {
            string examineDescription = "The gloves are made of leather and fit pretty nicely.  Each finger has a cutout for you fingers to slide through as well as what looks to be some type of claw that extends over each finger to help scratch the target.";
            string lookDescription = "The gloves appear to made of leather of varying grades of quality.  Still they serve their purpose of providing extra protection even if they don't look good.";
            string sentenceDescription = "leather gloves";
            string shortDescription = "A pair of leather gloves.";

            IArmor armor = CreateArmor(AvalableItemPosition.Hand, 13, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("gloves");

            return armor;
        }

        private IArmor Mask()
        {
            string examineDescription = "The mask does limit your visibility slightly but does protect your face from attacks.";
            string lookDescription = "The mask covers the wearer's face and provides some protection from attacks.";
            string sentenceDescription = "small mask";
            string shortDescription = "A mask made of bone.";

            IArmor armor = CreateArmor(AvalableItemPosition.Head, 13, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("mask");
            armor.KeyWords.Add("bone");

            return armor;
        }

        private IArmor Greaves()
        {
            string examineDescription = "The right greave has several large gashes while the left one looks brand new.  Maybe these are a mismatched set.";
            string lookDescription = "The greaves are made of leather with a bone outer covering.";
            string sentenceDescription = "greaves";
            string shortDescription = "A pair of bone greaves.";

            IArmor armor = CreateArmor(AvalableItemPosition.Legs, 13, examineDescription, lookDescription, sentenceDescription, shortDescription, new Leather());
            armor.KeyWords.Add("greaves");
            armor.KeyWords.Add("bone");

            return armor;
        }


        private IRoom GenerateRoom57()
        {
            string examineDescription = "The tunnel is a bit difficult to navigate as the floor is full of fallen debris.";
            string lookDescription = "Several sharp stalactite hang from the ceiling above.  Lets hope they don't fall down on you.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>() { "stalactite", }, "Suddenly a half dozen stalactites fall down shattering on the ground and sending sharp rock fragments flying through the air.");
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom58()
        {
            string examineDescription = "The tunnel opens up a bit to form a natural room.  To the east is a wooden door with a picture of a shield on it.";
            string lookDescription = "The tunnel opens up here to form a bit of a room.  You can hear your foot steps echo gently off the walls.  To the east is door has walled off part of the cave system.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom59()
        {
            string examineDescription = "This room is used by the kobold guards.  It has a few beds for sleeping as well as some table for eating and playing games.";
            string lookDescription = "A room used for the guards who keep the lair safe from intruders.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            ITrap trap = new Trap();
            trap.DisarmWord = new List<string>() { "door" };
            //take the dice that we would use for damage and use those values for the disarm value
            //we pass 50% so that the pc has a 50/50 odds of disarming
            IDice tempDice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 50);
            trap.DisarmSuccessRoll = tempDice.Die * tempDice.Sides;

            IEnchantment enchantment = new LeaveRoomEnchantment();
            enchantment.ActivationPercent = 100;
            enchantment.Effect = new Objects.Effect.Damage();

            IEffectParameter effectParameter = new EffectParameter();
            IDamage damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 100);
            damage.Type = DamageType.Bludgeon;
            effectParameter.Damage = damage;
            effectParameter.TargetMessage = new TranslationMessage("As you walk through the door it swings closed and bludgeons you in the back of the head repeatedly knocking you forward.");

            enchantment.Parameter = effectParameter;
            trap.Enchantments.Add(enchantment);
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom60()
        {
            string examineDescription = "The holes appear to be filled with busts of kobolds.  The ones that do have placards with names under them.";
            string lookDescription = "The walls of the tunnel have several small holes.  Some of them have small trinkets in them.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom61()
        {
            string examineDescription = "The holes appear to be filled with busts of kobolds.  The ones that do have placards with names under them.";
            string lookDescription = "The walls of the tunnel have several small holes.  Some of them have small trinkets in them.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom62()
        {
            string examineDescription = "A large room with a great deal of seating and tables are setup in circular fashion.";
            string lookDescription = "This appears to be some kinda common room.  A place a large fire is built in the middle but it is not lit.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom63()
        {
            string examineDescription = "The tapestry depicts the entrapping of Krutulmak.";
            string lookDescription = "A large tapestries line the wall.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom64()
        {
            string examineDescription = "The tapestry depicts miners searching for Krutulmak.";
            string lookDescription = "A large tapestries line the wall.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom65()
        {
            string examineDescription = "The tapestry depicts kobolds finding and freeing Krutulmak.";
            string lookDescription = "A large tapestries line the wall.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom66()
        {
            string examineDescription = "The bunks are made of wood and are about four feet long.";
            string lookDescription = "This is the kobolds sleeping quarters.  Bunk beds line the cave walls maximizing space.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom67()
        {
            string examineDescription = "The ceiling is jagged where part of it has caved in but seems to be reinforced to prevent future cave ins.";
            string lookDescription = "The tunnel shows signs of a recent cave in but has been cleared.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom68()
        {
            string examineDescription = "The tunnels all look well worn as if something important lies at the ends of them.";
            string lookDescription = "A smaller tunnel leads to the west while the main part continues to the south and east.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom69()
        {
            string examineDescription = "The tunnel walls are rough from carving but the floor has been worn smooth from foot traffic.";
            string lookDescription = "A long tunnel that seems to go on forever stretches out before you.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom70()
        {
            string examineDescription = "The tunnel has been carefully collapsed and widened to create a zig zag effect that is easier to defend.";
            string lookDescription = "The tunnel has been made to be hard to run through with lots of zig zags.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(Guard(room));
            room.AddMobileObjectToRoom(Guard(room));
            return room;
        }

        private IRoom GenerateRoom71()
        {
            string examineDescription = "There are thirty to fifty eggs here waiting to hatch.";
            string lookDescription = "The room is full of kobold eggs.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(EggCareTaker(room));
            return room;
        }

        private INonPlayerCharacter EggCareTaker(IRoom room)
        {
            string examineDescription = "The care giver carefully goes around examining each egg and makes sure it is growing properly.";
            string lookDescription = "Dressed in a light blue apron the kobold gives you a peaceful sensation.";
            string sentenceDescription = "kobold";
            string shortDescription = "A kobold care giver.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 11); 
            npc.KeyWords.Add("giver");
            npc.KeyWords.Add("kobold");

            return npc;
        }

        private IRoom GenerateRoom72()
        {
            string examineDescription = "Several books used for teaching are placed on a shelf in the back of the room.  In addition a large chalk board on wheels is nearby with a ring of sitting mats forming a small semi circle.";
            string lookDescription = "This part of the room is where the young kobolds are raised until they can join the main part of the tribe.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            //small kobold
            return room;
        }

        private INonPlayerCharacter ChildrenKobold(IRoom room)
        {
            string examineDescription = "It appears that when the teacher is away that the children really do play.";
            string lookDescription = "The small kobold ignores you and continues to play.";
            string sentenceDescription = "child kobold";
            string shortDescription = "A small kobold runs around the playing.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 8);
            npc.KeyWords.Add("kobold");
            return npc;
        }

        private IRoom GenerateRoom73()
        {
            string examineDescription = "The tunnel walls are fairly smooth and damp with water dripping down.";
            string lookDescription = "The sound of water can be heard from the tunnel to the south.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }

        private IRoom GenerateRoom74()
        {
            string examineDescription = "The well is a wooden platform for holding the bucket to retrieve water.  It stands about two feet tall and has a small wooden base to keep things from falling down the hole.";
            string lookDescription = "The room is dominated by a small well in the center of the room that is used to retrieve water from deep within the cave.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }



        private IRoom GenerateRoom75()
        {
            string examineDescription = "There food is stacked neatly on the shelves.  Going through the food though reveals that kobolds will eat most anything, bark, dirt, leather, eggshells.";
            string lookDescription = "A large amount of food is stored on shelves in this room.";
            string shortDescription = "Kobold Lair";
            IRoom room = IndoorRoomNoLight(examineDescription, lookDescription, shortDescription);

            return room;
        }
        #endregion Other Part of Cave
        #endregion Rooms
    }
}
