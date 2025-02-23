using System.ComponentModel;
using WebSocketCommunication;

namespace Client.ViewModels
{
    public class ConnectionPageViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Field
        private ClientWebSocket _socket;

        private string _connectionStatusMessage = "Connecting...";
        #endregion

        #region Properties
        public string ConnectionStatusMessage
        {
            get => _connectionStatusMessage;
            set
            {
                _connectionStatusMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionStatusMessage)));
            }
        }
        #endregion

        public ConnectionPageViewModel(ClientWebSocket socket)
        {
            _socket = socket;
            _socket.ConnectionFailed += OnConnectionFailed;
            _socket.Connected += OnConnected;
        }

        private void OnConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            switch (e.Error)
            {
                case WebSocketError.Timeout:
                    ConnectionStatusMessage = "Connection attempt timed out.";
                    break;

                case WebSocketError.Success:
                    ConnectionStatusMessage = "An unknown error occured during the connection attempt.";
                    break;

                case WebSocketError.InvalidMessageType:
                    ConnectionStatusMessage = "Unknown message received during connection attempt.";
                    break;

                case WebSocketError.Faulted:
                    ConnectionStatusMessage = "The server is unreachable";
                    break;

                case WebSocketError.NativeError:
                    ConnectionStatusMessage = "An unknown non-connection related error occured durring connection attempt.";
                    break;

                case WebSocketError.NotAWebSocket:
                    ConnectionStatusMessage = "Connection attempt was made to something other than a web socket server.";
                    break;

                case WebSocketError.UnsupportedVersion:
                    ConnectionStatusMessage = "Connection attempt terminated due to outdated server software";
                    break;

                case WebSocketError.UnsupportedProtocol:
                    ConnectionStatusMessage = "Connection attempt terminated due to an unsupported web socket protocal being requested.";
                    break;

                case WebSocketError.HeaderError:
                    ConnectionStatusMessage = "An error occurred while parsing the HTTP headers during the opening handshake.";
                    break;

                case WebSocketError.ConnectionClosedPrematurely:
                    ConnectionStatusMessage = "The connection attempt was terminated unexpectedly by the server.";
                    break;

                case WebSocketError.InvalidState:
                    ConnectionStatusMessage = "The client is in an invalid state to preform a connection attempt from.";
                    break;
            }
        }



        private void OnConnected(object? sender, EventArgs e)
        {
            ConnectionStatusMessage = "Connected!";
        }
    }
}
