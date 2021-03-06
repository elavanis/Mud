﻿using Objects.Command.Interface;
using Objects.Mob.Interface;

namespace Objects.Item.Items.Interface
{
    public interface IOpenable
    {
        bool Opened { get; set; }
        IResult Open(IMobileObject performer);
        IResult Close(IMobileObject performer);
        string OpenMessage { get; set; }
        string CloseMessage { get; set; }
    }
}
