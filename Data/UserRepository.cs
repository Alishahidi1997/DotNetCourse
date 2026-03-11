using DotnetAPI.Data; 
using DotnetAPI.Models; 

namespace DotnetAPI.Data
{
    public class UserRepository: IUserRepository
    {
        DataContextEF _EntityFramework; 
        public UserRepository(IConfiguration config)
        {
            _EntityFramework = new DataContextEF(config); 
        }
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _EntityFramework.Users.ToList<User>();
            return users;
        }

        public User GetUserSingle(int userId)
        {
            User? user = _EntityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if (user!= null)
                return user;
            throw new Exception("User Not Found"); 
        }
        public bool Save()
        {
            return _EntityFramework.SaveChanges() > 0; 
        }
        public void AddEntity<T>(T entityAdd)
        {
            if(entityAdd != null)
                _EntityFramework.Add(entityAdd); 
        } 

        public void RemoveEntity<T>(T entityAdd)
        {
            if(entityAdd != null)
                _EntityFramework.Remove(entityAdd); 
        } 

    }
}