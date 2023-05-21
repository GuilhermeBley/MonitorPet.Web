namespace MonitorPet.Core.Entity;

public class RoleEmailUser : Entity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public HashSet<string> Roles { get; } = new();

    private RoleEmailUser(int id, int userId, HashSet<string> roles)
    {
        Id = id;
        UserId = userId;
        Roles = roles;
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
            !this.Roles.Equals(entityToCheck.Roles))
            return false;

        return true;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode() * 534534345;
    }

    public static RoleEmailUser Create(int id, int userId, params string[] roles)
    {
        if (userId < 1)
            throw new Core.Exceptions.CommonCoreException("Id de usuário inválido.");

        return new RoleEmailUser(id, userId, roles.ToHashSet());
    }

    public static RoleEmailUser CreateWithDefaultId(int userId, params string[] roles)
        => Create(1, userId, roles);
}
