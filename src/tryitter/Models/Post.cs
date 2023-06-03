using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tryitter.Models;
public class Post
{
    [Key]
    public int PostId { get; set; }

    [MaxLength(300, ErrorMessage = "Content precisa ter no máximo 300 caracteres")]
    public string Content { get; set; } = default!;

    public DateTime CreatAt { get; set; }

    public DateTime UpdatetAt { get; set; }

    public byte[]? Image { get; set; }

    [ForeignKey("StudentId")]
    public int StudentId { get; set; }

    public Student? Student { get; set; }
}


// namespace tryitter.Models
// {
//     public class Post
//     {
//         public int PostId { get; set; }
//         public string PostText { get; set; }
//         public byte[] Image { get; set; }
//     }
// }