using BankSystemAssessment.Dto;
using BankSystemAssessment.Model;


namespace BankSystemAssessment.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        bool userExists(int userId);
        User GetUser(int id);
        User GetUser(string name);
        User GetUserTrimToUpper(User userCreate);
        bool CreateUser(string UserName,string Password, User user);
        bool Withdraw(int userId,decimal Balance);
        bool Deposit(int userId, decimal Balance);
        bool Transfer(int SenderUserID, int ReceiverUserID, decimal Amount);

        
        bool Save();
    }
}
