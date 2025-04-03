using Client.Enumerations;
using Client.EventArguments;
using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using WebSocketCommunication;
using WebSocketUtilities;

namespace Client.ViewModels
{
    public class SignUpPageViewModel : IViewModel
    {
        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<StateChangedEventArgs>? StateChanged;
        #endregion

        #region Field
        private ClientWebSocket _socket;

        private string _username = "";

        private string _password = "";
        #endregion

        #region Properties
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }
        #endregion

        #region Methods
        public SignUpPageViewModel(ClientWebSocket socket)
        {
            _socket = socket;
        }

        public void OnSignUpClicked(object sender, RoutedEventArgs e)
        {
            if (Username != string.Empty && Password != string.Empty)
            {
                CreateAccountPacket packet = new CreateAccountPacket(Username, Password);
                _socket.Send(PacketParser.Serialize(packet));
                Username = "";
                Password = "";
            }
        }

        public void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(ApplicationState.Login));
        }
        #endregion
    }
}
