using System;
namespace HostingService.Domain.Abstractions
{
    public class ValueObject : IEquatable<ValueObject>
    {
        public bool Equals(ValueObject obj)
        {
            return Equals(obj as object);
        }
    }
}

