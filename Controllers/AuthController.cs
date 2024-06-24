using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Models;
using HotelBookingWeb.Utility;
using HotelBookingWeb.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Runtime.ConstrainedExecution;
using HotelBookingWeb.Service;
using Microsoft.AspNet.Identity;

namespace HotelBookingWeb.Controllers
{

    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IUserService userService)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _userService = userService;
        }

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto UserID = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInUser(loginResponseDto);
                await SignInUser(UserID);

                _tokenProvider.SetToken(loginResponseDto.Token);
                TempData["success"] = "Login Successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Login Unsuccessfully....Please correct Username or Password";
                return RedirectToAction("Login", "Auth");
            }
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }
                assingRole = await _authService.AssignRoleAsync(obj);
                if (assingRole != null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
            };

            ViewBag.RoleList = roleList;
            return View(obj);
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            TempData["success"] = "LogOut Successful";
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region SignInUser
        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            //Identity Claims
            identity.AddClaim(new Claim(ClaimTypes.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        #endregion


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (ModelState.IsValid)
            {
                if (forgotPasswordDto.NewPassword == forgotPasswordDto.ConfirmPassword)
                {
                    ResponseDto responseDto = await _userService.GetUserByEmailAsync(forgotPasswordDto.Email);

                    if (responseDto != null && responseDto.IsSuccess)
                    {
                        ApplicationUserDto result = JsonConvert.DeserializeObject<ApplicationUserDto>(Convert.ToString(responseDto.Result));

                        /*var userId = result.Id;
                        ResponseDto responseUser = await _userService.GetUserByIdAsync(userId);

                        if (responseUser != null && responseUser.IsSuccess)
                        {
                            ApplicationUserDto model = JsonConvert.DeserializeObject<ApplicationUserDto>(Convert.ToString(responseUser.Result));

                            var passHash = new PasswordHasher().HashPassword(forgotPasswordDto.NewPassword);
                            model.PasswordHash = passHash;

                        }*/

                        var passHash = new PasswordHasher().HashPassword(forgotPasswordDto.NewPassword);
                        result.PasswordHash = passHash;

                        ResponseDto response = await _userService.UpdateUserByEmailAsync(result);

                        if (response != null && response.IsSuccess)
                        {
                            TempData["success"] = "Your New Password is successfully changed....";
                            return RedirectToAction("Login", "Auth");
                        }
                    }
                    return View(forgotPasswordDto);
                }
                else
                {
                    TempData["error"] = "Please enter correct Password";
                    return RedirectToAction("ForgotPassword", "Auth");
                }
            }


            return View();
        }

    }
}
