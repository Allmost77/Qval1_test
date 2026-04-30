using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace TestWPF.Models
{
 
    
        [Table("Товар")]
   public class Товар
        {
            public int ТоварID { get; set; }
            public string Артикул { get; set; } = "";
            public string Наименование_товара { get; set; } = "";
            public string Описание_товара { get; set; } = "";
            public decimal Цена { get; set; }
            public int Кол_во_на_складе { get; set; }
            public int Действующая_скидка { get; set; }
            public int? ПоставщикID { get; set; }
            public int? ПроизводительID { get; set; }
            public int? КатегорияТовараID { get; set; }
            public int? ЕдиницаИзмеренияID { get; set; }
            public string? Фото { get; set; }

        [NotMapped]
        public string ImagePath
        {
            get
            {
                var filename = string.IsNullOrWhiteSpace(Фото) ? "picture.png" : Фото;
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", filename);
            }
        }
 
        public Поставщик? Поставщик { get; set; }
        public Manufacturer? Производитель { get; set; }
        public Category? КатегорияТовара { get; set; }
        public Unit? ЕдиницаИзмерения { get; set; }

        [NotMapped]
        public bool HasDiscount => Действующая_скидка > 0;

        [NotMapped]
        public decimal FinalPrice => Цена * (100 - Действующая_скидка) / 100;

        [NotMapped]
        public bool OutOfStock => Кол_во_на_складе == 0;

        [NotMapped]
        public bool BigDiscount => Действующая_скидка > 15;
    }
    
}
