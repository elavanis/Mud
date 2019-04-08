using Objects.Item.Interface;
using Objects.Global;
using Objects.Effect.Interface;
using Shared.Sound.Interface;
using System.Diagnostics.CodeAnalysis;
using Objects.Magic.Interface;
using Objects.Room.Interface;

namespace Objects.Effect
{
    public class Replenish : IEffect
    {
        [ExcludeFromCodeCoverage]
        public ISound Sound { get; set; }

        /// <summary>
        /// Used to replenish an item.  IE pickup an object and another appears.  Used to replenish rose in garden.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parameter"></param>
        public void ProcessEffect(IEffectParameter parameter)
        {
            //Because one of the enchantment parameters is this one blanking that one will erase the information from the passed parameters
            //so we copy the values we need out before they are lost.
            IRoom parameterRoom = parameter.ObjectRoom;
            IItem parameterItem = parameter.Item;

            //Blank out the enchantment parameter reference to itself.
            if (parameter.Item.Enchantments.Count > 0)
            {
                foreach (IEnchantment enchantment in parameter.Item.Enchantments)
                {
                    enchantment.Parameter.Item = null;
                    enchantment.Parameter.ObjectRoom = null;
                    enchantment.Parameter.Target = null;
                }
            }

            IItem newItem = GlobalReference.GlobalValues.Serialization.Deserialize<IItem>(GlobalReference.GlobalValues.Serialization.Serialize(parameterItem));

            parameterRoom.AddItemToRoom(newItem);
        }
    }
}

