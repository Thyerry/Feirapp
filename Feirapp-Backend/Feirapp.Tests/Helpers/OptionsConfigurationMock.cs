using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Feirapp.Tests.Helpers
{
    public class OptionsConfigurationMock<T> : IOptions<T> where T : class
    {
        public T Value { get; }

        public OptionsConfigurationMock(T value)
        {
            Value = value;
        }
    }


    public class ConfigurationManagerMock : IConfiguration
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }

        public string? this[string key]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}