using WuphfApi.Models;

namespace WuphfApi.Repository.IRepository
{
    public interface ICommentoRepository : IRepository<Commento>
    {
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
    }
}
