using System.ComponentModel;

namespace AdobeCrapKiller {
    // Obsolete: Use CommunityToolkit.Mvvm.ComponentModel.ObservableObject instead?

    /// <summary>
    /// Represents a property that notifies listeners when its value changes.
    /// </summary>
    /// <remarks>This class implements the INotifyPropertyChanged interface, allowing data binding clients to
    /// be notified of changes to the Value property. It is useful in scenarios such as MVVM where property change
    /// notification is required for UI updates.</remarks>
    /// <typeparam name="T">The type of the value stored by the property.</typeparam>
    public class ObservableProp<T> : INotifyPropertyChanged {
        private T _value = default!;

        public T Value {
            get => _value;
            set { _value = value; NotifyPropertyChanged(nameof(Value)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
