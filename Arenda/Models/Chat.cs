using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("chats")]
    public class Chat
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("property_id")]
        public int? PropertyId { get; set; }

        [Column("guest_id")]
        public int GuestId { get; set; }

        [Column("manager_id")] // Новое поле для связи с менеджером
        public int ManagerId { get; set; }

        // Связи
        public virtual ResidentialProperty Property { get; set; }
        public virtual User Guest { get; set; }
        public virtual User Manager { get; set; } // Навигационное свойство для менеджера
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}   