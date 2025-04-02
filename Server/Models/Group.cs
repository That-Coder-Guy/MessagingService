namespace Server
{
    public class Group
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public Guid CreatorId { get; set; }

        public User Creator { get; set; }

        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public Group(string name, Guid creatorId, User creator)
        {
            Name = name;
            CreatorId = creatorId;
            Creator = creator;
        }
    }
}
