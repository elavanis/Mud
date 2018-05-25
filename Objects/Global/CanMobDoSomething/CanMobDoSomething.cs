using Objects.Command;
using Objects.Command.Interface;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items;
using Objects.Item.Items.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Item.Item;
using static Objects.Mob.MobileObject;

namespace Objects.Global.CanMobDoSomething
{
    public class CanMobDoSomething : ICanMobDoSomething
    {
        public bool SeeDueToLight(IMobileObject mob)
        {
            IRoom room = mob.Room;

            if (room.Attributes.Contains(Room.Room.RoomAttribute.NoLight)
                    || (GlobalReference.GlobalValues.GameDateTime.InGameDateTime.Hour >= 12) //night
                        && !room.Attributes.Contains(Room.Room.RoomAttribute.Light)) //the room is not lit
            {
                //room is dark
                if (mob.AttributesCurrent.Contains(MobileObject.MobileAttribute.Infravision))
                {
                    return true;
                }

                foreach (IEquipment equipment in mob.EquipedEquipment)
                {
                    if (equipment.ItemPosition == Equipment.AvalableItemPosition.Held
                        && equipment.Attributes.Contains(Item.Item.ItemAttribute.Light))
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        public bool SeeObject(IMobileObject observer, IBaseObject objectToSee)
        {
            if (objectToSee == null)
            {
                return true;
            }

            if (object.ReferenceEquals(objectToSee, observer))
            {
                //these are the same so we don't want to observer ourself
                return false;
            }

            if (observer.Position == CharacterPosition.Sleep)
            {
                return false;
            }

            if (!SeeDueToLight(observer))
            {
                return false;
            }

            IItem item = objectToSee as IItem;
            if (item != null)
            {
                if (item.Attributes.Contains(ItemAttribute.Invisible)
                && !observer.AttributesCurrent.Contains(MobileAttribute.SeeInvisible))
                {
                    return false;
                }
            }

            IMobileObject mob = objectToSee as IMobileObject;
            if (mob != null)
            {
                if (mob != null)
                {
                    if (mob.AttributesCurrent.Contains(MobileAttribute.Invisibile)
                    && !observer.AttributesCurrent.Contains(MobileAttribute.SeeInvisible))
                    {
                        return false;
                    }
                }

                if (mob.AttributesCurrent.Contains(MobileAttribute.Hidden))
                {
                    //roll to see if the observer notices the hidden character
                    //the roll will be the observer dex & int vs the observer dex & int
                    if (GlobalReference.GlobalValues.Random.Next(mob.DexterityEffective) + GlobalReference.GlobalValues.Random.Next(mob.IntelligenceEffective)
                        > GlobalReference.GlobalValues.Random.Next(observer.DexterityEffective) + GlobalReference.GlobalValues.Random.Next(observer.IntelligenceEffective))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public IResult Move(IMobileObject performer)
        {
            switch (performer.Position)
            {
                case CharacterPosition.Sleep:
                    return new Result(false, "You can not move while asleep.");
                case CharacterPosition.Sit:
                    return new Result(false, "You can not move while sitting.");
                case CharacterPosition.Relax:
                    return new Result(false, "You can not move while relaxing.");
            }

            return null;
        }

        public bool Hear(IMobileObject observer, IBaseObject objectToHear)
        {
            if (objectToHear == null)
            {
                return true;
            }

            if (object.ReferenceEquals(observer, objectToHear))
            {
                //these are the same so we don't want to hear ourself
                return false;
            }

            //TODO Add mute ability here

            return true;
        }
    }
}
