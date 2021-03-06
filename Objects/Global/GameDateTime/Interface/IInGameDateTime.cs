﻿using System;

namespace Objects.Global.GameDateTime.Interface
{
    public interface IInGameDateTime
    {
        Objects.GameDateTime.Interface.IGameDateTime GameDateTime { get; }
        Objects.GameDateTime.Interface.IGameDateTime GetDateTime(DateTime dateTime);
    }
}