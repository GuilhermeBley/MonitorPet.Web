using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class UsuarioDosador : Entity
{
    public int Id { get; private set; }
    public Guid IdDosador { get; private set; }
    public int IdUser { get; private set; }

    private UsuarioDosador(int id, Guid idDosador, int idUser)
    {
        Id = id;
        IdDosador = idDosador;
        IdUser = idUser;
    }
    
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;
        
        var usuarioDosador = obj as UsuarioDosador;
        if (usuarioDosador is null)
            return false;

        if (!this.IdUser.Equals(usuarioDosador.IdUser) ||
            !this.IdDosador.Equals(usuarioDosador.IdDosador))
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 785865655;
    }

    public static UsuarioDosador Create(Guid idDosador, int idUser)
    {
        if (idDosador == default)
            throw new CommonCoreException("Dosador inválido.");

        if (idUser < 0)
            throw new CommonCoreException("Usuário inválido.");

        return new UsuarioDosador(0, idDosador, idUser);
    }
}