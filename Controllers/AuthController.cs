using DotnetAPI.Data;
using DotnetAPI.DTO;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace DotnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        DataContextDapper _dp; 
        IConfiguration _config; 
        public AuthController(IConfiguration config)
        {
            _dp = new DataContextDapper(config); 
            _config = config; 
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser(UserRegistrationDto userRegDTO)
        {
            if(userRegDTO.Password == userRegDTO.PasswordConfirmation)
                return Ok(); 
            throw new Exception("Passwords do not match."); 
                
        }
        [HttpPost("Login")]
        public IActionResult LoginUser(UserForLoginDto userForLogin)
        {
            
                return Ok(); 
                
        }
    }
}