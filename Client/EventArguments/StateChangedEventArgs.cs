using Client.Enumerations;

namespace Client.EventArguments
{
    public class StateChangedEventArgs : EventArgs
    {
        public ApplicationState NewState { get; }

        public StateChangedEventArgs(ApplicationState newState)
        {
            NewState = newState;
        }
    }
}
