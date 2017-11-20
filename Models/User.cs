using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileCRM.Models
{
    public class User
    {
        // ID пользователя
        public int Id { get; set; }
        // Логин позьзователя
        public string Login { get; set; }
        // Имя пользователя
        public string Name { get; set; }
        // Фамилия пользователя
        public string Surname { get; set; }
        // Отчество пользователя
        public string Patronymic { get; set; }
        // Пароль пользователя
        public string Password { get; set; }
        // Роль пользователя
        public string Role { get; set; }
        // Фото пользователя
        public string Photo { get; set; }
    }
}