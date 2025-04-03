using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for SignUpControl.xaml
    /// </summary>
    public partial class SignUpPage : UserControl
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        
        private void OnSignUpClicked(object sender, RoutedEventArgs e)
        {
            ((SignUpPageViewModel)DataContext)?.OnSignUpClicked(sender, e);
        }

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            ((SignUpPageViewModel)DataContext)?.OnLoginClicked(sender, e);
        }
    }
}
