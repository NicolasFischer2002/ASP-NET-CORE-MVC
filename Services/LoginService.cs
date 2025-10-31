using ASP_NET_CORE_MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_CORE_MVC.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _db;

        public LoginService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            return await _db.Usuarios
                            .AnyAsync(u => u.Nome == username && u.Senha == password);
        }
    }
}