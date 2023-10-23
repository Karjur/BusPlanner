using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class AnySpec<T> : Spec<T>
    {
        public override Expression<Func<T, bool>> ToPredicate()
        {
            return x => true;
        }
    }
}