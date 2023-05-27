namespace MonitorPet.Ui.Shared.Model.Email;

public class QueryRoleEmailUserViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmailTypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
