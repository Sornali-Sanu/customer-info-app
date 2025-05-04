
using Client.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Client;

public partial class AddCustomerPage : ContentPage,INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private CustomerDto customerDto;
    private ImageSource pickedImageSource;
    public ObservableCollection<AddressDto> AddressList { get; set; } = new ObservableCollection<AddressDto>();


    public ImageSource PickedImageSource
    {
        get => pickedImageSource;
        set
        {
            if (pickedImageSource != value)
            {
                pickedImageSource = value;
                OnPropertyChanged(); 
            }
        }
    }

    public AddCustomerPage()
	{
		InitializeComponent();
        customerDto = new CustomerDto();
        BindingContext = this;
       
	}

   

    private async void ImgButton(object sender, EventArgs e)
    {
        var pic = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Please Select a Image" });

       
        if (pic != null)
        {
            using var stream = await pic.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imagedata = memoryStream.ToArray();
             customerDto.ImageBase64=Convert.ToBase64String(imagedata);

           
            memoryStream.Position = 0;
            PickedImageSource = ImageSource.FromStream(() => new MemoryStream(imagedata));
        }
  

    }

    private void AddressBtn(object sender, EventArgs e)
    {
        AddressList.Add(new AddressDto { Street = AddressStreetEntry.Text, City = AddressCityEntry.Text });

        AddressStreetEntry.Text = "";
        AddressCityEntry.Text = "";

    }



    private async void SaveBtn(object sender, EventArgs e)
    {
        customerDto.Name = NameEntry.Text;
        customerDto.DateofBirth = EnterDate.Date;
        customerDto.IsRegular = IsRegular.IsChecked;
        customerDto.MobileNo = EnterMobile.Text;
        customerDto.Email= EnterEmail.Text;
        customerDto.Addresses = AddressList.ToList();
        customerDto.AddressJson = JsonSerializer.Serialize(AddressList);
        customerDto.ImageUrl = customerDto.ImageBase64;
        try
        {
            JsonSerializerOptions _serializerOptions;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,

            };
            HttpClient _client = new HttpClient();

            _client.BaseAddress = new Uri( DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5199");
            string json = System.Text.Json.JsonSerializer.Serialize<CustomerDto>(customerDto, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _client.PostAsync("api/customer", content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Add Customer Successfully", "Ok");
                await Navigation.PushAsync(new CustomerListPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to add Customer", "Ok");
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }


    }
}