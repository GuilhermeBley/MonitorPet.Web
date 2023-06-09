using AutoMapper;

namespace MonitorPet.Ui.Server.Profiles;

public class ViewModelsProfile : Profile
{
    public ViewModelsProfile()
    {
        CreateMap<Shared.Model.User.CreateUserViewModel, Application.Model.User.CreateUserModel>().ReverseMap();
        CreateMap<Shared.Model.User.QueryUserViewModel, Application.Model.User.QueryUserModel>().ReverseMap();
        CreateMap<Shared.Model.Dosador.DosadorJoinUsuarioDosadorViewModel, Application.Model.Dosador.DosadorJoinUsuarioDosadorModel>().ReverseMap();
        CreateMap<Shared.Model.User.UpdateUserViewModel, Application.Model.User.UpdateUserModel>().ReverseMap();

        CreateMap<Shared.Model.Agendamento.AgendamentoViewModel, Application.Model.Agendamento.AgendamentoModel>().ReverseMap();
        CreateMap<Shared.Model.Agendamento.CreateAgendamentoViewModel, Application.Model.Agendamento.CreateAgendamentoModel>().ReverseMap();
        CreateMap<Shared.Model.Agendamento.UpdateAgendamentoViewModel, Application.Model.Agendamento.UpdateAgendamentoModel>().ReverseMap();

        CreateMap<Shared.Model.Dosador.PutDosadorViewModel, Application.Model.Dosador.UpdateDosadorModel>()
            .ForMember(
                src => src.Name,
                opt => opt.MapFrom(from => from.NewName))
            .ForMember(
                src => src.ImgStream,
                opt => opt.MapFrom(from => from.NewImage == null || from.NewImage.Length < 1 ? Stream.Null : new MemoryStream(from.NewImage)));
        CreateMap<Shared.Model.Dosador.JoinUsuarioDosadorInfoViewModel, Application.Model.Dosador.JoinUsuarioDosadorInfoModel>().ReverseMap();

        CreateMap<Shared.Model.PesoHistorico.ConsumptionIntervalViewModel, Application.Model.PesoHistorico.ConsumptionIntervalModel>().ReverseMap();
        CreateMap<Shared.Model.PesoHistorico.ConsumptionViewModel, Application.Model.PesoHistorico.ConsumptionModel>().ReverseMap();

        CreateMap<Shared.Model.Email.CreateOrUpdateRoleEmailUserViewModel, Application.Model.Email.CreateOrUpdateRoleEmailUserModel>().ReverseMap();
        CreateMap<Shared.Model.Email.QueryRoleEmailUserViewModel, Application.Model.Email.QueryRoleEmailUserModel>().ReverseMap();
        CreateMap<Shared.Model.Email.EmailTypeViewModel, Application.Model.Email.EmailTypeModel>().ReverseMap();
    }
}