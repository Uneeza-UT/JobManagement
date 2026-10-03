using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Identity;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.Exceptions;
using JobManagement.Application.Models.Email;
using JobManagement.Application.Models.Identity;
using JobManagement.Application.Validations.Identity;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobManagement.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly FrontendSettings _frontendSettings;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IOptions<FrontendSettings> frontendSettings,
            ICurrentUserService currentUserService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _frontendSettings = frontendSettings.Value;
            _currentUserService = currentUserService;
            _emailSender = emailSender;
        }



        public async Task<AuthResponse> Login(AuthRequest request)
        {
            //Validate the request
            var validator = new AuthRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid credentials. ", validationResult);

            }



            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found", request.Email);
            }


            // Verify the user's password
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                throw new NotFoundException($"Credentials for {request.Email} aren't valid", request.Email);
            }


            //Generate a JWT token
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);


            //Create the authentication response
            var response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return response;
        }


        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            //Validate the request
            var validator = new RegistrationRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid request. ", validationResult);
            }


            //Create a new user 
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);


            if (!result.Succeeded)
            {
                // Collect Identity validation errors
                StringBuilder errorString = new StringBuilder();

                foreach(var err in result.Errors)
                {
                    errorString.AppendFormat(".{0}\n", err.Description);
                }

                throw new BadRequestException($"{errorString}");
            }


            //Assign the selecetd role to the new user
            await _userManager.AddToRoleAsync(user, request.Role);


            //Notify the user about successful registration through email
            await _emailSender.SendEmail(new EmailMessageData
            {
                To = user.Email,
                Subject = "Welcome to the Job Management System",
                Body = $"Hello {user.FirstName},\n\n" +
                   "Your account has been successfully created.\n\n" +
                   "Thank you,\n" +
                   "Job Management System"
            });


            return new RegistrationResponse() { UserId = user.Id };
        }


        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            // Retrieve the user's claims and roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);


            // Convert the user's roles into role claims
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();


            // Create the claims to include in the JWT token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);


            // Create the security key and signing credentials

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            // Create the JWT token with issuer, audience, claims, and expiration
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }



        //Method to change password from profile when user is logged in
        public async Task ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), _currentUserService.UserId);
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                   Environment.NewLine,
                   result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }

            await _emailSender.SendEmail(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Your Password was Changed",
                Body = $"Hello {user.FirstName},\n\n" +
                        "Your password for the Job Management System has been changed.\n\n" +
                        "If you made this change, no further action is required.\n\n" +
                        "If you did not change your password, please contact the system administrator immediately.\n\n" +
                        "Thank you,\n" +
                        "Job Management System"
            });

        }



        //Takes user email and sends a reset password link
        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Email);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            //Url for full-stack job management application

            //var resetUrl =
            //$"{_frontendSettings.ResetPasswordUrl}" +
            //$"?email={Uri.EscapeDataString(user.Email!)}" +
            //$"&token={Uri.EscapeDataString(token)}";


            await _emailSender.SendEmail(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Reset Your Password",
                Body = $"Hello {user.FirstName},\n\n" +
                        "We received a request to reset your password.\n\n" +
                        $"Reset your password using the following token:\n{token}\n\n" +
                        "If you did not request a password reset, you can safely ignore this email.\n\n" +
                        "Thank you,\n" +
                        "Job Management System"
            });

        }

        // Reset the user's password using the provided reset token 
        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), _currentUserService.UserId);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword);


            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                throw new BadRequestException(errors);
            }


            await _emailSender.SendEmail(new EmailMessageData
            {
                To = user.Email!,
                Subject = "Your Password was Reset",
                Body = $"Hello {user.FirstName},\n\n" +
                        "Your password for the Job Management System has been changed.\n\n" +
                        "If you made this change, no further action is required.\n\n" +
                        "If you did not change your password, please contact the system administrator immediately.\n\n" +
                        "Thank you,\n" +
                        "Job Management System"
            });

        }
    }
}
