using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestWPF.Models
{
    [Table("Пользователи")]
    public class User
    {
        [Key]
        public int ПользовательID { get; set; }
        public int РольID { get; set; }
        public string Имя { get; set; } = "";
        public string Фамилия { get; set; } = "";
        public string Отчество { get; set; } = "";
        public string Логин { get; set; } = "";
        public string Пароль { get; set; } = "";
       

    }
}
