using System;

namespace Application.Interfaces;

public interface IDateTimeService
{
    DateTime NowUtc { get; }

    DateTime AddDaysInUTC(int days);

    DateTime AddMinutesInUtc(double minutes);
}