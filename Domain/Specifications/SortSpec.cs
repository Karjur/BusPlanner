#nullable enable
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public abstract class SortSpec<T> : ISortSpec<T>,
        IEquatable<SortSpec<T>>
    {
        private readonly List<SortSpec<T>>
            _thenBySpecs = new List<SortSpec<T>>();

        protected SortSpec(bool descending)
        {
            Descending = descending;
        }

        public bool Descending { get; }
        
        public IQueryable<T> Sort(IQueryable<T> query)
        {
            var ordered = Descending 
                ? query.OrderByDescending(KeySelector) 
                : query.OrderBy(KeySelector);
        
            foreach (var thenBy in _thenBySpecs)
            {
                ordered = thenBy.Descending
                    ? ordered.ThenByDescending(thenBy.KeySelector)
                    : ordered.ThenBy(thenBy.KeySelector);
            }
        
            return ordered;
        }

        public SortSpec<T> ThenBy(SortSpec<T> thenBy)
        {
            _thenBySpecs.Add(thenBy);
            return this;
        }

        protected abstract Expression<Func<T, object>> KeySelector { get; }

        private bool AreThenBySpecsEqual(SortSpec<T> other)
        {
            if (_thenBySpecs.Count() != other._thenBySpecs.Count())
            {
                return false;
            }

            for (var i = 0; i < _thenBySpecs.Count(); i++)
            {
                if (!Equals(_thenBySpecs[i], other._thenBySpecs[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(SortSpec<T>? other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;

            return AreThenBySpecsEqual(other) 
                && Descending == other.Descending;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != GetType()) 
                return false;
            return Equals((SortSpec<T>)obj);
        }

        public override int GetHashCode()
        {
            var hasher = new HashCode();

            hasher.Add(GetType());
            hasher.Add(Descending);
            foreach (var spec in _thenBySpecs)
            {
                hasher.Add(spec);
            }

            return hasher.ToHashCode();
        }

        // For testing only
        protected SortSpec()
        {
        }
    }

    /// <summary>
    /// Do not use this interface directly for implementing specifications, use abstract SortSpecification<TEntity, Tkey> class for that.
    /// </summary>
    public interface ISortSpec<TEntity>
    {
        IQueryable<TEntity> Sort(IQueryable<TEntity> query);
    }
}
#nullable restore