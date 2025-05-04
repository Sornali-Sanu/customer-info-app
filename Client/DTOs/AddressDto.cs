using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.DTOs
{
    public class AddressDto:INotifyPropertyChanged
    {
       

        private string city;
        private string street;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string City
        {
            get => city;
            set
            {
                city = value;
                NotifyPropertyChanged();
            }
        }
        public string Street
        {
            get => street;
            set
            {
                street = value;
                NotifyPropertyChanged();
            }
        }
    }
}
