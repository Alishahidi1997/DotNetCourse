using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AutoMapper;

namespace DotnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserControllerEF : ControllerBase
    {
        IMapper _mapper;
        IUserRepository _userRepository; 
        public UserControllerEF(IConfiguration config, IUserRepository userRepository)
        {
            
            _userRepository = userRepository; 
            _mapper = new Mapper(new MapperConfiguration(cfg =>{
                cfg.CreateMap<UserDTO, User>();
                }));
        }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        
        return _userRepository.GetUsers(); 
        
    }
    [HttpGet("GetUserSingle")]
    public User GetUserSingle(int userId)
    {
        User? user = _userRepository.GetUserSingle(userId);
        return user; 
    }
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
        {
            User? user1 = GetUserSingle(user.UserId); 
            if (user1 != null)
            {
                user1.Active = user.Active; 
                user1.Email = user.Email; 
                user1.FirstName = user.FirstName; 
                user1.LastName = user.LastName; 
                user1.Gender = user.Gender; 
                if(_userRepository.Save())
                    return Ok(); 
                throw new Exception("Save error");
            }
            throw new Exception("No User Found111");
            
        }
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDTO user)
        {
            User user1 = _mapper.Map<User>(user); 
            // User? user1 = dataEF.Users.Where(u=> user.UserId == u.UserId).FirstOrDefault<User>(); 
         
                _userRepository.AddEntity<User>(user1); 
                if(_userRepository.Save())
                    return Ok(); 
                throw new Exception("No User Found");
        }
    [HttpPost("DeleteUser")]
    public IActionResult DeleteUser(int userId)
        {
            User? user1 = GetUserSingle(userId);
            if (user1 != null)
            {
                _userRepository.RemoveEntity<User>(user1);
                if(_userRepository.Save())
                    return Ok(); 
                throw new Exception("No User Found");
            }
            throw new Exception("No User Found");
        }
            
}
}

