namespace WebSocketUtilities
{
    public class PacketParser
    {
        #region Properties
        public string PublicKey { get; }

        public string ForeignPublicKey { get; set; }
        #endregion

        #region Fields
        private string _privateKey;
        #endregion

        #region Methods
        public PacketParser()
        {

        }

        public MemoryStream Encrypt(MemoryStream data)
        {

        }

        public MemoryStream Decrypt(MemoryStream data)
        {

        }

        public MemoryStream Serialize(IPacket packet)
        {

        }

        public IPacket Deserialize(MemoryStream data)
        {

        }
        #endregion
    }
}
