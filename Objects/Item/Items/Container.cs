using Objects.Command;
using Objects.Command.Interface;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Item.Items
{
    public class Container : Item, IContainer, IOpenable
    {
        [ExcludeFromCodeCoverage]
        public bool Opened { get; set; } = false;

        [ExcludeFromCodeCoverage]
        public string OpenMessage { get; set; }

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

        public IResult Open()
        {
            Opened = true;
            return new Result(OpenMessage, false);
        }
    }
}
