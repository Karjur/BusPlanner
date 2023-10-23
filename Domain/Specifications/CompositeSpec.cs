#nullable enable
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    // Base class for AndSpec and OrSpec.
    public abstract class CompositeSpec<T> : Spec<T>, IEquatable<CompositeSpec<T>>
    {
        public ImmutableHashSet<Spec<T>> Specs { get; }

        protected abstract Expression<Func<T, bool>> CombineExpressions(
            Expression<Func<T, bool>> exp1,
            Expression<Func<T, bool>> exp2);

        protected CompositeSpec(params Spec<T>[] specs)
        {
            if (specs == null) throw new ArgumentNullException(nameof(specs));

            Specs = GetSpecsToReduce(specs)
                .ToImmutableHashSet(); // HashSet gets rid of duplicates, i.e it reduces for us.
        }

        private IEnumerable<Spec<T>> GetSpecsToReduce(IEnumerable<Spec<T>> specs)
        {
            foreach (var spec in specs)
            {
                if (spec is CompositeSpec<T> compositeSpec
                    && compositeSpec.GetType() == GetType())
                {
                    foreach (var compInnerSpec in compositeSpec.Specs)
                        yield return compInnerSpec;
                }
                else
                    yield return spec;
            }
        }

        public sealed override Expression<Func<T, bool>> ToPredicate()
        {
            Expression<Func<T, bool>>? predicate = null;

            foreach (var spec in Specs)
            {
                predicate = (predicate == null)
                    ? spec.ToPredicate()
                    : CombineExpressions(predicate, spec.ToPredicate());
            }

            if (predicate == null) // By default will satisfy (if no arguments were specified)
                return Any.ToPredicate();

            return predicate;
        }

        public override bool Equals(object? other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (other.GetType() != GetType())
                return false;

            var otherSpec = (CompositeSpec<T>) other;
            return Specs.SetEquals(otherSpec.Specs);
        }

        public override int GetHashCode()
        {
            var hasher = new HashCode();
            foreach (var spec in Specs)
            {
                hasher.Add(spec);
            }

            hasher.Add(GetType());

            return hasher.ToHashCode();
        }

        public bool Equals(CompositeSpec<T>? other)
        {
            return Equals((object?) other);
        }
    }
}
#nullable restore