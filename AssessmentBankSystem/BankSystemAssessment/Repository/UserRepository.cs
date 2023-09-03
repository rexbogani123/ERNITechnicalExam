using BankSystemAssessment.Data;
using BankSystemAssessment.Dto;
using BankSystemAssessment.Interfaces;
using BankSystemAssessment.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BankSystemAssessment.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContex _context;
        public UserRepository(DataContex context)
        {
            _context = context;
        }

        public bool CreateUser(string UserName, string Password, User user)
        {
           
            // Create a new user with the provided username and password
            var newUser = new User
            {
                Username = UserName,
                Password = Password 
            };

            // Perform user creation logic, such as adding the user to a repository or database
            try
            {
                _context.Add(newUser);
                 return Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public User GetUser(string name)
        {
            return _context.Users.Where(p => p.Username == name).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(p => p.Id).ToList();
        }

        public User GetUserTrimToUpper(User userCreate)
        {
            return GetUsers().FirstOrDefault(c => c.Username.Trim().ToUpper() == userCreate.Username.TrimEnd().ToUpper());
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool userExists(int userId)
        {
            return _context.Users.Any(p => p.Id == userId);
        }

        public bool Withdraw(int userId, decimal Balance)
        {
            var users = GetUser(userId);
            users.Balance -= Balance;
            _context.Users.Update(users);
            return Save();

        }
        public bool Deposit(int userId, decimal Balance)
        {
            var users = GetUser(userId);
            users.Balance += Balance;
            _context.Update(users);
            return Save();

        }
        public bool Transfer(int SenderUserID, int ReceiverUserID, decimal Amount)
        {
           var senderUserId = GetUser(SenderUserID);
            var receiverUserId = GetUser(ReceiverUserID);
            senderUserId.Balance -= Amount;
            receiverUserId.Balance += Amount;
            _context.Update(senderUserId);
            _context.Update(receiverUserId);
            return Save();

        }

        
    }
}
