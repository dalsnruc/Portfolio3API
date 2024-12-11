namespace WebApi.Models.UserModels
{
    public class CreateUserModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }
        public string Phonenumber { get; set; }

    }
}
