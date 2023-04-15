using MonitorPet.Application.Security;

namespace MonitorPet.Application.Tests.Context;

internal class TestContext : ContextClaim
{
    
    private static readonly AsyncLocal<ContextHolder> _contextCurrent = new AsyncLocal<ContextHolder>();

    /// <inheritdoc cref="IScrapContextAccessor.ScrapContext" path="*"/>
    public CurrentClaim? ClaimContext
    {
        get { return _contextCurrent.Value?.claimContext; }
        set
        {
            var holder = _contextCurrent.Value;
            if (holder != null)
            {
                holder.claimContext = null;
            }

            if (value != null)
            {
                _contextCurrent.Value = new ContextHolder() { claimContext = value };
            }
        }
    }
    
    public TestContext() { }

    /// <summary>
    /// Private unique holder class to context
    /// </summary>
    private class ContextHolder
    {
        public CurrentClaim? claimContext;
    }

    public async override Task<CurrentClaim?> GetCurrentClaim()
    {
        await Task.CompletedTask;
        return ClaimContext;
    }
}