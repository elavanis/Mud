using Objects.Global.GameDateTime.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.TagWrapper.TagWrapper;

namespace Objects.Global.GameDateTime
{
    public class InGameDateTime : IInGameDateTime
    {
        private ITime _time;
        public InGameDateTime(ITime time)
        {
            _time = time;
        }

        public Objects.GameDateTime.Interface.IGameDateTime GameDateTime
        {
            get
            {
                return GetDateTime(_time.CurrentDateTime);
            }
        }


        public Objects.GameDateTime.Interface.IGameDateTime GetDateTime(DateTime dateTime)
        {
            return new Objects.GameDateTime.GameDateTime(_time.CurrentDateTime);
        }
    }
}
