using DotnetAPI.Models; 
namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool Save();
        public void AddEntity<T>(T entityAdd);
        public void RemoveEntity<T>(T entityAdd);
        public IEnumerable<User> GetUsers();
        public User GetUserSingle(int userId); 

    }
}