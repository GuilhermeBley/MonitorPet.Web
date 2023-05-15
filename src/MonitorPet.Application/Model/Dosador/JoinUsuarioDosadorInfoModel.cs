namespace MonitorPet.Application.Model.Dosador;

public class JoinUsuarioDosadorInfoModel : DosadorJoinUsuarioDosadorModel
{
    public double CurrentWeight { get; set; }
    public DateTime? LastSeen { get; set; }
}
