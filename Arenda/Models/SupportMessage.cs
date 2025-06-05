using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("support_messages")]
    public class SupportMessage
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("ticket_id")]
        public int TicketId { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("sent_at")]
        public DateTime SentAt { get; set; }

        [Column("is_from_support")]
        public bool IsFromSupport { get; set; }
    }
}