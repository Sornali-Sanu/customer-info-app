using Client.DTOs;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Client;

public partial class CustomerListPage : ContentPage
{
    public ObservableCollection<CustomerDto> CustomerList { get; set; }=new ObservableCollection<CustomerDto>();


    public CustomerListPage()
	{
		InitializeComponent();
        BindingContext = this;
        _ = LodeStudent();

    }

    private async Task LodeStudent()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri
             (DeviceInfo.Platform == DevicePlatform.Android ?
                 "http://10.0.2.2:5199" : "http://localhost:5199"
             )
        };

        var res = await client.GetAsync("api/customer");

        if (res.IsSuccessStatusCode)
        {
            var customer = await res.Content.ReadAsStringAsync();

            CustomerList = JsonSerializer.Deserialize<ObservableCollection<CustomerDto>>
                (customer, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (CustomerList != null)
            {
                CustomerListView.ItemsSource = CustomerList;
            }
            else
            {
                await DisplayAlert("eror", "CustomerList can not be Loaded", "Ok");
            }
        }
    }

    private void AddCustomerBtn(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddCustomerPage());
    }

    private void updateCustomerBtn(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CustomerDto customerDto)
        {
            Navigation.PushAsync(new UpdateCustomer(customerDto.Id));
        }
    }

    private async void DeleteBtn(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CustomerDto customerDto)
        {
            bool result = await DisplayAlert("Delete", $"Delete this student {customerDto.Name}?", "Yes", "No");
            if (result)
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri
                    (DeviceInfo.Platform == DevicePlatform.Android ?
                        "http://10.0.2.2:5199" : "http://localhost:5199"
                    )
                };

                var res = await client.DeleteAsync($"api/customer/{customerDto.Id}");
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("Succes", "Deleted SuccessFully", "Ok");
                    await Navigation.PushAsync(new CustomerListPage());

                }
                else
                {
                    await DisplayAlert("eror", "Error Occour", "Ok");
                }

            }
        }
    }
}