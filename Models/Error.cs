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
        public DateTime Date { get; set; }
        // Название ошибки
        public string Message { get; set; }
        // Текст ошибки
        public string StackTrace { get; set; }
        // Пользователь, при котором возникла ошибка
        public string User { get; set; }
        // Комментарий пользователя
        public string UserComment { get; set; }
        // Статус ошибки
        public string Status { get; set; }
    }
}