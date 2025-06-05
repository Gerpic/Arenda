using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("chat_messages")]
    public class Message
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("chat_id")]
        public int ChatId { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        // Связи
        public virtual Chat Chat { get; set; }
        public virtual User Sender { get; set; }
    }
}