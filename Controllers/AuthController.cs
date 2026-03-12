using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.DTO;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation; 
using System.Text;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser(UserRegistrationDto userRegDTO)
        {
            
            if(userRegDTO.Password == userRegDTO.PasswordConfirmation){
                string sqlCheckEmail = $"Select UserName from TutorialAppSchema.Auth Where UserName = '{userRegDTO.UserName}'"; 
                if(_dp.LoadData<string>(sqlCheckEmail).Count() == 0){
                    byte[] passwordSalt = new byte[128/8]; 
                    using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt); 
                    }
                    byte[] passwordHash = GetPasswordHash(userRegDTO.Password, passwordSalt);
                    string sqlAddAuth = $@"Insert INTO TutorialAppSchema.Auth 
                    ([UserName],
                    [PasswordHash], 
                    [PasswordSalt]) Values('{userRegDTO.UserName}',@passwordHash,@passwordSalt )";
                    List<SqlParameter> sqlParam = new List<SqlParameter>(); 
                    SqlParameter passwordSalt1 = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary); 
                    passwordSalt1.Value = passwordSalt; 
                    SqlParameter passwordHash1 = new SqlParameter("@PasswordHash", SqlDbType.VarBinary); 
                    passwordHash1.Value = passwordHash; 
                    sqlParam.Add(passwordHash1); 
                    sqlParam.Add(passwordSalt1); 
                    if(_dp.ExecuteWithParams(sqlAddAuth, sqlParam) > 0){
                        string sql = @"INSERT INTO TutorialAppSchema.Users(
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active]
                        ) VALUES (" +
                            "'" + userRegDTO.FirstName + 
                            "', '" + userRegDTO.LastName +
                            "', '" + userRegDTO.Email + 
                            "', '" + userRegDTO.Gender + 
                            "', '" + 1 + 
                        "')";
                        if(_dp.ExecuteWithRow(sql)) 
                            return Ok();
                        throw new Exception("Failed to Add user!");
                    }
                }
                throw new Exception("user with the same email already exists"); 
            } 
            throw new Exception("Passwords do not match."); 
                
        }

        private byte[] GetPasswordHash(string? password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser(UserForLoginDto userForLogin)
        {
            
            string sqlUserName = $"select [PasswordHash], [PasswordSalt] from TutorialAppSchema.Auth Where UserName = '{userForLogin.UserName}'"; 
            UserForLoginConfirmationDto user1 = _dp.LoadDataSingle<UserForLoginConfirmationDto>(sqlUserName);
            byte[] passwordHash = GetPasswordHash(userForLogin.Password, user1.PasswordSalt); 
            for(int i = 0; i < passwordHash.Length; i ++)
            {
                if (passwordHash[i] != user1.PasswordHash[i])
                {
                    return StatusCode(401,"Incorrect Password"); 
                }
            }
            return Ok(); 
                
        }
    }
}