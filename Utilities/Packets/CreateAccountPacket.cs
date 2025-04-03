namespace WebSocketUtilities
{
    public class CreateAccountPacket : IPacket
    {
        public PacketId Id { get; } = PacketId.CreateAccount;

        public string Username { get; }

        public string Password { get; }

        public CreateAccountPacket(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
