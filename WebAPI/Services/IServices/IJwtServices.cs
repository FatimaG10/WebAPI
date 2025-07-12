namespace WebAPI.Services.IServices
{
    public interface IJwtServices
    {
        //interfas del generador
        public string GenerateToken(string username, string role);
    }
}
