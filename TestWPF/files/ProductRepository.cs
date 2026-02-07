using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Controls;
using TestWPF.Models;

namespace TestWPF.files
{
    static class Db
    {
        public static string S(IDataRecord r, string col) =>

            r[col] == DBNull.Value ? "" : r[col].ToString() ?? "";

        public static int I(IDataRecord r, string col) =>

           r[col] == DBNull.Value ? 0 : Convert.ToInt32(r[col]);

        public static decimal D(IDataRecord r, string col) =>

           r[col] == DBNull.Value ? 0 : Convert.ToDecimal(r[col]);
    }
    public static class ProductRepository
    {

        internal static string ConnectionString = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";

        public static List<Product> GetProducts()
        {
            var list = new List<Product>();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(@"Select * from dbo.View_1", conn);
            using var r = cmd.ExecuteReader();

            while (r.Read())
            {
                var v = r["Quantity"];
                System.Diagnostics.Debug.WriteLine(v == DBNull.Value ? "Quantity is NULL" : $"Quantity={v}");

                var img = Db.S(r, "ImagePath");
                list.Add(new Product
                {
                    Id = Db.I(r, "Id"),
                    Article = Db.S(r, "Article"),
                    Name = Db.S(r, "Name"),
                    Category = Db.S(r, "Category"),
                    Description = Db.S(r, "Description"),
                    Manufacturer = Db.S(r, "Manufacturer"),
                    Supplier = Db.S(r, "Supplier"),
                    Unit = Db.S(r, "Unit"),
                    Price = Db.I(r, "Price"),
                    Quantity = Db.I(r, "Quantity"),
                    Discount = Db.I(r, "Discount"),
                    ImagePath = string.IsNullOrWhiteSpace(img) ? "picture.png" : img
                });
            }
            return list;
        }
        public static int DeleteProduct(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM dbo.Tovar WHERE Номер = @Номер", conn);
            cmd.Parameters.AddWithValue("@Номер", id);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>Обновляет карточку товара по Номер (Id). Передаются Id справочников, не названия.</summary>
        public static int UpdateProduct(int id,
            string article,
            string name,
            string description,
            decimal price,
            int quantity,
            int discount,
            int? supplierId,
            int? manufacturerId,
            int? categoryId,
            int? unitId,
            string? imagePath)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(@"
UPDATE dbo.Tovar SET
  [Артикул] = @Article,
  [Наименование_товара] = @Name,
  [Описание_товара] = @Description,
  [Цена] = @Price,
  [Кол_во_на_складе] = @Quantity,
  [Действующая_скидка] = @Discount,
  [Поставщик] = @SupplierId,
  [Производитель] = @ManufacturerId,
  [Категория_товара] = @CategoryId,
  [Единица_измерения] = @UnitId,
  [Фото] = @ImagePath
WHERE Номер = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Article", article);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.Parameters.AddWithValue("@SupplierId", (object?)supplierId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ManufacturerId", (object?)manufacturerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)categoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UnitId", (object?)unitId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ImagePath", (object?)imagePath ?? DBNull.Value);

            return cmd.ExecuteNonQuery();
        }
    }
}
