using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheIslandPostManager.Models;

public partial class Order : ObservableObject
{
    [ObservableProperty] private string downloadURL;

    [Display(Name = "Customer ID", AutoGenerateField = false)]
    public string CustomerID { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public string Email { get; set; }

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Date/Time", AutoGenerateField = false)]
    public DateTime DateTime { get; set; }
    //public List<Address> CC { get; set; }
    [ObservableProperty] private List<IImage> approvedImages = new();
    [ObservableProperty] private List<IImage> approvedPrints = new();

    [ObservableProperty] private bool isFinalized;
}
