using Client.DTOs;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;

namespace Client;

public partial class UpdateCustomer : ContentPage
{
    private DTOs.CustomerDto customerDto;
    private ImageSource pickedImageSource;
    public ObservableCollection<AddressDto> AddressList { get; set; } = new ObservableCollection<AddressDto>();
    public ImageSource PickedImageSource
    {
        get => pickedImageSource;
        set
        {
            pickedImageSource = value;
            OnPropertyChanged();
        }
    }
    public UpdateCustomer(int customerId)
	{
		InitializeComponent();
        BindingContext = this;
        _ = LoadCustomerData(customerId);
    }

    private async Task LoadCustomerData(int customerId)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5199");

            HttpResponseMessage response = await client.GetAsync($"api/customer/{customerId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                customerDto = JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (customerDto != null)
                {
                    if (!string.IsNullOrEmpty(customerDto.ImageUrl))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(customerDto.ImageUrl);
                            PickedImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error: Invalid base64 string: {ex.Message}");
                            await DisplayAlert("Error", "Invalid image data.", "OK");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading image: {ex.Message}");
                            await DisplayAlert("Error", "Error loading image.", "OK");
                        }
                    }
                    AddressList = new ObservableCollection<AddressDto>(customerDto.Addresses);

                    // Set the ListView's ItemsSource to the ObservableCollection
                    AddressesListView.ItemsSource = AddressList;
                    NameEntry.Text = customerDto.Name;
                    EnterDate.Date = customerDto.DateofBirth;
                    IsRegular.IsChecked = customerDto.IsRegular;
                    EnterMobile.Text = customerDto.MobileNo;
                    EnterEmail.Text = customerDto.Email;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to deserialize customer data.", "OK");
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to load customer data.", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during data load: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while loading data.", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnUploadImageBtn(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Please select a picture" });
        if (result != null)
        {
            using var stream = await result.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imageData = memoryStream.ToArray();
            customerDto.ImageBase64 = Convert.ToBase64String(imageData);
            PickedImageSource = ImageSource.FromFile(result.FullPath);
        }

    }

  
    private async void OnAddAddressClicked(object sender, EventArgs e)
    {
        AddressList.Add(new AddressDto
        { Street = StreetEntry.Text, City = CityEntry.Text });
        StreetEntry.Text = "";
        CityEntry.Text = "";

    }

    private async void OnUpdateStudentClicked(object sender, EventArgs e)
    {
        if (customerDto == null)
        {
            await DisplayAlert("Error", "Customer data can not be loaded.", "OK");
            return;
        }

        customerDto.Name = NameEntry.Text;
        customerDto.DateofBirth =EnterDate.Date;
        customerDto.MobileNo = EnterMobile.Text;
        customerDto.Email = EnterEmail.Text;
        customerDto.IsRegular = IsRegular.IsChecked;
        customerDto.Addresses = AddressList.ToList(); 
        customerDto.AddressJson = JsonSerializer.Serialize(AddressList);
        customerDto.ImageUrl = customerDto.ImageBase64;

        try
        {
            JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5199" : "http://localhost:5199");

            string json = JsonSerializer.Serialize(customerDto, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            
            HttpResponseMessage response = await _client.PutAsync($"api/customer/{customerDto.Id}", content); 

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Student updated successfully.", "OK");
              
                await Navigation.PushAsync(new CustomerListPage());
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Update failed: {response.StatusCode}, {errorContent}");
                await DisplayAlert("Error", $"Failed to update student: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update error: {ex.Message}");
            await DisplayAlert("Error", "An error occurred during update.", "OK");
        }
    }

    private void OnRemoveAddressClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is AddressDto address)
        {
            AddressList.Remove(address);
        }

    }
}