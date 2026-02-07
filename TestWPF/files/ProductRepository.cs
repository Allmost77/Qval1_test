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
    }
}
