using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("cities")]
    public class City
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public ICollection<ResidentialProperty> Properties { get; set; } = new List<ResidentialProperty>();
    }
}