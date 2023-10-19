using System;
namespace HostingService.Domain.User.ValueObject
{
    public class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            Value = value;
        }
    }
}

