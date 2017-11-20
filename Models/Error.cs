using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class Error
    {
        // ID ошибки
        public int Id { get; set; }
        // Дата возникновения ошибки
        [Required]
        public DateTime Date { get; set; }
        // Название ошибки
        [Required]
        public string Message { get; set; }
        // Текст ошибки
        [Required]
        public string StackTrace { get; set; }
        // Пользователь, при котором возникла ошибка
        [Required]
        public string User { get; set; }
        // Комментарий пользователя
        public string UserComment { get; set; }
        // Статус ошибки
        [Required]
        public string Status { get; set; }
    }
}