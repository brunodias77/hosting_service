using System;
namespace HostingService.Domain.User.ValueObject
{
    public class LastName
    {
        public string Value { get; private set; }

        public LastName(string value)
        {
            this.Value = value;
        }
    }
}

