using WuphfWeb.Models;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
        Task<T> ConfirmEmail<T>(string userId, string code);
        Task<T> ChangePassword<T>(ChangePasswordModel changePasswordModel);
        Task<T> ResetPasswordToken<T>(ResetPasswordTokenModel tokenModel);
        Task<T> ResetPassword<T>(ResetPasswordModel resetPasswordModel);
    }
}
