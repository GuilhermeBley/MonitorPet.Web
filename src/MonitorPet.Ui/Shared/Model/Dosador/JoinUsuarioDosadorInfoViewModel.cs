namespace MonitorPet.Ui.Shared.Model.Dosador;

public class JoinUsuarioDosadorInfoViewModel : DosadorJoinUsuarioDosadorViewModel
{
    public double CurrentWeight { get; set; }
    public DateTime? LastSeen { get; set; }
    public DateTime? LastRelease{ get; set; }
}
