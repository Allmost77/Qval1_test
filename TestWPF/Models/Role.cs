using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestWPF.Models
{
    [Table("Роль")]
    public class Role
    {
        [Key]
        public string ID { get; set; }
        
        public string Имя { get; set; }
        
    }
}
