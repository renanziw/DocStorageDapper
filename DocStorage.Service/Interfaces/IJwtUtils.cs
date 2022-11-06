namespace DocStorage.Service.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(Model.User user);
        public int? ValidateJwtToken(string token);
    }
}
