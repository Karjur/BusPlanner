using System.Reflection;

namespace Domain.Common;

public abstract class Enumeration : IComparable
{
    public int Id { get; private set; }

    public string Value { get; } = null!;

    public string DisplayName { get; private set; } = null!;

    protected Enumeration()
    {
    }

    protected Enumeration(int id, string value, string displayName)
    {
        Id = id;
        Value = value;
        DisplayName = displayName;
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public override bool Equals(object? obj)
    {
        var otherValue = obj as Enumeration;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType() == obj?.GetType();
        var valueMatches = Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }


    public static T FromValue<T>(string value) where T : Enumeration, new()
    {
        var matchingItem = parse<T, string>(value, "value", item => item.Value == value);
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
    {
        var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
        return matchingItem;
    }

    private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = $"'{value}' is not a valid {description} in {typeof(T)}";
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public int CompareTo(object? other)
    {
        var enumeration = (Enumeration)other!;
        return string.Compare(Value, enumeration.Value, StringComparison.Ordinal);
    }
}