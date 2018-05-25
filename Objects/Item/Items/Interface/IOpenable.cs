using Objects.Command.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Item.Items.Interface
{
    public interface IOpenable
    {
        IResult Open();
        string OpenMessage { get; set; }
    }
}
