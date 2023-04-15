namespace MonitorPet.Ui.Shared.Model.Exceptions;

public class ErrorViewModel 
{
    public int Code { get; set; } = (int)System.Net.HttpStatusCode.BadRequest;
    public string Message { get; set; } = string.Empty;

    public ErrorViewModel()
    {
    }

    public ErrorViewModel(int code, string message)
    {
        Code = code;
        Message = message;
    }
}