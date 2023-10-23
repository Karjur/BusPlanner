using System.Linq.Expressions;
using Domain.Specifications.Helpers;

namespace Domain.Specifications.LogicalOperators
{
    /// <remarks> Prefer "!" instead of using this class directly. </remarks>>
    public class NotSpec<TEntity> : Spec<TEntity>
    {
        public Spec<TEntity> Spec { get; }

        public NotSpec(Spec<TEntity> specification)
        {
            Spec = specification;
        }

        public sealed override Expression<Func<TEntity, bool>> ToPredicate()
        {
            return Spec.ToPredicate().Not();
        }
    }
}