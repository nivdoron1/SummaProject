namespace SummaProject1Vue
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Dob { get; set; }
        public string? Photo { get; set; }
    }
}