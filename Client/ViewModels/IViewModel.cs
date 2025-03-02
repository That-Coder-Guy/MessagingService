using System.ComponentModel;
using Client.EventArguments;

namespace Client.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        public event EventHandler<StateChangedEventArgs>? StateChanged;
    }
}
