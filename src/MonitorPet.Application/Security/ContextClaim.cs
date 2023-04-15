namespace MonitorPet.Application.Security;

public abstract class ContextClaim
{
    public abstract Task<CurrentClaim?> GetCurrentClaim();
    public async Task<CurrentClaim> GetRequiredCurrentClaim()
        => await GetCurrentClaim()
            ?? throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autenticado.");
}