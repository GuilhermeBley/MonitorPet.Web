namespace MonitorPet.Application.Tests.Email;

/// <summary>
/// Scooped inbox
/// </summary>
public sealed class FakeInbox
{
    private List<object[]> _inbox { get; } = new();
    private object[]? _lastInbox => _inbox.LastOrDefault();
    public object[]? LastInbox => _lastInbox;

    public void AddToLastInbox(params object[] args)
    {
        _inbox.Add(args);
    }

    public T? TryGetObjectLastInbox<T>()
        where T : class
    {
        var objsInbox = _lastInbox;

        if (objsInbox is null)
            return default;

        foreach(var objInbox in objsInbox)
        {
            var objConverted = objInbox as T;

            if (objConverted is null)
                continue;

            return objConverted;
        }

        return default;
    }
}