using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using TestWPF.Models;

namespace TestWPF.files
{
    /// <summary>
    /// Справочники для комбобоксов в Add: отдаём Id + название, в БД при добавлении уходит только Id.
    /// Подставьте свои имена таблиц/колонок, если в БД иначе (Номер/Код, Наименование/Название).
    /// </summary>
    public static class ReferenceRepository
    {
        private static string ConnectionString => ProductRepository.ConnectionString;

        private static List<IdNameItem> Load(string sql, string nameColumn)
        {
            var list = new List<IdNameItem>();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new IdNameItem
                {
                    Id = Db.I(r, "Номер"),
                    Name = Db.S(r, nameColumn)
                });
            }
            return list;
        }

        // Имена таблиц/колонок — как в вашем представлении: Поставщик, Производительl, [Категория товара], Ед
        public static List<IdNameItem> GetSuppliers() => Load("SELECT Номер, Поставщик FROM dbo.Поставщик ORDER BY Поставщик", "Поставщик");
        public static List<IdNameItem> GetManufacturers() => Load("SELECT Номер, Производитель FROM dbo.Производительl ORDER BY Производитель", "Производитель");
        public static List<IdNameItem> GetCategories() => Load("SELECT Номер, Категория_товара FROM dbo.[Категория товара] ORDER BY Категория_товара", "Категория_товара");
        public static List<IdNameItem> GetUnits() => Load("SELECT Номер, Единица_измерения FROM dbo.Ед ORDER BY Единица_измерения", "Единица_измерения");
    }
}
