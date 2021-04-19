using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("categories", Schema = "Production")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
