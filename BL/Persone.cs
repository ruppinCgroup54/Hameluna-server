namespace hameluna_server.BL
{
    public class Persone
    {
        public Persone()
        {
            PhoneNumber = "";
            Email = "";
            FirstName = "";
            LastName = "";
        }

        public Persone(string phoneNumber, string firstName, string lastName, string email)
        {
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        

    }
}
