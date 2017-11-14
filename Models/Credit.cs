using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class Credit
    {
        // ID кредитного дела 
        public int Id { get; set; }
        // Имя клиента 
        public string Name { get; set; }
        // Фамилия клиента 
        public string Surname { get; set; }
        // Отчество клиента 
        public string Patronymic { get; set; }
        // Ссылка на фото клиента
        public string Photo { get; set; }
        // Почта клиента 
        public string Email { get; set; }
        // Источник обращения 
        public string Source { get; set; }
        // Номер телефона клиента 
        public long PhoneNumber { get; set; }
        // Дата рождения клиента 
        public DateTime Birthday { get; set; }
        // Место работы клиента 
        public string Work { get; set; }
        // История взаимоотношений
        public ICollection<Story> Stories { get; set; }
        // Комментарии
        public ICollection<Comment> Comments { get; set; }

        public Credit()
        {
            Stories = new List<Story>();
            Comments = new List<Comment>();
        }
    }
}