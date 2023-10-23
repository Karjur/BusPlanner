namespace Shared.Helpers
{
    /// <summary>
    /// Standard Unit type that serves as void in generic arguments.
    /// </summary>
    /// <remarks>
    /// Source: https://chtenb.dev/?page=unit-cs
    /// </remarks>
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Value = new Unit();

        public override bool Equals(object? obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public static bool operator ==(Unit left, Unit right) => left.Equals(right);
        public static bool operator !=(Unit left, Unit right) => !(left == right);
        public bool Equals(Unit other) => true;
        public override string ToString() => "()";
    }
}
