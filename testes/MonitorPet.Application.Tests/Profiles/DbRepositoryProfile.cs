using AutoMapper;

namespace MonitorPet.Application.Tests.Profiles;

public class DbRepositoryProfile : Profile
{
    public DbRepositoryProfile()
    {
        CreateMap<Core.Entity.Dosador, ModelDb.DosadorDbModel>();
        CreateMap<Core.Entity.User, ModelDb.UserDbModel>();
        CreateMap<Core.Entity.UsuarioDosador, ModelDb.UsuarioDosadorDbModel>()
            .ForMember(src => src.IdUsuario, opt => opt.MapFrom(dest => dest.IdUser));

        CreateMap<ModelDb.UsuarioDosadorDbModel, Application.Model.Dosador.UsuarioDosadorModel>().ReverseMap();
        CreateMap<ModelDb.DosadorDbModel, Application.Model.Dosador.DosadorModel>().ReverseMap();
        CreateMap<ModelDb.UserDbModel, Application.Model.User.UserModel>().ReverseMap();
    }
}