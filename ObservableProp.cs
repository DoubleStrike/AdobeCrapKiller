using System.ComponentModel;

// Obsolete: Use CommunityToolkit.Mvvm.ComponentModel.ObservableObject instead?

namespace AdobeCrapKiller {
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
