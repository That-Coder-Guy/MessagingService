using Microsoft.EntityFrameworkCore;
using WebSocketCommunication;
using WebSocketCommunication.Server;
using WebSocketUtilities;

namespace Server
{
    public class MessagerClientHandler : WebSocketHandler
    {
        private User? _clientAccount;

        protected override void OnConnected(object? sender, EventArgs e)
        {
            
        }

        protected override void OnConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            
        }

        protected override void OnDisconnected(object? sender, DisconnectEventArgs e)
        {
            
        }

        protected override void OnMessageReceived(object? sender, MessageEventArgs e)
        {
            IPacket packet = PacketParser.Deserialize(e.Data);
            using (DatabaseContext context = new DatabaseContext())
            {
                if (packet is CreateAccountPacket createAccountPacket)
                {
                    User newUser = new User(
                        createAccountPacket.Username,
                        createAccountPacket.Password
                        );
                    context.Users.Add(newUser);

                    context.SaveChanges(); // TODO: Add exception handling for concurrancy exceptions or add a AddUser method to the DatabaseContext class that handles the logic.

                    _clientAccount = newUser;
                }
            }
        }
    }
}
