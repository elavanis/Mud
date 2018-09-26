using System;
using System.Collections.Generic;

namespace Objects.Global.GameDateTime.Interface
{
    public interface IInGameDateTime
    {
        Objects.GameDateTime.Interface.IGameDateTime GameDateTime { get; }
        Objects.GameDateTime.Interface.IGameDateTime GetDateTime(DateTime dateTime);
    }
}