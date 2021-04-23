using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models.Internal
{
    [Table("UserInfo", Schema = "Internal")]
    public class UserInfo
    {
        [Key]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string Password { get; set; }
    }
}
