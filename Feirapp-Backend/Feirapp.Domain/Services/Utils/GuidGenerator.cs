namespace Feirapp.Domain.Services.Utils;

public static class GuidGenerator
{
    public static Guid Generate()
    {
        var randomBytes = Guid.NewGuid().ToByteArray();
        var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestamp);

        for (var i = 0; i < 6; i++)
        {
            randomBytes[10 + i] = timestamp[i];
        }
        
        return new Guid(randomBytes);
    }
}