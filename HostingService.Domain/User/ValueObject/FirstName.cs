using System;
namespace HostingService.Domain.User.ValueObject
{
    public class FirstName
    {

        public string Value { get; }

        public FirstName(string value)
        {
            Value = value;
        }
    }
}

