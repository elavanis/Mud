using Objects.Global;
using Objects.Item.Interface;
using Objects.Mob;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item
{
    public class Item : BaseObject, IItem
    {
        public Item()
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


        private List<ItemAttribute> _attributes = null;
        public List<ItemAttribute> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new List<ItemAttribute>();
                }
                return _attributes;
            }
        }

        private List<MobileObject.MobileAttribute> _mobAttributes = null;
        public virtual List<MobileObject.MobileAttribute> AttributesForMobileObjectsWhenEquiped
        {
            get
            {
                if (_mobAttributes == null)
                {
                    _mobAttributes = new List<MobileObject.MobileAttribute>();
                }
                return _mobAttributes;
            }
        }

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
