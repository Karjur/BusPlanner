#nullable enable
using System.Linq.Expressions;
using Domain.Common;
using Domain.Specifications.LogicalOperators;

namespace Domain.Specifications
{
    /// <summary>
    /// "Specification"
    /// </summary>
    public abstract class Spec<TEntity> : ValueObject, ISpec<TEntity>
    {
        public static AnySpec<TEntity> Any => new AnySpec<TEntity>();
        public static Spec<TEntity> None => !Any;

        public IQueryable<TEntity> SatisfyEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(ToPredicate());
        }

        public IQueryable<TEntity> SatisfyEntitiesFrom(IEnumerable<TEntity> query)
        {

            try
            {
                return query.AsQueryable().Where(ToPredicate());
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Unsupported type for overload. Try casting with .AsQueryable() first.",
                    nameof(query), e);
            }
        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            return ToPredicate().Compile().Invoke(entity);
        }

        public bool IsNotSatisfiedBy(TEntity entity)
        {
            return !IsSatisfiedBy(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToPredicate();

        /// <summary> Logical AND operator for specifications. </summary>
        /// <remarks> Use "&&" for correctness because specifications use lazy (conditional) evaluation internally. </remarks>
        public static Spec<TEntity> operator &(Spec<TEntity> spec1, Spec<TEntity> spec2)
        {
            return new AndSpec<TEntity>(spec1, spec2);
        }

        /// <summary> Logical OR operator for specifications. </summary>
        /// <remarks> Use "||" for correctness because specifications use lazy (conditional) evaluation internally. </remarks>
        public static Spec<TEntity> operator |(Spec<TEntity> spec1, Spec<TEntity> spec2)
        {
            return new OrSpec<TEntity>(spec1, spec2);
        }

        /// <summary> Logical NOT operator for specifications. </summary>
        public static Spec<TEntity> operator !(Spec<TEntity> spec1)
        {
            return new NotSpec<TEntity>(spec1);
        }

        #region Non-operational boilerplate for conditional operators
        public static bool operator true(Spec<TEntity> spec)
        {
            return false; // No Operation - boilerplate for conditional operators.
        }

        public static bool operator false(Spec<TEntity> spec)
        {
            return false; // No Operation - boilerplate for conditional operators.
        }
        #endregion
    }

    public interface ISpecifier
    {
        public IEnumerable<T> SatisfyEntitiesFrom<T>(IQueryable<T> collection, ISpec<T> spec);

        public IEnumerable<T> SatisfyEntitiesFrom<T>(IEnumerable<T> collection, ISpec<T> spec);

        public bool IsSatisfiedBy<T>(T entity, ISpec<T> spec);
    }

    class Specifier : ISpecifier
    {
        public IEnumerable<T> SatisfyEntitiesFrom<T>(IQueryable<T> collection, ISpec<T> spec)
        {
            return spec.SatisfyEntitiesFrom(collection);
        }
        
        public IEnumerable<T> SatisfyEntitiesFrom<T>(IEnumerable<T> collection, ISpec<T> spec)
        {
            return spec.SatisfyEntitiesFrom(collection);
        }

        public bool IsSatisfiedBy<T>(T entity, ISpec<T> spec)
        {
            return spec.IsSatisfiedBy(entity);
        }
    }

    /// <summary> 
    /// If you want to implement then inherit from <see cref="Spec{T}"/> directly! 
    /// </summary>
    public interface ISpec<T> : ISpec
    {
        IQueryable<T> SatisfyEntitiesFrom(IQueryable<T> query);
        IQueryable<T> SatisfyEntitiesFrom(IEnumerable<T> query);
        bool IsSatisfiedBy(T entity);
    }

    // Marker interface for reflection.
    public interface ISpec
    {
    }
}
#nullable restore