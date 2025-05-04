using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.DTOs
{
    public class CustomerDto : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private DateTime dateofBirth;
        private string mobileNo;
        private string email;
        private bool isRegular;
        private string? imageUrl;
        private string imageBase64;
        private string addressJson;
        private List<AddressDto>addresses=new List<AddressDto>() { new AddressDto()};

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id { get => id; set { NotifyPropertyChanged(); id = value; }  }
        public string Name { get => name; set { NotifyPropertyChanged(); name = value; } }
        public DateTime DateofBirth { get => dateofBirth; set { NotifyPropertyChanged(); dateofBirth = value; }  }
        public string MobileNo{ get => mobileNo; set { NotifyPropertyChanged(); mobileNo = value; } }
        public string Email { get => email; set { NotifyPropertyChanged(); email = value; }  }
        public bool IsRegular { get => isRegular; set { NotifyPropertyChanged(); isRegular = value; }  }
        public string? ImageUrl { get => imageUrl; set { NotifyPropertyChanged(); imageUrl = value; }  }
        public string ImageBase64 { get => imageBase64; set { NotifyPropertyChanged(); imageBase64 = value; } }
        public string AddressJson { get => addressJson; set { NotifyPropertyChanged(); addressJson = value; }  }
        public List<AddressDto> Addresses { get => addresses; set { NotifyPropertyChanged(); addresses = value; }  }

       
    }
}
