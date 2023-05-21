namespace MonitorPet.Core.Entity;

public class RoleEmailUser : Entity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public EmailConfigType Role { get; }

    private RoleEmailUser(int id, int userId, EmailConfigType role)
    {
        Id = id;
        UserId = userId;
        Role = role;
    }

    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;

        var entityToCheck = obj as RoleEmailUser;
        if (entityToCheck is null)
            return false;

        if (!this.Id.Equals(entityToCheck.Id) ||
            !this.UserId.Equals(entityToCheck.UserId) ||
            !this.Role.Equals(entityToCheck.Role))
            return false;

        return true;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode() * 534534345;
    }

    public static RoleEmailUser Create(int id, int userId, EmailConfigType role)
    {
        if (userId < 1)
            throw new Core.Exceptions.CommonCoreException("Id de usuário inválido.");

        return new RoleEmailUser(id, userId, role);
    }

    public static RoleEmailUser CreateWithDefaultId(int userId, EmailConfigType role)
        => Create(1, userId, role);
}
