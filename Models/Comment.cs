using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class Comment
    {
        // ID комментария
        public int Id { get; set; }
        // Дата 
        [Required]
        public DateTime Date { get; set; }
        // ФИО пользователя
        [Required]
        public string User { get; set; }
        // Комментарий
        [Required]
        public string UserComment { get; set; }
        // Кредитное дело
        public int? CreditId { get; set; }
        public Credit Credit { get; set; }
    }
}