using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvenShare
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected Project ProjectContext { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
