using MiscShared;
using Objects.Damage.Interface;
using Objects.Die;
using Objects.Effect;
using Objects.Language;
using Objects.Magic.Enchantment;
using Objects.Magic.Interface;
using Objects.Mob.Interface;
using Objects.Personality;
using Objects.Personality.Interface;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using Shared.Sound;
using Shared.Sound.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Objects.Damage.Damage;
using static Objects.Global.Direction.Directions;
using static Objects.Mob.NonPlayerCharacter;
using static Objects.Room.Room;

namespace GenerateZones.Zones
{
    public class StartingBoat : BaseZone, IZoneCode
    {
        public StartingBoat() : base(1)
        {
        }

        public IZone Generate()
        {
            Zone.Name = nameof(StartingBoat);

            BuildRoomsViaReflection(this.GetType());

            ConnectRooms();

            AddAmbientSound();

            return Zone;
        }

        private void ConnectRooms()
        {
            //ZoneHelper.ConnectRoom(Zone.Rooms[1], "W", Zone.Rooms[2]);
            Zone.Rooms[1].West = new Exit() { Zone = 1, Room = 2 };
            ZoneHelper.ConnectRoom(Zone.Rooms[2], Direction.West, Zone.Rooms[3]);
            ZoneHelper.ConnectRoom(Zone.Rooms[3], Direction.Up, Zone.Rooms[4]);
            ZoneHelper.ConnectRoom(Zone.Rooms[4], Direction.West, Zone.Rooms[5]);
            ZoneHelper.ConnectRoom(Zone.Rooms[5], Direction.West, Zone.Rooms[6]);
            ZoneHelper.ConnectRoom(Zone.Rooms[6], Direction.Down, Zone.Rooms[7]);
        }

        private void AddAmbientSound()
        {
            ISound sound = new Sound();
            sound.Loop = true;
            sound.SoundName = string.Format("{0}\\{1}", Zone.Name, "ShipFire.mp3");

            foreach (Room room in Zone.Rooms.Values)
            {
                room.Sounds.Add(sound);
            }
        }

        #region Rooms
        private IRoom GenerateRoom1()
        {
            string examineDescription = "Is it really wise to be taking your time to examine the room while the ship is on FIRE!!!!  None the less you are in your room.  Flames are consuming the walls to the east and north.  The wall to the south still looks to be in good shape but it might not be a good idea to stay and find out how it takes for a wooden ship to burn.  Instead try leaving to the WEST.";
            string lookDescription = "You are awaken from you sleep by a hot ember that has landed on you forehead.  Quickly knocking it off you realize the ship is on fire.";
            string shortDescription = "FIRE!";
            IRoom room = BelowDeck(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(RoomAttribute.Indoor);

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new RoomId(7, 8); //hospital
            room.Enchantments.Add(enter);
            return room;
        }

        private IRoom GenerateRoom2()
        {
            string examineDescription = "The door to the east is burning brightly while the hallway to the west seems slightly less on fire.";
            string lookDescription = "Fire has spread to most of the ship and smoke makes it hard to see and breath.";
            string shortDescription = "FIRE!";
            IRoom room = BelowDeck(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(RoomAttribute.Indoor);

            return room;
        }

        private IRoom GenerateRoom3()
        {
            string examineDescription = "The smoky ash burns your eyes making it hard to see.";
            string lookDescription = "Rays of light from above streak down through the smoky air while the orange glow of flames light the way to the east.";
            string shortDescription = "Stairs to the deck above";
            IRoom room = BelowDeck(examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(RoomAttribute.Indoor);

            return room;
        }

        private IRoom GenerateRoom4()
        {
            string examineDescription = "The crew scramble around trying to put out fires, attack the dragons as they dive in for an attack and get to the one remaining life boat on the west side of the ship.";
            string lookDescription = "A pair of dragons circle above taking turns breathing fire onto the ship.";
            string shortDescription = "On the ships' deck";
            IRoom room = OnDeck(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(DeckCrew(room));
            room.AddMobileObjectToRoom(DeckCrew(room));

            return room;
        }

        private IRoom GenerateRoom5()
        {
            string examineDescription = "Standing in the middle of the ship you can see that the ship is lost.  The center mast is engulfed in flames, the sails long burned away, both the aft and stern of the ship is ablaze.  It is only a matter of time before she burns enough to allow water into her hull or fire reaches the gunpowder magazine and the ship is blown apart.";
            string lookDescription = "Flames are beginning to make the deck unstable.  Holes are opening up as barrels fall through the floor.";
            string shortDescription = "On the ships' deck";
            IRoom room = OnDeck(examineDescription, lookDescription, shortDescription);

            room.AddMobileObjectToRoom(DeckCrew(room));
            room.AddMobileObjectToRoom(DeckCrew(room));

            return room;
        }

        private IRoom GenerateRoom6()
        {
            string examineDescription = "Looking around one last time at {shipName} you bid it fairwell. She had been home to you for the last several weeks but she will be no more.";
            string lookDescription = "Several feet below you is the last life boat.  Passengers and members of the crew are offering you words of encouragement to get in the boat.";
            string shortDescription = "Above the life boat";
            IRoom room = OnDeck(examineDescription, lookDescription, shortDescription);

            room.FlavorOptions.Add("{shipName}", new List<string>() { "Boaty McBoat Face", "The Tinderbox", "The MatchBox", "The Ocean Blaze", "The Firetrap", "Ablaze", "Kindling" });

            room.AddMobileObjectToRoom(DeckCrew(room));
            room.AddMobileObjectToRoom(DeckCrew(room));

            return room;
        }

        private IRoom GenerateRoom7()
        {
            string examineDescription = "";
            string lookDescription = "You should not see this unless you are a god.  In which case congratulations on godhood.";
            string shortDescription = "In the water";
            IRoom room = OnDeck(examineDescription, lookDescription, shortDescription);

            EnterRoomEnchantment enter = new EnterRoomEnchantment();
            enter.ActivationPercent = 100;
            enter.Effect = new SetRespawnPoint();
            enter.Parameter.ObjectId = new RoomId(7, 3); //hospital
            room.Enchantments.Add(enter);

            IEnchantment death = new EnterRoomEnchantment();
            death.Effect = new MobDie();
            death.ActivationPercent = 100;
            death.EnchantmentEndingDateTime = new DateTime(9999, 12, 31);
            death.Parameter.TargetMessage = new TranslationMessage("Just as you were about to hop into the boat one of the dragons breathed a fireball in your area knocking you overboard and into the water.  The cool water is a sharp contrast to the intense heat of the fireball that burned your skin.  You fight to keep your head above the water but you are to disoriented from the blast.  You can hear the people shouting above and the flames from the ship and then silence as you slip beneath the water.  The light from the burning ship can be seen momentarily as you sink into the dark depths of the ocean.");
            room.Enchantments.Add(death);

            return room;
        }

        private IRoom BelowDeck(string examineDescription, string lookDescription, string shortDescription)
        {
            IRoom room = IndoorRoomLight(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Enchantments.Add(FireDamage());

            return room;
        }
        private IRoom OnDeck(string examineDescription, string lookDescription, string shortDescription)
        {
            IRoom room = OutdoorRoom(Zone.Id, examineDescription, lookDescription, shortDescription);
            room.Attributes.Add(RoomAttribute.Light);
            room.Enchantments.Add(DragonBreath());

            return room;
        }

        #endregion Rooms

        #region Npcs
        private INonPlayerCharacter DeckCrew(IRoom room)
        {
            string examineDescription = "He looks like your average sailer, dressed in slightly raged clothing and well tanned from his time at sea.";
            string lookDescription = "This crew member run back and forth trying put out fires and throw spears at the dragons as they attack.";
            string sentenceDescription = "sailor";
            string shortDescription = "A crew member.";

            INonPlayerCharacter npc = CreateNonplayerCharacter(MobType.Humanoid, room, examineDescription, lookDescription, sentenceDescription, shortDescription, 20);
            npc.KeyWords.Add("crew");
            npc.KeyWords.Add("deckhand");
            npc.KeyWords.Add("member");

            IWanderer wanderer = new Wanderer();
            wanderer.NavigableRooms.Add(new RoomId(1, 4));
            wanderer.NavigableRooms.Add(new RoomId(1, 5));
            wanderer.NavigableRooms.Add(new RoomId(1, 6));
            npc.Personalities.Add(wanderer);

            Speaker speaker = new Speaker();
            speaker.SpeakPercent = 1;
            speaker.ThingsToSay.Add("Quick matey, get a bucket to that fire.");
            speaker.ThingsToSay.Add("Here comes the red dragon!  Spears to the ready.");
            speaker.ThingsToSay.Add("Here comes the black dragon!  Hit it in they mouth when it attacks.");
            speaker.ThingsToSay.Add("Get the passengers to the life boats.");

            return npc;
        }

        #endregion Npcs

        private IEnchantment FireDamage()
        {
            IEnchantment fireDamage = new HeartbeatBigTickEnchantment();
            fireDamage.Effect = new Damage();
            fireDamage.ActivationPercent = 5;
            fireDamage.EnchantmentEndingDateTime = new DateTime(9999, 12, 31);
            IDamage damage = new Objects.Damage.Damage(new Dice(1, 6), DamageType.Fire);
            fireDamage.Parameter.Damage = damage;
            fireDamage.Parameter.TargetMessage = new TranslationMessage("Fire leaps up and burns you.");
            fireDamage.Parameter.Description = "fire";

            return fireDamage;
        }


        private IEnchantment DragonBreath()
        {
            ISound sound = new Sound();
            sound.Loop = false;
            sound.RandomSounds.Add($"{Zone.Name}\\DragonFireball_Center.mp3");
            sound.RandomSounds.Add($"{Zone.Name}\\DragonFireball_L-R.mp3");
            sound.RandomSounds.Add($"{Zone.Name}\\DragonFireball_R-L.mp3");

            IEnchantment fireDamage = new HeartbeatBigTickEnchantment();
            fireDamage.Effect = new Damage(sound);
            fireDamage.ActivationPercent = 3;
            fireDamage.EnchantmentEndingDateTime = new DateTime(9999, 12, 31);

            IDamage damage = new Objects.Damage.Damage(new Dice(5, 6), DamageType.Fire);
            fireDamage.Parameter.Damage = damage;
            fireDamage.Parameter.TargetMessage = new TranslationMessage("A dragon sweeps down and breaths fire on you.");
            fireDamage.Parameter.Description = "a dragon";

            return fireDamage;
        }
    }
}
