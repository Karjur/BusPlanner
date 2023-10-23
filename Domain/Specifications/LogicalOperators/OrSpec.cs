using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Domain.Specifications.Helpers;

namespace Domain.Specifications.LogicalOperators
{
    /// <remarks> Prefer "||" instead of using this class directly. </remarks>>
    public sealed class OrSpec<TEntity> : CompositeSpec<TEntity>
    {
        [JsonConstructor]
        public OrSpec(params Spec<TEntity>[] specs) : base(specs)
        {
        }
        
        public OrSpec(IEnumerable<Spec<TEntity>> specs) : base(specs.ToArray())
        {
        }

        protected override Expression<Func<TEntity, bool>> CombineExpressions(
            Expression<Func<TEntity, bool>> exp1, 
            Expression<Func<TEntity, bool>> exp2)
        {
            return exp1.Or(exp2);
        }
    }
}
