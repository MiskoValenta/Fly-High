using FlyHigh.DTOs;  // Obsahuje RegisterUserDto, UserSummaryDto, atd.
using FlyHigh.DTOs.LogInDTOs;
using FlyHigh.DTOs.TeamMemberDTOs;
using FlyHigh.DTOs.UserDTOs;
using FlyHigh.Models;
using FlyHigh.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlyHigh.Controllers
{
    [ApiController]
    [Route("Auth/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        // POST: api/auth/register - ✅ používám STÁVAJÍCÍ RegisterUserDto
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto registerDto)
        {
            // Kontrola, zda uživatel již existuje
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Uživatel s tímto emailem již existuje");
            }

            // ✅ Vytvoření nového uživatele se VŠEMI fieldy z RegisterUserDto
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,      // ✅ Z původního DTO
                DateOfBirth = registerDto.DateOfBirth ?? DateTime.MinValue,  // ✅ Z původního DTO
                EmailConfirmed = true,
                IsActive = true
            };

            // Vytvoření uživatele s heslem
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Přiřazení základní role
            await _userManager.AddToRoleAsync(user, "Player");

            // Získání rolí a generování tokenu
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(24),
                User = new UserSummaryDto  // ✅ Používám STÁVAJÍCÍ UserSummaryDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Email = user.Email,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    IsActive = user.IsActive
                },
                Roles = roles.ToList()
            };

            return Ok(response);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            // Najít uživatele podle emailu
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("Neplatný email nebo heslo");
            }

            // Kontrola hesla
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest("Neplatný email nebo heslo");
            }

            // Kontrola, zda je uživatel aktivní
            if (!user.IsActive)
            {
                return BadRequest("Uživatelský účet je deaktivován");
            }

            // Aktualizace posledního přihlášení
            user.LastLoginAt = DateTime.Now;
            await _userManager.UpdateAsync(user);

            // Získání rolí a generování tokenu
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(24),
                User = new UserSummaryDto  // ✅ Používám STÁVAJÍCÍ UserSummaryDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Email = user.Email,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    IsActive = user.IsActive
                },
                Roles = roles.ToList()
            };

            return Ok(response);
        }

        // GET: api/auth/me - ✅ vrací STÁVAJÍCÍ UserDetailDto
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<ActionResult<UserDetailDto>> GetCurrentUser()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest("Neplatný token");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Uživatel nenalezen");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Používám STÁVAJÍCÍ UserDetailDto
            var userDetail = new UserDetailDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                ProfilePictureUrl = user.ProfilePictureUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Teams = new List<TeamMembershipDto>() // Zatím prázdné, můžeš načíst později
            };

            return Ok(userDetail);
        }
    }
}
