namespace Boilerplate.Api
{
    public interface ICurrentUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string ProjectId { get; set; } 

    }
}
