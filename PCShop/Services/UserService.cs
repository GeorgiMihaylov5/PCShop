using Microsoft.AspNetCore.Identity;
using PCShop.Abstraction;
using PCShop.Data;
using PCShop.Entities;
using PCShop.Entities.Enums;

namespace PCShop.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User ChangePassword(string username, string oldPassword, string newPassword, string confirmNewPassoword)
        {
            var user = Get(username);

            if (user is null)
            {
                throw new NullReferenceException();
            }

            var hasher = new PasswordHasher<User>();

            if (hasher.VerifyHashedPassword(user, user.Password, oldPassword) != PasswordVerificationResult.Success)
            {
                throw new NullReferenceException();
            }

            if (newPassword != confirmNewPassoword)
            {
                throw new NullReferenceException();
            }

            user.Password = hasher.HashPassword(user, newPassword);

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User CreateEmployee(string firstName, string lastName, string username, string email)
        {
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
                IsAdministrator = false,
                RegisterDate = DateTime.UtcNow,
                Role = UserRole.Emoloyee,
            };


            var hasher = new PasswordHasher<User>();

            user.Password = hasher.HashPassword(user, "employee123");

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User EditProfile(string username, string firstName, string lastName, int age, string phoneNumber)
        {
            var user = Get(username);

            if (user is null)
            {
                throw new NullReferenceException();
            }
            
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Age = age;
            user.PhoneNumber = phoneNumber;


            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User Get(string username)
        {
            if (username is null)
            {
                throw new ArgumentNullException();
            }

            return _context.Users
                .FirstOrDefault(x => x.Username == username);
        }

        public ICollection<User> GetClients()
        {
            return _context.Users
                .Where(x => x.Role == UserRole.Client)
                .ToList();
        }

        public ICollection<User> GetEmployees()
        {
            return _context.Users
                .Where(x => x.Role == UserRole.Emoloyee)
                .ToList();
        }

        public User Login(string username, string password)
        {
            if (username is null || password is null)
            {
                throw new ArgumentNullException();
            }

            var user = Get(username);

            if (user is null)
            {
                throw new InvalidOperationException();
            }

            var hasher = new PasswordHasher<User>();

            if (hasher.VerifyHashedPassword(user,user.Password, password) == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }

        public User Register(string firstName, string lastName, string username, string email, string password, string confirmPassowrd)
        {
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
                IsAdministrator = false,
                RegisterDate = DateTime.UtcNow,
                Role = UserRole.Client,
            };

            if (password != confirmPassowrd)
            {
                throw new InvalidOperationException();
            }

            var hasher = new PasswordHasher<User>();

            user.Password = hasher.HashPassword(user, password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Promote(string username)
        {
            ChangeIsAdmin(username, true);
        }

        public void Demote(string username)
        {
            ChangeIsAdmin(username, false);
        }
       
        private void ChangeIsAdmin(string username, bool isAdmin)
        {
            var user = Get(username);

            if (user is null)
            {
                throw new NullReferenceException();
            }

            if (user.Role != UserRole.Emoloyee)
            {
                throw new InvalidOperationException();
            }

            user.IsAdministrator = true;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
