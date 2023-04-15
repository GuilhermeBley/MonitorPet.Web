using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class ResultAccessAccountViewModel
{
    public QueryUserViewModel? User { get; set; }
    public string? Token { get; set; }
}