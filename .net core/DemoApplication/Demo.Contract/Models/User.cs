namespace Demo.Contract.Models
{
    public class User
    {
        private Address _address;

        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public Address Address { get => _address; set => _address = value; }
    }
}
