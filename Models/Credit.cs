using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class Credit
    {
        // ID кредитного дела 
        public int Id { get; set; }
        // Имя клиента 
        [Required]
        public string Name { get; set; }
        // Фамилия клиента 
        [Required]
        public string Surname { get; set; }
        // Отчество клиента 
        public string Patronymic { get; set; }
        // Ссылка на фото клиента
        [Required]
        public string Photo { get; set; }
        // Почта клиента 
        [Required]
        public string Email { get; set; }
        // Источник обращения 
        [Required]
        public string Source { get; set; }
        // Номер телефона клиента 
        [Required]
        public long PhoneNumber { get; set; }
        // Дата рождения клиента 
        [Required]
        public DateTime Birthday { get; set; }
        // Место работы клиента 
        public string Work { get; set; }
        // Логин пользователя, который добавил кредитное дело
        public string UserLogin { get; set; }
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