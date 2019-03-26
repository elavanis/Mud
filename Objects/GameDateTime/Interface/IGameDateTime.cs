using System;

namespace Objects.GameDateTime.Interface
{
    public interface IGameDateTime
    {
        int Day { get; }
        Days DayName { get; }
        int Hour { get; }
        int Minute { get; }
        int Month { get; }
        Months MonthName { get; }
        int Year { get; }
        Years YearName { get; }
        decimal TotalDaysPastBegining { get; }

        IGameDateTime AddDays(int days);

        bool IsGreaterThan(IGameDateTime other);
        bool IsLessThan(IGameDateTime other);
    }
}