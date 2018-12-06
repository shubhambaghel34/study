namespace Demo.Contract.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int EntityType { get; set; }
        public int EntityId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string Street { get; set; }
        public int AddressType { get; set; }
    }
}
