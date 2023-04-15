namespace MonitorPet.Core.Entity;

public abstract class Entity : IEntity
{
    public virtual Guid IdEntity { get; } = Guid.NewGuid();

    /// <summary>
    /// Check entities
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        var entity = obj as IEntity;
        if (entity is null)
            return false;

        return (base.Equals(obj) && this.IdEntity.Equals(entity.IdEntity));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 162731263;
    }

    public override string ToString()
    {
        return $"{IdEntity.ToString()}|{base.ToString()}";
    }
}