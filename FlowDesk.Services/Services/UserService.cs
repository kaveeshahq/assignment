using FlowDesk.Data.UnitOfWork;
using FlowDesk.Domain;
using FlowDesk.Domain.Exceptions;
using FlowDesk.Services.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace FlowDesk.Services.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async System.Threading.Tasks.Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with email {Email}", request.Email);

        var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email);
        if (emailExists)
            throw new DuplicateEmailException(request.Email);

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = passwordHash,
            Role = UserRole.TeamMember,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} created successfully", user.Id);
        return MapToResponse(user);
    }

    public async System.Threading.Tasks.Task<UserResponse?> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return null;

        return MapToResponse(user);
    }

    public async System.Threading.Tasks.Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user == null)
            return null;

        return MapToResponse(user);
    }

    public async System.Threading.Tasks.Task<IEnumerable<UserResponse>> GetActiveUsersAsync()
    {
        var users = await _unitOfWork.Users.GetActiveUsersAsync();
        return users.Select(MapToResponse);
    }

    public async System.Threading.Tasks.Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(MapToResponse);
    }

    public async System.Threading.Tasks.Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user {UserId}", id);

        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);

        if (!string.IsNullOrEmpty(request.FullName))
            user.FullName = request.FullName;

        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} updated successfully", user.Id);
        return MapToResponse(user);
    }

    public async System.Threading.Tasks.Task<bool> DeactivateUserAsync(int id)
    {
        _logger.LogInformation("Deactivating user {UserId}", id);

        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} deactivated", id);
        return true;
    }

    public async System.Threading.Tasks.Task<bool> ActivateUserAsync(int id)
    {
        _logger.LogInformation("Activating user {UserId}", id);

        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User {UserId} activated", id);
        return true;
    }

    public bool VerifyPasswordHash(string password, string hash)
    {
        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hash.Substring(0, 128))))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != Convert.FromBase64String(hash.Substring(128))[i])
                    return false;
            }
        }
        return true;
    }

    private static string HashPassword(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(salt) + Convert.ToBase64String(hash);
        }
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
