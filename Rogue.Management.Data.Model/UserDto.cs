namespace Rogue.Management.Data.Model
{
    public class UserDto
    {
        public UserDto(string username, string email, string passwordHash, DateTime createdAt)
        {
            this.Username = username;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.CreatedAt = createdAt;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ProjectDto>? Projects { get; set; }
    }
}
