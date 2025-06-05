using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("roles")]
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }
    }
}