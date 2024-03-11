using PCShop.Entities;

namespace PCShop.Abstraction
{
    public interface IUserService
    {
        public ICollection<User> GetClients();
        public ICollection<User> GetEmployees();
        public User Get(string username);
        public User Register(string firstName, string lastName, string username, string email, string password, string confirmPassow0rd);
        public User Login(string username, string password);
        public User EditProfile(string username, string firstName, string lastName, int age, string phoneNumber);
        public User ChangePassword(string username, string oldPassword, string newPassword, string confirmNewPassowrd);
        public void Promote(string username);
        public void Demote(string username);
        public User CreateEmployee(string firstName, string lastName, string username, string email);
    }
}
