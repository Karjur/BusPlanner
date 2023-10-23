using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace Domain.Common
{
    public abstract class ValueObject
    {
        private static readonly ConcurrentDictionary<Type, ImmutableArray<FieldInfo>> TypeFieldCache = new();

        public static bool operator ==(ValueObject? left, object? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject? left, object? right)
        {
            return !Equals(left, right);
        }
        
        public override bool Equals(object? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (other.GetType() != GetType())
            {
                return false;
            }

            // Objects are equal, if they are of same type and have all same field values 
            // (this counts also automatic properties, because all automatic properties have backing fields accessible through reflection).
            var thisInstanceFields = GetTypeFields(GetType());

            var areEqual = thisInstanceFields.All(field =>
            {
                var thisValue = field.GetValue(this);
                var otherValue = field.GetValue(other);

                if (thisValue == null)
                {
                    return otherValue == null;
                }

                if (otherValue == null)
                {
                    return false;
                }

                if (IsEnumerableAndNotString(thisValue))
                {
                    var enumerator1 = ((IEnumerable)thisValue).GetEnumerator();
                    var enumerator2 = ((IEnumerable)otherValue).GetEnumerator();

                    var next1 = enumerator1.MoveNext();
                    var next2 = enumerator2.MoveNext();
                    while (next1 && next2)
                    {
                        var a = enumerator1.Current;
                        var b = enumerator2.Current;

                        if (a == null)
                        {
                            return b == null;
                        }

                        if (!a.Equals(b))
                        {
                            return false;
                        }

                        next1 = enumerator1.MoveNext();
                        next2 = enumerator2.MoveNext();
                    }

                    return next1 == next2;
                }

                return otherValue.Equals(thisValue);
            });
            return areEqual;
        }

        public override int GetHashCode()
        {
            var hasher = new HashCode();

            var thisType = GetType();
            IEnumerable<object?> hashFields =
                GetTypeFields(thisType)
                    .Select(f => f.GetValue(this))
                    .Concat(new[] { thisType });

            foreach (var field in hashFields)
            {
                if (IsEnumerableAndNotString(field))
                {
                    foreach (var item in (IEnumerable)field!)
                    {
                        hasher.Add(item);
                    }
                }
                else
                {
                    hasher.Add(field);
                }
            }

            return hasher.ToHashCode();
        }

        private IEnumerable<FieldInfo> GetTypeFields(Type type)
        {
            if (type == null)
                return Enumerable.Empty<FieldInfo>();

            return TypeFieldCache.GetOrAdd(
                type,
                _ => type
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Concat(GetTypeFields(type.BaseType!))
                    .ToImmutableArray()
            );
        }

        private bool IsEnumerableAndNotString(object? obj)
            => obj is IEnumerable
            && !(obj is string);
    }
}
#nullable restore