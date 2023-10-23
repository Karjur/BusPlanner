using System.Linq.Expressions;
using Domain.Specifications.Helpers;

namespace Domain.Specifications.LogicalOperators
{
    /// <remarks> Prefer "&&" instead of using this class directly. </remarks>>
    public sealed class AndSpec<TEntity> : CompositeSpec<TEntity>
    {
        public AndSpec(IEnumerable<Spec<TEntity>> specifications) : this(specifications.ToArray())
        {
        }
        
        public AndSpec(params Spec<TEntity>[] specifications) : base(specifications)
        {
        }

        protected override Expression<Func<TEntity, bool>> CombineExpressions(
            Expression<Func<TEntity, bool>> exp1, 
            Expression<Func<TEntity, bool>> exp2)
        {
            return exp1.And(exp2);
        }
    }
}
