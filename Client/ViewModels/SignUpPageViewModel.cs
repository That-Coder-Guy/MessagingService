using Client.Enumerations;
using Client.EventArguments;
using System.ComponentModel;
using System.Windows;
using WebSocketCommunication;

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
        #endregion

        #region Properties

        #endregion

        #region Methods
        public SignUpPageViewModel(ClientWebSocket socket)
        {
            _socket = socket;
        }

        public void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(ApplicationState.Login));
        }
        #endregion
    }
}
