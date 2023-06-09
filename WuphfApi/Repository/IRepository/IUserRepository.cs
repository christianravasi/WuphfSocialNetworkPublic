using WuphfApi.Models;
using WuphfApi.Models.DTO;

namespace WuphfApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username, string email);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ApplicationUser> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
