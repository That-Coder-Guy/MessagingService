﻿using Client.Enumerations;
using Client.EventArguments;
using System.ComponentModel;
using System.Windows;
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

        #region Methods
        public LoginPageViewModel(ClientWebSocket socket)
        {
            _socket = socket;
        }

        public void OnSignUpClicked(object sender, RoutedEventArgs e)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(ApplicationState.SignUp));
        }
        #endregion
    }
}
