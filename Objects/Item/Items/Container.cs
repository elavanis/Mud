using Objects.Command;
using Objects.Command.Interface;
using Objects.Global;
using Objects.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.LoadPercentage.Interface;
using Objects.Mob.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Container : Item, IContainer, IOpenable, ILoadableItems
    {
        [ExcludeFromCodeCoverage]
        public bool Opened { get; set; } = false;

        [ExcludeFromCodeCoverage]
        public string OpenMessage { get; set; }

        [ExcludeFromCodeCoverage]
        public string CloseMessage { get; set; }

        [ExcludeFromCodeCoverage]
        public List<ILoadPercentage> LoadableItems { get; } = new List<ILoadPercentage>();

        private List<IItem> _items = null;
        public List<IItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<IItem>();
                }

                return _items;
            }
        }



        public IResult Open(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Engine.Event.Open(performer, this);
            Opened = true;
            return new Result(OpenMessage, false);
        }

        public IResult Close(IMobileObject performer)
        {
            GlobalReference.GlobalValues.Engine.Event.Close(performer, this);
            Opened = false;
            return new Result(CloseMessage, false);
        }
    }
}
