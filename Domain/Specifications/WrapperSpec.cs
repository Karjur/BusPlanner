using System.Linq.Expressions;

namespace Domain.Specifications
{
    public abstract class WrapperSpec<TEntity> : Spec<TEntity>
    {
        public abstract Spec<TEntity> WrapSpecs();

        public sealed override Expression<Func<TEntity, bool>> ToPredicate() => WrapSpecs().ToPredicate();
    }
}