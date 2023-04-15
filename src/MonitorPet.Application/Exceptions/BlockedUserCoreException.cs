using MonitorPet.Core.Exceptions;

namespace MonitorPet.Application.Exceptions;

public class BlockedUserCoreException : UnauthorizedCoreException
{
    public const string DefaultMessageBlockedUserCoreException = "Usuário bloqueado.";
    public DateTime LockoutEnd { get; }

    public BlockedUserCoreException(DateTime lockOutEnd)
        : base(DefaultMessageBlockedUserCoreException)
        => LockoutEnd = lockOutEnd;
}