using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Guid SenderId { get; set; }

        public User Sender { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; }

        public Message(string content, Guid senderId, User sender, Guid groupId, Group group)
        {
            Content = content;
            SenderId = senderId;
            Sender = sender;
            GroupId = groupId;
            Group = group;
        }
    }
}
