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
        public DateTime Date { get; set; }
        // ФИО пользователя
        public string User { get; set; }
        // Комментарий
        [Required(ErrorMessage = "Введите комментарий")]
        public string UserComment { get; set; }
        // Кредитное дело
        public int? CreditId { get; set; }
        public Credit Credit { get; set; }
    }
}