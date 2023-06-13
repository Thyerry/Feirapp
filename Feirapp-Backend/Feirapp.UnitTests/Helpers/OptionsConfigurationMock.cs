using Microsoft.Extensions.Options;

namespace Feirapp.UnitTests.Helpers
{
    public class OptionsConfigurationMock<T> : IOptions<T> where T : class
    {
        public T Value { get; }

        public OptionsConfigurationMock(T value)
        {
            Value = value;
        }
    }
}