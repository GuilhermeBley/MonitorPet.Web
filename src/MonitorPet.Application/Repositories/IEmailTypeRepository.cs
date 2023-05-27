using MonitorPet.Application.Model.Email;

namespace MonitorPet.Application.Repositories;

public interface IEmailTypeRepository
{
    Task<IEnumerable<EmailTypeModel>> GetAll();
}
