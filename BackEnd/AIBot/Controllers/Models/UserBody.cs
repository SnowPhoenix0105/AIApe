namespace Buaa.AIBot.Controllers.Models
{
    public class UserInfo
    {
        public string Name {get; set;}

        public string Email {get; set;}

        public string Password {get; set;}

        public int Uid {get; set;}

        // 0 is Traveler, 1 is User, 2 is Administrator
        public int Auth {get; set;}
    }
}