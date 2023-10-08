using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // if the name is Id or CategoryId then you dont need the data annotation for key. Key is not needed here.
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
