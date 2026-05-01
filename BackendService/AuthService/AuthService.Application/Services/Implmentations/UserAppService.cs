using Authservice.Domain.Entites;
using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstractions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace AuthService.Application.Services.Implmentations
{
    public class UserAppService : IUserAppService
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserAppService(IUserRepository userRepository,
                              IMapper mapper,
                              IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
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
                    var UserModel= _mapper.Map<UserDTO>(user);
                    UserModel.Token = GenerateJwtToken(UserModel);
                    return UserModel;
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


        private string GenerateJwtToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int ExpireMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);

            var claims = new[] {
                             new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                             new Claim(JwtRegisteredClaimNames.Email, user.Email),
                             new Claim("Roles", string.Join(",",user.Roles)),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                             };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                            _configuration["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.UtcNow.AddMinutes(ExpireMinutes), //token expiry minutes
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
