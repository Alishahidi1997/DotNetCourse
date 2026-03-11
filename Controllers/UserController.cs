using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
         DataContextDapper dataDapper; 
        public UserController(IConfiguration config)
        {
            dataDapper= new DataContextDapper(config); 
        }
        [HttpGet("testConnection")]
        public DateTime testConnection()
        {
            return dataDapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }
        [HttpGet("GetUser/{testVal}")]
        public string[] GetUser(string testVal)
        {
            string[] users = new string[]{"test1", "test2",testVal};
            return users; 
        }
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        
        IEnumerable<User> users = dataDapper.LoadData<User>("Select * From TutorialAppSchema.Users");
        return users;
        
    }
     [HttpGet("GetSingleUser/{userId}")]
    // public IEnumerable<User> GetUsers()
    public User GetSingleUser(int userId)
    {
        string sql = $@"
            SELECT *
            FROM TutorialAppSchema.Users
                WHERE UserId = {userId.ToString()}" ; //"7"
        User user = dataDapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult UpdateUser(User user)
        {
            int ac = Convert.ToInt32(user.Active); 
            string sql = $@"Update TutorialAppSchema.Users 
            set FirstName = '{user.FirstName}', 
            LastName = '{user.LastName}',
            Email = '{user.Email}',
            Gender = '{user.Gender}',
            Active = {ac}
             where UserId = {user.UserId};"; 
            Console.WriteLine(sql); 
            if (dataDapper.ExecuteWithRow(sql))
            {
                return Ok(); 
            }
            throw new Exception("Error updating"); 
        }
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDTO user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (" +
                "'" + user.FirstName + 
                "', '" + user.LastName +
                "', '" + user.Email + 
                "', '" + user.Gender + 
                "', '" + user.Active + 
            "')";
        
        Console.WriteLine(sql);

        if (dataDapper.ExecuteWithRow(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to Add User");
    }
    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
        {
            string sql = $"Delete from TutorialAppSchema.Users where UserId={userId}"; 
            Console.WriteLine(sql);

        if (dataDapper.ExecuteWithRow(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to Add User");
        }
}
}

