using System;
namespace HostingService.Domain.Abstractions
{
    public class TypeIdValueBase : IEquatable<TypeIdValueBase>
    {
        public Guid Value { get; }

        protected TypeIdValueBase(Guid value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypeIdValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypeIdValueBase other)
        {
            return this.Value == other.Value;
        }
        public static bool operator ==(TypeIdValueBase obj1, TypeIdValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(TypeIdValueBase x, TypeIdValueBase y)
        {
            return !(x == y);
        }
    }
}

