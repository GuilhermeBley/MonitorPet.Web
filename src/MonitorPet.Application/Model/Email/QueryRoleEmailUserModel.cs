namespace MonitorPet.Application.Model.Email;

public class QueryRoleEmailUserModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmailTypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
