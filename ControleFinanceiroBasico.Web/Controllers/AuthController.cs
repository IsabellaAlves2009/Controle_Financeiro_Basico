using ControleFinanceiroBasico.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroBasico.Web.Controllers
{
    public class AuthController : Controller
    {
        public readonly SignInManager<IdentityUser> _signInManager;
        public readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("nova-conta")]
        public IActionResult Registrar()
        {
            return View();
        }


        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioRegistro);
            }

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(usuarioRegistro.Email, usuarioRegistro.Senha, false, true);
                return RedirectToAction("Index", "Home");
            }

            TempData["Erros"] = result.Errors;
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "")
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLogin login, string returnUrl = "")
        {
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, true);
            if (result.Succeeded)
            {
                return string.IsNullOrEmpty(returnUrl) ? RedirectToAction("Index", "Home") : LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                TempData["Erros"] = new List<IdentityError>
                {
                    new IdentityError{ Description = "O usuário está bloqueado"},
                };
                return View();
            }

            TempData["Erros"] = new List<IdentityError>
            {
                new IdentityError{ Description = "Usuário ou senha inválidos"},
            };
            return View();
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}