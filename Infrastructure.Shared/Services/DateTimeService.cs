using Application.Interfaces;

using System;

namespace Infrastructure.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;

    public DateTime AddDaysInUTC(int days) => DateTime.UtcNow.AddDays(days);

    public DateTime AddMinutesInUtc(double minutes) => DateTime.UtcNow.AddMinutes(minutes);
}