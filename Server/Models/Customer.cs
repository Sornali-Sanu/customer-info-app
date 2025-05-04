namespace Server.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public bool IsRegular { get; set; }
        public string? ImageUrl { get; set; }
        public List<Address> Addresses { get; set; }=new List<Address>();

    }
}
