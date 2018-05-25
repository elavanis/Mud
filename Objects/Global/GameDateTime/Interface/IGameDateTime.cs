using System;
using System.Collections.Generic;

namespace Objects.Global.GameDateTime.Interface
{
    public interface IGameDateTime
    {
        DateTime InGameDateTime { get; }
        string InGameFormatedDateTime { get; }
        List<string> YearNames { get; }
        DateTime GetDateTime(DateTime dateTime);

        string BuildFormatedDateTime();
        string BuildFormatedDateTime(DateTime dateTime);
    }
}