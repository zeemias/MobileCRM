using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MobileCRM.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}