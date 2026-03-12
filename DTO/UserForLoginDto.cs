namespace DotnetAPI.DTO
{
    public partial class UserForLoginDto
    {
        public string? UserName{get; set;}
        public string? Password{get; set;}

        public UserForLoginDto()
        {
            if(UserName == null)
                UserName = "";
            if(Password == null)
                Password = ""; 
        }
    }
}