using Client.EventArguments;
using System.ComponentModel;
using WebSocketCommunication;

namespace Client.ViewModels
{
    public class LoginPageViewModel : IViewModel
    {
        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<StateChangedEventArgs>? StateChanged;
        #endregion

        #region Field
        private ClientWebSocket _socket;
        #endregion

        public LoginPageViewModel(ClientWebSocket socket)
        {
            _socket = socket;
        }
    }
}
