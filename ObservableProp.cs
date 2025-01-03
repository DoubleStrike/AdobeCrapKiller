using System.ComponentModel;

namespace AdobeCrapKiller
{
    public class ObservableProp<T> : INotifyPropertyChanged
    {
        private T _value;

        public T Value
        {
            get => _value;
            set { _value = value; NotifyPropertyChanged(nameof(Value)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
