using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("support_tickets")]
    public class SupportTicket
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("subject")]
        public string Subject { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("resolved_by")]
        public int? ResolvedBy { get; set; }

        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }

        // Добавьте это свойство!
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}