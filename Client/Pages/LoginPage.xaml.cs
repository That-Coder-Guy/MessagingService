using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnSignUpClicked(object sender, RoutedEventArgs e)
        {
            ((LoginPageViewModel)DataContext)?.OnSignUpClicked(sender, e);
        }
    }
}
