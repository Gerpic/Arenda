using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("property_categories")]
    public class PropertyCategory
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("category_name")]
        public string CategoryName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<ResidentialProperty> Properties { get; set; } = new List<ResidentialProperty>();
    }
}