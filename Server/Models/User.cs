using System.Text.RegularExpressions;

namespace Server
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; }

        public string Password { get; set; }

        public byte[]? ProfilePicture { get; set; }

        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<Group> CreatedGroups { get; set; } = new List<Group>();

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
