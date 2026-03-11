using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserControllerEF : ControllerBase
    {
         DataContextEF dataEF; 

        public UserControllerEF(IConfiguration config)
        {
            dataEF= new DataContextEF(config); 
        }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        
        return dataEF.Users.ToList<User>(); 
        
    }
    [HttpGet("GetUserSingle")]
    public User GetUserSingle(int userId)
    {
        User? user = dataEF.Users
        .Where(u => u.UserId == userId).FirstOrDefault<User>(); 
        if(user!=null)
            return user;
        return null; 
    }
    [HttpPut("EditUser/{user}")]
    public IActionResult EditUser(User user)
        {
            User? user1 = dataEF.Users.Where(u=> user.UserId == u.UserId).FirstOrDefault<User>(); 
            if (user1 != null)
            {
                user1.Active = user.Active; 
                user1.Email = user.Email; 
                user1.FirstName = user.FirstName; 
                user1.LastName = user.LastName; 
                user1.Gender = user.Gender; 
                if(dataEF.SaveChanges() > 0)
                    return Ok(); 
                throw new Exception("No User Found");
            }
            throw new Exception("No User Found");
            
        }
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDTO user)
        {
            User user1 = new User(); 
            // User? user1 = dataEF.Users.Where(u=> user.UserId == u.UserId).FirstOrDefault<User>(); 
                user1.Active = user.Active; 
                user1.Email = user.Email; 
                user1.FirstName = user.FirstName; 
                user1.LastName = user.LastName; 
                user1.Gender = user.Gender; 
                dataEF.Add(user1); 
                if(dataEF.SaveChanges() > 0)
                    return Ok(); 
                throw new Exception("No User Found");
        }
    [HttpPost("DeleteUser")]
    public IActionResult DeleteUser(int userId)
        {
            User? user1 = dataEF.Users.Where(u=> userId == u.UserId).FirstOrDefault<User>(); 
            if (user1 != null)
            {
                dataEF.Users.Remove(user1);
                if(dataEF.SaveChanges() > 0)
                    return Ok(); 
                throw new Exception("No User Found");
            }
            throw new Exception("No User Found");
        }
            
}
}

