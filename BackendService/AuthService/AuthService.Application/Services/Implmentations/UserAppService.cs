using Authservice.Domain.Entites;
using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstractions;
using AutoMapper;
// Alias for BCrypt.Net.BCrypt to reduce verbosity and improve code readability
// Allows using BC.HashPassword() instead of BCrypt.Net.BCrypt.HashPassword()
// Used for secure password hashing and verification operations in user authentication
using BC = BCrypt.Net.BCrypt;

namespace AuthService.Application.Services.Implmentations
{
    public class UserAppService : IUserAppService
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        public UserAppService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            //return users.Select(u => new UserDTO
            //{
            //    UserId = u.Id,
            //    Name = u.Name,
            //    Email = u.Email,
            //    PhoneNumber = u.PhoneNumber,
            //    Roles = u.Roles.Select(r => r.Name).ToArray(),
            //});

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public UserDTO LoginUser(LoginDTO loginDTO)
        {
            User user = _userRepository.GetUserByEmail(loginDTO.Email);
            if (user! != null)
            {
                // Verify the login password against the stored hashed password using BCrypt
                // BC.Verify() securely compares the plain-text password from the login request
                // with the hashed password stored in the database to prevent plain-text password storage
                // This returns true if passwords match, false otherwise
                // BCrypt uses salt and key stretching to prevent rainbow table and brute force attacks
                bool isValidPassword = BC.Verify(loginDTO.Password, user.Password);
                if (isValidPassword)
                {
                    return _mapper.Map<UserDTO>(user);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public bool SignUpUser(SignUpDTO signUpDTO, string Role)
        {
            User user = _userRepository.GetUserByEmail(signUpDTO.Email);
            if (user == null)
            {
                user = _mapper.Map<User>(signUpDTO);
                user.Password = BC.HashPassword(signUpDTO.Password);
                return _userRepository.RegisterUser(user, Role);
            }
            else
            {
                return false; // User already exists
            }
        }
    }
}
