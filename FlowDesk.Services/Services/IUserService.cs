using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    Task<UserResponse?> GetUserByIdAsync(int id);
    Task<UserResponse?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserResponse>> GetActiveUsersAsync();
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);
    Task<bool> DeactivateUserAsync(int id);
    Task<bool> ActivateUserAsync(int id);
}
