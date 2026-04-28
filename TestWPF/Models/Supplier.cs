using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TestWPF.Models
{
    [Table("Поставщик")]
    public class  Поставщик
    {
        [Key]
        public int ID { get; set; }
        public string Имя { get; set; } = "";
    }
}
