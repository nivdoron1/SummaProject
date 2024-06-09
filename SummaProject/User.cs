namespace SummaProject1Vue
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public byte[]? Photo { get; set; }

        public User(string username, string email, string birthDate, byte[]? photo)
        {
            Username = username;
            Email = email;
            BirthDate = birthDate;
            Photo = photo;
        }
    }
}
