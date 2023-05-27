namespace tryitter.Models
{
    public class Studant
    {
        public int StudantId { get; set; }
        public string StudantName { get; set; }
        public string Email { get; set; }
        public string CurrentModel { get; set; }
        public string Password { get; set; }
        public Post Post { get; set; }
    }
}