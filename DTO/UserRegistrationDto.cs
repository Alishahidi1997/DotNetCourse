namespace DotnetAPI.DTO
{
    public partial class UserRegistrationDto
    {
        public string? UserName{get; set;}
        public string? Password{get; set;}
        public string? PasswordConfirmation{get; set;}

        public string? FirstName {get; set; } 
        public string? LastName {get; set; } 
        public string? Email {get; set; } 
        public string? Gender {get; set; } 
        public bool Active {get; set; } 
        public UserRegistrationDto()
        {
            if(UserName == null)
                UserName = "";
            if(Password == null)
                Password = ""; 
            if(PasswordConfirmation == null)
                PasswordConfirmation = ""; 
            if(FirstName == null)
                FirstName = "";
            if(LastName == null)
                LastName = "";
            if(Email == null)
                Email = "";
            if(Gender == null)
                Gender = "";
        }
    }
}