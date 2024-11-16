using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<DbSet<User>> _mockUserSet;
    private readonly Mock<ApplicationDbContext> _mockContext;

    public AuthServiceTests()
    {
        // Создаем мок-объект для DbSet<User>
        _mockUserSet = new Mock<DbSet<User>>();

        // Создаем мок-объект для ApplicationDbContext
        _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

        // Настраиваем контекст, чтобы возвращать наш мок-объект
        _mockContext.Setup(m => m.Users).Returns(_mockUserSet.Object);

        // Инициализируем AuthService с мок-контекстом
        _authService = new AuthService(_mockContext.Object);
    }

    [Fact]
    public void Authenticate_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Password = "hashed_password" // Здесь должен быть хешированный пароль
        };

        // Настраиваем мок-объект, чтобы возвращать пользователя
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new List<User> { user }.AsQueryable().Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(new List<User> { user }.AsQueryable().Expression);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(new List<User> { user }.AsQueryable().ElementType);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(new List<User> { user }.AsQueryable().GetEnumerator());

        // Act
        var token = _authService.Authenticate("testuser", "hashed_password");

        Assert.NotNull(token);
    }

   
