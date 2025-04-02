
namespace Server
{
    public class GroupUser
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; }

        public GroupUser(Guid userId, User user, Guid groupId, Group group)
        {
            UserId = userId;
            User = user;
            GroupId = groupId;
            Group = group;
        }
    }
}
