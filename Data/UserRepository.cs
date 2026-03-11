

namespace DotnetAPI.Data
{
    public class UserRepository
    {
        DataContextEF _EntityFramework; 
        public UserRepository(IConfiguration config)
        {
            _EntityFramework = new DataContextEF(config); 
        }
        public bool Save()
        {
            return _EntityFramework.SaveChanges() > 0; 
        }
        public void AddEntity<T>(T entityAdd)
        {
            _EntityFramework.Add(entityAdd); 
        } 

        public void RemoveEntity<T>(T entityAdd)
        {
            _EntityFramework.Remove(entityAdd); 
        } 

    }
}