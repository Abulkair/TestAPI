public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Authenticate(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        if (user == null) return null;

        // Генерация токена (например, JWT)
        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        // Логика генерации токена
    }
}
