using System;

namespace Application.Interfaces.Services;

public interface IDateTimeService
{
    DateTime NowUtc { get; }

    DateTime AddDaysInUTC(int days);

    DateTime AddMinutesInUtc(double minutes);
}