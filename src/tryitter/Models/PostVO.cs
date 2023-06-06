namespace tryitter.Models
{
    public class PostVO
    {
        public int PostId { get; set; }

        public string Content { get; set; }

        public DateTime CreatAt { get; set; }

        public DateTime UpdatetAt { get; set; }

        public byte[]? Image { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }
    }
}
