using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class EmailConfigType : Entity
{
    public int Id { get; private set; }
    public string Type { get; private set; }
    public string? Description { get; private set; }

    private EmailConfigType(int id, string type, string? description)
    {
        Id = id;
        Type = type;
        Description = description;
    }

    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;

        var entityToCheck = obj as EmailConfigType;
        if (entityToCheck is null)
            return false;

        if (!this.Id.Equals(entityToCheck.Id) ||
            Type.Equals(entityToCheck.Type) ||
            Description != entityToCheck.Description)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 45645623;
    }

    public static EmailConfigType Create(int id, string type, string? description)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new CommonCoreException("Tipo inválido.");

        return new EmailConfigType(id, type, description);
    }

    public static EmailConfigType CreateWithDefaultId(string type, string? description)
        => Create(1, type, description);
}
