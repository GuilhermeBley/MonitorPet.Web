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
    }
}