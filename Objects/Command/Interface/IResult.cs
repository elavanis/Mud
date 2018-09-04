using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Command.Interface
{
    public interface IResult
    {
        string ResultMessage { get; set; }
        bool AllowAnotherCommand { get; set; }
    }
}
