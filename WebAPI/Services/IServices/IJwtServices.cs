namespace WebAPI.Services.IServices
{
    public interface IJwtServices
    {
        public string GenerateToken(string username, string role);
    }
}
