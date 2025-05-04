using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Server.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public bool IsRegular { get; set; }
        public string? ImageUrl { get; set; }
        public string ImageBase64 { get; set; }
        public string AddressJson { get; set; }




    }
}
