using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebSocketUtilities
{
    public class PacketParser
    {
        #region Properties
        public byte[] PublicKey { get; }

        public byte[]? ForeignPublicKey { get; set; } = null;
        #endregion

        #region Fields
        private readonly byte[] _privateKey;

        private static readonly Dictionary<PacketId, Type> _packetMap = new() {
            { PacketId.CreateAccount, typeof(CreateAccountPacket) }
        };
        #endregion

        #region Methods
        public PacketParser()
        {
            using (RSA rsa = RSA.Create(2048))
            {
                PublicKey = rsa.ExportRSAPublicKey();
                _privateKey = rsa.ExportRSAPrivateKey();
            }
        }

        public MemoryStream Encrypt(MemoryStream data)
        {
            if (ForeignPublicKey == null)
            {
                throw new InvalidOperationException("ForeignPublicKey is not set.");
            }

            MemoryStream encryptedData = new MemoryStream();
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(ForeignPublicKey, out _);
                int bufferSize = rsa.KeySize / 8 - 2 * SHA256.HashSizeInBytes - 2;

                byte[] buffer = new byte[bufferSize];
                while (data.Position < data.Length)
                {
                    int bytesRead = data.Read(buffer);
                    encryptedData.Write(rsa.Encrypt(buffer[..bytesRead], RSAEncryptionPadding.OaepSHA256));
                }
            }
            encryptedData.Position = 0;
            return encryptedData;
        }

        public MemoryStream Decrypt(MemoryStream data)
        {
            MemoryStream decryptedData = new MemoryStream();
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(_privateKey, out _);
                int bufferSize = rsa.KeySize / 8 - 2 * SHA256.HashSizeInBytes - 2;

                byte[] buffer = new byte[bufferSize];
                while (data.Position < data.Length)
                {
                    int bytesRead = data.Read(buffer);
                    decryptedData.Write(rsa.Decrypt(buffer[..bytesRead], RSAEncryptionPadding.OaepSHA256));
                }
            }
            decryptedData.Position = 0;
            return decryptedData; 
        }

        public static MemoryStream Serialize<T>(T packet) where T : IPacket
        {
            MemoryStream data = new MemoryStream();
            
            JsonSerializer.Serialize(data, packet);
            data.Position = 0;
            return data;
        }

        public static IPacket Deserialize(MemoryStream data)
        {
            //data.Position = 0;
            try
            {
                using (JsonDocument documant = JsonDocument.Parse(data))
                {
                    PacketId packetId = (PacketId)documant.RootElement.EnumerateObject().First().Value.GetInt32();
                    if (_packetMap.TryGetValue(packetId, out Type? packetType))
                    {
                        data.Position = 0;
                        if (JsonSerializer.Deserialize(data, packetType) is IPacket packet)
                        {
                            return packet;
                        }

                        throw new ArgumentException($"MemoryStream could not be deserialized into {packetType.Name}", nameof(data));
                    }
                    throw new ArgumentException($"MemoryStream could not be deserialized into a packet", nameof(data));
                }
            }
            catch (Exception exc)
            {
                Debug.Print(exc.Message);
                throw;
            }
        }
        #endregion
    }
}
