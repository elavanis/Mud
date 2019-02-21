using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Zone.Interface;
using Objects.Zone;
using System.Reflection;
using Objects.Room.Interface;
using Objects.Room;
using Objects.Trap.Interface;
using Objects.Trap;
using Objects.Global.Stats;
using Objects.Magic.Interface;
using Objects.Magic.Enchantment;
using Objects.Die;
using static Objects.Global.Direction.Directions;
using Objects.Item;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Item.Items;
using Objects.Material.Materials;
using Objects.Global;
using Objects.Damage.Interface;
using Objects.Damage;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Mob.Interface;
using Objects.Mob;
using Objects.Personality.Personalities;
using static Objects.Damage.Damage;
using Objects.Die.Interface;
using Objects;
using Objects.Language;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Item.Items.Equipment;
using static Objects.Item.Items.Weapon;

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
            //AddSounds();

            return Zone;
        }

        private void ConnectRooms()
        {
            Zone.RecursivelySetZone();

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
        private IRoom ZoneRoom(int movementCost)
        {
            IRoom room = CreateRoom(movementCost);
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.NoLight);
            return room;
        }

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
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The cave walls are slimy and damp with water.";
            room.LookDescription = "You can hear water dripping far off in the cave.";
            room.ShortDescription = "The entrance of the Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The room is about eight feet tall and fifteen feet wide.  Two smaller tunnels lead off to the east and west.";
            room.LookDescription = "The cave is opens up a little here to a small room.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(WanderingGuard());

            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The craftsmanship is pretty good.  Each brick is a little off but they were able to fit them all together to make a sturdy wall.";
            room.LookDescription = "This tunnel is not part of the original cave system.  It have been carved out and built back up with bricks.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(WanderingGuard());

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls are lined with busts of an unknown dragon god.  Each bust has three holes, two for the eyes and one for the mouth.  Some of the holes seem to be scratched as if something comes out or something goes in them.";
            room.LookDescription = "The tunnel continues on with a slight bend to the east and west.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Pierce, new List<string>() { "Bust", "Dragon" }, "As you attempt to leave you hear a click and then the air is filled with the whooshing of arrows flying toward you.");
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the tunnel depict kobolds going into a tunnel and mining.";
            room.LookDescription = "Dim light can be seen flickering to the east.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(WanderingGuard());

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Remove(Room.RoomAttribute.NoLight);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);
            room.ExamineDescription = "While the walls of the chamber are well lit the center is hard to see.  There is a large statue of a kobold god named Krutulmak.";
            room.LookDescription = "Torches line the walls of the cavern here.  It appears to be some kind of chamber to hold a statue of the god of mining.";
            room.ShortDescription = "Kobold Lair";

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
            IRoom room = ZoneRoom(1);
            room.Attributes.Remove(Room.RoomAttribute.NoLight);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The walls of the tunnel are rough with the scars of pick axes.  Unsure what the kobolds were mining here it will impossible to tell as the walls have been picked clean of any thing that might give a clue.";
            room.LookDescription = "This tunnel has been carved out of the ground and leads in a slightly down fashion.";
            room.ShortDescription = "Kobold Lair";

            room.AddItemToRoom(Statue());
            return room;
        }

        private IRoom GenerateRoom8()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel is very roughly carved with signs of more than one cave in.";
            room.LookDescription = "The tunnel continues its path weaving slightly back and forth following what ever the kobolds were mining.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "It appears at some point the miners hit water and flooded this tunnel.  Other than that it would be interesting to know what is creating that light.  Perhaps a miners globe that got left behind in when the tunnel flooded.";
            room.LookDescription = "The tunnel continues into a downward into water.  Light can be seen coming from deep below the waters surface.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom10()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The equipment looks a little dusty from all the mining dust but is otherwise in good shape.";
            room.LookDescription = "The room is filled with mining equipment.  Hard hats and mining picks are lined on the wall.";
            room.ShortDescription = "Kobold Lair";

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
            IArmor armor = CreateArmor(AvalableItemPosition.Head, 11, new Steel());
            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(armor.Level);

            armor.KeyWords.Add("Mining");
            armor.KeyWords.Add("Helmet");
            armor.ExamineDescription = "The helmet appears to be made of steal and has two small cut outs for ears of a kobold.";
            armor.LookDescription = "The helmet looks to be in good shape.  It has a place for a small candle for light in the front.  Right behind where the candle would go is a name.  {Name}";
            armor.SentenceDescription = "mining helmet";
            armor.ShortDescription = "A miners helmet.";

            armor.FlavorOptions.Add("{Name}", new List<string>() { "Jaap", "Qrink", "Grun", "Gregho", "Eezrark", "Jelo", "Kedzod", "Vokzoxild", "Nuneato", "Rardikzu" });
            return armor;
        }

        private IWeapon MiningPick()
        {
            IWeapon weapon = CreateWeapon(WeaponType.Pick, 11);
            IDamage damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = Objects.Damage.Damage.DamageType.Slash;
            weapon.DamageList.Add(damage);

            weapon.KeyWords.Add("Mining");
            weapon.KeyWords.Add("Pick");
            weapon.ExamineDescription = "The shaft of the pick has the name {Name} carved into the handle but is almost worn smooth.";
            weapon.LookDescription = "A well balanced pick this tool has seen a lot of use over the years.  The handle is worn smooth and the pick is slightly bent.";
            weapon.SentenceDescription = "miner pick";
            weapon.ShortDescription = "A miners pick.";

            weapon.FlavorOptions.Add("{Name}", new List<string>() { "Jaap", "Qrink", "Grun", "Gregho", "Eezrark", "Jelo", "Kedzod", "Vokzoxild", "Nuneato", "Rardikzu" });

            return weapon;
        }
        #endregion Leading To Mine Shaft

        #region Mine Shaft
        #region Shaft
        private IRoom GenerateRoom11()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "There is a wooden ladder attached to the wall of the shaft as well as a bucket and pulley for raising and lowering items from the pit.";
            room.LookDescription = "The room is barren except for a single shaft leading down into the abyss.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom12()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom13()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());

            return room;
        }
        private IRoom GenerateRoom14()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom15()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom16()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom17()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom18()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom19()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom20()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ladder for all its use and age still is quite sturdy.  It appears a few rungs here and there have been replaced to ensure its safety.";
            room.LookDescription = "The ladder continues up and down the mine shaft.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private INonPlayerCharacter Miner()
        {
            INonPlayerCharacter npc = Npc(11);
            npc.Personalities.Add(new Wanderer());

            npc.KeyWords.Add("miner");
            npc.SentenceDescription = "Kobold miner";
            npc.ShortDescription = "A Kobold miner.";
            npc.LookDescription = "The miner is hard at work building new tunnels and mining materials.";
            npc.ExamineDescription = "The miner ignores you at first then notices you looking at them.  Started at first it decides the best thing to do is ignore you and hope you go away.";

            npc.AddEquipment(MiningPick());
            npc.AddEquipment(MiningHelmet());

            return npc;
        }

        private INonPlayerCharacter Npc(int level)
        {
            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, level);
            npc.KeyWords.Add("kobold");

            return npc;
        }

        #endregion Shaft

        #region Level 2
        private IRoom GenerateRoom25()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            room.LookDescription = "A mining cart track extends off into the darkness.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom26()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            room.LookDescription = "A mining cart track extends off into the darkness.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom27()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            room.LookDescription = "A mining cart track extends off into the darkness.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom28()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            room.LookDescription = "A mining cart track extends off into the darkness.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom29()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water drips down from cracks in the mine ceiling forming small puddles on the mine floor.";
            room.LookDescription = "A mining cart track extends off into the darkness.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        #endregion Level 2

        #region Level 3
        private IRoom GenerateRoom30()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom31()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());

            return room;
        }
        private IRoom GenerateRoom32()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom33()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom34()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom35()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom36()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom37()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom38()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());
            room.AddMobileObjectToRoom(Miner());

            return room;
        }

        private IRoom GenerateRoom39()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls of the mine can be seen streaked with black coal here and there.";
            room.LookDescription = "The mining tunnel is roughly hewn from the soft rock.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        #endregion Level 3

        #region Level 4
        private IRoom GenerateRoom40()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom41()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom42()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom43()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls appear to be naturally carved from flowing water over time but are dry now.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());

            return room;
        }

        private IRoom GenerateRoom44()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());
            room.AddMobileObjectToRoom(Miner());

            return room;
        }

        private IRoom GenerateRoom45()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom46()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom47()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom48()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());

            return room;
        }

        private IRoom GenerateRoom49()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The water is clear and smooth as glass.  You can see the bottom of the lake extend out several feet before disapearing into the darkness.";
            room.LookDescription = "Smooth walls form a natural tunnel.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        #endregion Level 4

        #region Level 5
        private IRoom GenerateRoom50()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            room.LookDescription = "A small tunnel extends off into the distance making several turns.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom51()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            room.LookDescription = "A small tunnel extends off into the distance making several turns.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            room.AddMobileObjectToRoom(Miner());

            return room;
        }

        private IRoom GenerateRoom52()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            room.LookDescription = "Inside the tunnel the sharp obsidian presses against you.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom53()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel appears to be a mixture of sharp and smooth glass obsidian.";
            room.LookDescription = "Inside the tunnel the sharp obsidian presses against you.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>(), "As you crawl through the tunnel sharp glass cuts you.");
            trap.Enchantments[0].Parameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForTrapLevel(11, 10);
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom54()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel opens up to a room that seems to shimmer with reflective gems.";
            room.LookDescription = "A small shimmering room.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom55()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel opens up to a room that seems to shimmer with reflective gems.";
            room.LookDescription = "A small shimmering room.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        #endregion Level 5

        #region Level 6
        private IRoom GenerateRoom21()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The boulders are big enough to let light through.  Through the gaps you can see the tunnel continues on into the darkness.  Occasionally you can see the faint shimmer of light coming from down the tunnel but there is no way to get through and find out what it is.";
            room.LookDescription = "A cave in has blocked the path to the North.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom22()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            room.LookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom23()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            room.LookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        private IRoom GenerateRoom24()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls while not smooth are well formed.  An iron track for mining carts with wooden supports completes the mine's tunnel.";
            room.LookDescription = "The tunnel walls are reinforced with wooden beams and carved with a high level of precision.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Miner());

            return room;
        }
        #endregion Level 6
        #endregion Mine Shaft

        #region Other Part of Cave
        private IRoom GenerateRoom56()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "A natural tunnel that lead deep into the depths of the earth.  The ground is covered in foot prints except for several spots on the ground.";
            room.LookDescription = "This part of the tunnel has received more foot traffic leading the floor to be worn smooth.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Pierce, new List<string>() { "ground", "foot", "print" }, "As you step forward your foot falls through the ground onto a spike in the floor.");
            room.Traps.Add(trap);

            room.AddMobileObjectToRoom(WanderingGuard());

            return room;
        }

        private INonPlayerCharacter WanderingGuard()
        {
            INonPlayerCharacter npc = Guard();
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

        private INonPlayerCharacter Guard()
        {
            INonPlayerCharacter npc = Npc(13);

            npc.ExamineDescription = "The guard looks tougher than a normal kobold, like its been trained extra hard and will fight to defend the tribe.";
            npc.LookDescription = "The kobold is slightly taller than most at a little over four feet tall.";
            npc.ShortDescription = "A kobold guard.";
            npc.SentenceDescription = "guard";
            npc.KeyWords.Add("guard");

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
            IWeapon weapon = CreateWeapon(WeaponType.Spear, 13);

            weapon.ExamineDescription = "The spear is about three feet long and made of wood.  It had a point of cobalt on it that is very sharp to the touch.";
            weapon.LookDescription = "A spear about three feet long with a sharp point of cobalt on the end.";
            weapon.ShortDescription = "A small shoddily made spear.";
            weapon.SentenceDescription = "small spear";
            weapon.KeyWords.Add("spear");

            IDamage damage = new Objects.Damage.Damage();
            damage.Type = DamageType.Pierce;
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(13);
            weapon.DamageList.Add(damage);

            return weapon;
        }

        private IArmor Bracer()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Arms, 13, new Leather());

            armor.ExamineDescription = "The bracer is made of several smaller bones sewn on a strip of leather wrapped around the wearers arm.";
            armor.LookDescription = "A leather bracer made with bone for extra protection.";
            armor.ShortDescription = "A bone bracer.";
            armor.SentenceDescription = "bone bracer";
            armor.KeyWords.Add("bone");
            armor.KeyWords.Add("bracer");

            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(13);

            return armor;
        }

        private IArmor BreastPlate()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Body, 13, new Leather());

            armor.ExamineDescription = "The chest plate looks to like someone took a bears rib cage and set it before you and told you to wear it for armor.";
            armor.LookDescription = "You look at what looks to be a bears rib cage, a set of breast plate armor made of bone.";
            armor.ShortDescription = "A breast plate made of bone.";
            armor.SentenceDescription = "bone breast plate";
            armor.KeyWords.Add("bone");
            armor.KeyWords.Add("breast");
            armor.KeyWords.Add("plate");

            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(13);

            return armor;
        }

        private IArmor Gloves()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Hand, 13, new Leather());

            armor.ExamineDescription = "The gloves are made of leather and fit pretty nicely.  Each finger has a cutout for you fingers to slide through as well as what looks to be some type of claw that extends over each finger to help scratch the target.";
            armor.LookDescription = "The gloves appear to made of leather of varying grades of quality.  Still they serve their purpose of providing extra protection even if they don't look good.";
            armor.ShortDescription = "A pair of leather gloves.";
            armor.SentenceDescription = "leather gloves";
            armor.KeyWords.Add("leather");
            armor.KeyWords.Add("gloves");

            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(13);

            return armor;
        }

        private IArmor Mask()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Head, 13, new Leather());

            armor.ExamineDescription = "The mask does limit your visibility slightly but does protect your face from attacks.";
            armor.LookDescription = "The mask covers the wearer's face and provides some protection from attacks.";
            armor.ShortDescription = "A mask made of bone.";
            armor.SentenceDescription = "small mask";
            armor.KeyWords.Add("mask");
            armor.KeyWords.Add("bone");

            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(13);

            return armor;
        }

        private IArmor Greaves()
        {
            IArmor armor = CreateArmor(AvalableItemPosition.Legs, 13, new Leather());

            armor.ExamineDescription = "The right greave has several large gashes while the left one looks brand new.  Maybe these are a mismatched set.";
            armor.LookDescription = "The greaves are made of leather with a bone outer covering.";
            armor.ShortDescription = "A pair of bone greaves.";
            armor.SentenceDescription = "greaves";
            armor.KeyWords.Add("greaves");
            armor.KeyWords.Add("bone");

            armor.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(13);

            return armor;
        }


        private IRoom GenerateRoom57()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel is a bit difficult to navigate as the floor is full of fallen debris.";
            room.LookDescription = "Several sharp stalactite hang from the ceiling above.  Lets hope they don't fall down on you.";
            room.ShortDescription = "Kobold Lair";

            ITrap trap = BuildTrap(DamageType.Slash, new List<string>() { "stalactite", }, "Suddenly a half dozen stalactites fall down shattering on the ground and sending sharp rock fragments flying through the air.");
            room.Traps.Add(trap);

            return room;
        }

        private IRoom GenerateRoom58()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel opens up a bit to form a natural room.  To the east is a wooden door with a picture of a shield on it.";
            room.LookDescription = "The tunnel opens up here to form a bit of a room.  You can hear your foot steps echo gently off the walls.  To the east is door has walled off part of the cave system.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom59()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "This room is used by the kobold guards.  It has a few beds for sleeping as well as some table for eating and playing games.";
            room.LookDescription = "A room used for the guards who keep the lair safe from intruders.";
            room.ShortDescription = "Kobold Lair";

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
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The holes appear to be filled with busts of kobolds.  The ones that do have placards with names under them.";
            room.LookDescription = "The walls of the tunnel have several small holes.  Some of them have small trinkets in them.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom61()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The holes appear to be filled with busts of kobolds.  The ones that do have placards with names under them.";
            room.LookDescription = "The walls of the tunnel have several small holes.  Some of them have small trinkets in them.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom62()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "A large room with a great deal of seating and tables are setup in circular fashion.";
            room.LookDescription = "This appears to be some kinda common room.  A place a large fire is built in the middle but it is not lit.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom63()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tapestry depicts the entrapping of Krutulmak.";
            room.LookDescription = "A large tapestries line the wall.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom64()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tapestry depicts miners searching for Krutulmak.";
            room.LookDescription = "A large tapestries line the wall.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom65()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tapestry depicts kobolds finding and freeing Krutulmak.";
            room.LookDescription = "A large tapestries line the wall.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom66()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The bunks are made of wood and are about four feet long.";
            room.LookDescription = "This is the kobolds sleeping quarters.  Bunk beds line the cave walls maximizing space.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom67()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ceiling is jagged where part of it has caved in but seems to be reinforced to prevent future cave ins.";
            room.LookDescription = "The tunnel shows signs of a recent cave in but has been cleared.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom68()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnels all look well worn as if something important lies at the ends of them.";
            room.LookDescription = "A smaller tunnel leads to the west while the main part continues to the south and east.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom69()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel walls are rough from carving but the floor has been worn smooth from foot traffic.";
            room.LookDescription = "A long tunnel that seems to go on forever stretches out before you.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom70()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel has been carefully collapsed and widened to create a zig zag effect that is easier to defend.";
            room.LookDescription = "The tunnel has been made to be hard to run through with lots of zig zags.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(Guard());
            room.AddMobileObjectToRoom(Guard());
            return room;
        }

        private IRoom GenerateRoom71()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "There are thirty to fifty eggs here waiting to hatch.";
            room.LookDescription = "The room is full of kobold eggs.";
            room.ShortDescription = "Kobold Lair";

            room.AddMobileObjectToRoom(EggCareTaker());
            return room;
        }

        private INonPlayerCharacter EggCareTaker()
        {
            INonPlayerCharacter npc = Npc(11);

            npc.ExamineDescription = "The care giver carefully goes around examining each egg and makes sure it is growing properly.";
            npc.LookDescription = "Dressed in a light blue apron the kobold gives you a peaceful sensation.";
            npc.ShortDescription = "A kobold care giver.";
            npc.SentenceDescription = "kobold";
            npc.KeyWords.Add("giver");

            return npc;
        }

        private IRoom GenerateRoom72()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Several books used for teaching are placed on a shelf in the back of the room.  In addition a large chalk board on wheels is nearby with a ring of sitting mats forming a small semi circle.";
            room.LookDescription = "This part of the room is where the young kobolds are raised until they can join the main part of the tribe.";
            room.ShortDescription = "Kobold Lair";

            //small kobold
            return room;
        }

        private INonPlayerCharacter ChildrenKobold()
        {
            INonPlayerCharacter npc = Npc(8);

            npc.ExamineDescription = "It appears that when the teacher is away that the children really do play.";
            npc.LookDescription = "The small kobold ignores you and continues to play.";
            npc.ShortDescription = "A small kobold runs around the playing.";
            npc.SentenceDescription = "child kobold";

            return npc;
        }

        private IRoom GenerateRoom73()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel walls are fairly smooth and damp with water dripping down.";
            room.LookDescription = "The sound of water can be heard from the tunnel to the south.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }

        private IRoom GenerateRoom74()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The well is a wooden platform for holding the bucket to retrieve water.  It stands about two feet tall and has a small wooden base to keep things from falling down the hole.";
            room.LookDescription = "The room is dominated by a small well in the center of the room that is used to retrieve water from deep within the cave.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }



        private IRoom GenerateRoom75()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "There food is stacked neatly on the shelves.  Going through the food though reveals that kobolds will eat most anything, bark, dirt, leather, eggshells.";
            room.LookDescription = "A large amount of food is stored on shelves in this room.";
            room.ShortDescription = "Kobold Lair";

            return room;
        }
        #endregion Other Part of Cave
        #endregion Rooms
    }
}