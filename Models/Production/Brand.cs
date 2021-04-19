using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("brands", Schema = "Production")]
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
