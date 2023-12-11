using System;
namespace HostingService.Domain.User.ValueObject
{
    public class Password
    {
        public string Value { get; }

        public Password(string value)
        {
            Value = value;
        }
    }
}

