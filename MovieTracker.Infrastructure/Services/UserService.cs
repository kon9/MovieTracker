using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Core.Repositories;
using MovieTracker.Core.Services;
using MovieTracker.Core.Utilities;

namespace MovieTracker.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.ExistsByUsernameAsync(createUserDto.Username))
                throw new InvalidOperationException("Username already exists");

            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
                throw new InvalidOperationException("Email already exists");

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = PasswordHasher.HashPassword(createUserDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (!string.IsNullOrEmpty(updateUserDto.Username) && updateUserDto.Username != user.Username)
            {
                if (await _userRepository.ExistsByUsernameAsync(updateUserDto.Username))
                    throw new InvalidOperationException("Username already exists");
                user.Username = updateUserDto.Username;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email) && updateUserDto.Email != user.Email)
            {
                if (await _userRepository.ExistsByEmailAsync(updateUserDto.Email))
                    throw new InvalidOperationException("Email already exists");
                user.Email = updateUserDto.Email;
            }

            var updatedUser = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _userRepository.ExistsAsync(id))
                throw new InvalidOperationException("User not found");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _userRepository.ExistsByUsernameAsync(username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _userRepository.ExistsByEmailAsync(email);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password");

            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid username or password");

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // In a real application, you would generate a JWT token here
            return $"User {user.Username} logged in successfully";
        }

        public async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return false;

            return PasswordHasher.VerifyPassword(password, user.PasswordHash);
        }
    }
} 