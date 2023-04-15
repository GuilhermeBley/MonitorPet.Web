using MonitorPet.Ui.Shared.Model.Exceptions;

namespace MonitorPet.Ui.Client.Model;

internal class ResultErrors
{
    public bool ContainsError { get; } = false;
    public IEnumerable<ErrorViewModel> Errors { get; } = Enumerable.Empty<ErrorViewModel>();
    public IEnumerable<string> ErrorsMessages => Errors.Select(error => error.Message);

    public ResultErrors(bool containsError, IEnumerable<ErrorViewModel> errors)
    {
        ContainsError = containsError;
        Errors = errors;
    }
}
