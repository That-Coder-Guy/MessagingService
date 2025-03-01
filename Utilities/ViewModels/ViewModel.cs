using System.ComponentModel;
using Utilities.EventArguments;

namespace Utilities.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        public event EventHandler<StateChangedEventArgs>? StateChanged;
    }
}
