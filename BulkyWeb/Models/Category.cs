using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // if the name is Id or CategoryId then you dont need the data annotation for key. Key is not needed here.
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        [Range(1, 100)]
        public int DisplayOrder { get; set; }
    }
}
