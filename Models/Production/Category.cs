using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("Categories", Schema = "Production")]
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
