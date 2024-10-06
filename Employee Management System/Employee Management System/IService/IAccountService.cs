using Employee_Management_System.DTO;

namespace Employee_Management_System
{
    public interface IAccountService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task EnsureDefaultAdminAsync();
    }
}
