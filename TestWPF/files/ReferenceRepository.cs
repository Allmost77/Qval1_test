using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using TestWPF.Models;

namespace TestWPF.files
{
    public static class ReferenceRepository
    {
        static List<IdNameItem> Load(string sql, string nameCol)
        {
            var list = new List<IdNameItem>();
            using var conn = new SqlConnection(ProductRepository.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(new IdNameItem { Id = Db.I(r, "ТоварID"), Name = Db.S(r, nameCol) });
            return list;
        }
        public static List<IdNameItem> GetSuppliers() =>
    Load("SELECT ID AS ТоварID, Имя AS Поставщик FROM dbo.Поставщик ORDER BY Имя", "Поставщик");

        public static List<IdNameItem> GetManufacturers() =>
            Load("SELECT ID AS ТоварID, Имя AS Производитель FROM dbo.Производитель ORDER BY Имя", "Производитель");

        public static List<IdNameItem> GetCategories() =>
            Load("SELECT ID AS ТоварID, Имя AS Категория_товара FROM dbo.КатегорияТовара ORDER BY Имя", "Категория_товара");

        public static List<IdNameItem> GetUnits() =>
            Load("SELECT ID AS ТоварID, Имя AS ЕдиницаИзмерения FROM dbo.ЕдиницаИзмерения ORDER BY Имя", "ЕдиницаИзмерения");
    }
}
