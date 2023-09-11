using System;

namespace Feirapp.Tests.Fixtures;

public static class FixtureUtils
{
    public static DateTime FakeDate(Bogus.Faker f)
    {
        var date = f.Date.PastDateOnly();
        return new DateTime(date.Year, date.Month, date.Day).ToUniversalTime();
    }
}