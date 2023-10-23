using Domain.Specifications;

namespace Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }

    protected ISpecifier Specifier { get; set; } = new Specifier();
}
