using ASP_NET_CORE_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_CORE_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoginService _loginService;

        public AccountController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // GET: /Account/Index  (página de login)
        [HttpGet]
        public IActionResult Index()
        {
            // Limpa qualquer sessão anterior
            HttpContext.Session.Remove("IsAuthenticated");
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            ModelState.Clear();

            // Validação básica dos campos
            if (string.IsNullOrWhiteSpace(username))
                ModelState.AddModelError("username", "O campo Usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "O campo Senha é obrigatório.");

            if (!ModelState.IsValid)
                return View("Index");

            bool loginValido = await _loginService.ValidateCredentialsAsync(username, password);

            if (loginValido)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View("Index");
        }

        // GET: /Account/Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            var flag = HttpContext.Session.GetString("IsAuthenticated");
            if (flag != "true")
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            ViewData["User"] = "UsuárioDemo";
            return View();
        }
    }
}