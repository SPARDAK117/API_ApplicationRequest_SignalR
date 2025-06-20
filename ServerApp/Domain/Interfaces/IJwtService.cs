namespace Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(long userId, string username, string role);
    }
}
