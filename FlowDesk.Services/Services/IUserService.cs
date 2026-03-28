using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface IUserService
{
    System.Threading.Tasks.Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    System.Threading.Tasks.Task<UserResponse?> GetUserByIdAsync(int id);
    System.Threading.Tasks.Task<UserResponse?> GetUserByEmailAsync(string email);
    System.Threading.Tasks.Task<IEnumerable<UserResponse>> GetActiveUsersAsync();
    System.Threading.Tasks.Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    System.Threading.Tasks.Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);
    System.Threading.Tasks.Task<bool> DeactivateUserAsync(int id);
    System.Threading.Tasks.Task<bool> ActivateUserAsync(int id);
}
