using System.Linq.Expressions;
using Domain.Common;

namespace Domain.Specifications
{
    /// <summary>
    /// Marker for reflection.
    /// </summary>
    public interface IMapSpec
    {
        Type MapFrom { get; }
        Type MapTo { get; }
    }

    public interface IMapSpec<TMapped> : IMapSpec
    {
        IQueryable<TMapped> Map(IQueryable query);
        TMapped Map(object from);
    }

    public interface IMapSpec<T, TMapped> : IMapSpec<TMapped>
    {
        IQueryable<TMapped> Map(IQueryable<T> query);
        IEnumerable<TMapped> Map(IEnumerable<T> query);
    }

    public abstract class MapSpec<T, TMapped> : ValueObject, IMapSpec<T, TMapped>
    {
        public IQueryable<TMapped> Map(IQueryable query)
        {
            return Map((IQueryable<T>)query);
        }

        public Type MapFrom => typeof(T);
        public Type MapTo => typeof(TMapped);

        public TMapped Map(object from)
        {
            return ToSelector().Compile().Invoke((T)from);
        }

        public IQueryable<TMapped> Map(IQueryable<T> query)
        {
            return query.Select(ToSelector());
        }

        public IEnumerable<TMapped> Map(IEnumerable<T> query)
        {
            return query.Select(ToSelector().Compile());
        }

        protected abstract Expression<Func<T, TMapped>> ToSelector();
    }
}