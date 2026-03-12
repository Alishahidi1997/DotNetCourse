namespace DotnetAPI.DTO
{
    public partial class UserRegistrationDto
    {
        public string? UserName{get; set;}
        public string? Password{get; set;}
        public string? PasswordConfirmation{get; set;}

        public UserRegistrationDto()
        {
            if(UserName == null)
                UserName = "";
            if(Password == null)
                Password = ""; 
            if(PasswordConfirmation == null)
                PasswordConfirmation = ""; 
        }
    }
}