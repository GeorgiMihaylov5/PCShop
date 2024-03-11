using PCShop.Entities;

namespace PCShop.Abstraction
{
    public interface IJWTService
    {
        public int ExpiresDays { get; }
        public string CreateJWT(User user, string role);
    }
}
