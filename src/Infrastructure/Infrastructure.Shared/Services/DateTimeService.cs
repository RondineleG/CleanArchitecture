using Application.Interfaces.Services;

using System;

namespace Infrastructure.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;

    public DateTime AddDaysInUTC(int days)
    {
        return DateTime.UtcNow.AddDays(days);
    }

    public DateTime AddMinutesInUtc(double minutes)
    {
        return DateTime.UtcNow.AddMinutes(minutes);
    }
}