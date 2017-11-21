using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileCRM.Models
{
    public class Credit
    {
        // ID кредитного дела 
        public int Id { get; set; }
        // Имя клиента 
        [Required(ErrorMessage = "Введите имя клиента")]
        public string Name { get; set; }
        // Фамилия клиента 
        [Required(ErrorMessage = "Введите фамилию клиента")]
        public string Surname { get; set; }
        // Отчество клиента 
        public string Patronymic { get; set; }
        // Ссылка на фото клиента
        public string Photo { get; set; }
        // Почта клиента 
        [Required(ErrorMessage = "Введите почту клиента")]
        [DataType(DataType.EmailAddress)]
        [Remote("ValidateEmail", "Home")]
        public string Email { get; set; }
        // Источник обращения 
        public string Source { get; set; }
        // Номер телефона клиента 
        [Required(ErrorMessage = "Введите номер телефона клиента")]
        [Remote("ValidatePhone", "Home")]
        public string PhoneNumber { get; set; }
        // Дата рождения клиента 
        [Required(ErrorMessage = "Введите дату рождения клиента")]
        [DataType(DataType.Date)]
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