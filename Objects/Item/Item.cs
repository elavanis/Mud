using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item
{
    public class Item : BaseObject, IItem
    {
        public Item(int id, int zone, string examineDescription, string lookDescription, string sentenceDescription, string shortDescription) : base(id, zone, examineDescription, lookDescription, sentenceDescription, shortDescription)
        {
        }

        #region Properties
        [ExcludeFromCodeCoverage]
        public virtual int Level { get; set; }

        [ExcludeFromCodeCoverage]
        public int Weight { get; set; } = 1;

        [ExcludeFromCodeCoverage]
        public virtual ulong Value { get; set; }
        #endregion Properties


        public override void FinishLoad(int zoneObjectSyncValue = -1)
        {
            base.FinishLoad(zoneObjectSyncValue);
            Value = GlobalReference.GlobalValues.DefaultValues.MoneyForNpcLevel(Level);
        }

        public object Clone()
        {
            IItem newItem = GlobalReference.GlobalValues.Serialization.Deserialize<IItem>(
                                          GlobalReference.GlobalValues.Serialization.Serialize(this));
            return newItem;
        }

        public List<ItemAttribute> Attributes { get; } = new List<ItemAttribute>();

        public virtual List<MobileObject.MobileAttribute> AttributesForMobileObjectsWhenEquiped { get; } = new List<MobileObject.MobileAttribute>();

        public enum ItemAttribute
        {
            NoGood,
            NoEvil,
            NoNeutral,
            Invisible,
            /// <summary>
            /// Produces light in the dark
            /// </summary>
            Light,
            /// <summary>
            /// Players can not pickup item
            /// </summary>
            NoGet

        }
    }
}
