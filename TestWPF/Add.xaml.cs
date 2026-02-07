using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using TestWPF.files;
using TestWPF.Helpers;
using TestWPF.Models;

namespace TestWPF
{
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
            ProductFormHelper.FillCombo(SupplierTextBox, ReferenceRepository.GetSuppliers());
            ProductFormHelper.FillCombo(ManufacturerTextBox, ReferenceRepository.GetManufacturers());
            ProductFormHelper.FillCombo(CategoryTextBox, ReferenceRepository.GetCategories());
            ProductFormHelper.FillCombo(UnitTextBox, ReferenceRepository.GetUnits());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var (ok, article, name, desc, price, qty, discount, sId, mId, cId, uId, err) = ProductFormHelper.ReadForm(
                ArticleTextBox, NameTextBox, DescriptionTextBox, PriceTextBox, QuantityTextBox, DiscountTextBox,
                SupplierTextBox, ManufacturerTextBox, CategoryTextBox, UnitTextBox);
            if (!ok) { MessageBox.Show(err); return; }
            try
            {
                var newId = AddProduct(article, name, desc, price, qty, discount, sId, mId, cId, uId, null);
                MessageBox.Show($"Товар добавлен. ID: {newId}");
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public static int AddProduct(string article, string name, string description, decimal price, int quantity, int discount,
            int? supplierId, int? manufacturerId, int? categoryId, int? unitId, string? imagePath)
        {
            using var conn = new SqlConnection(ProductRepository.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(@"
INSERT INTO dbo.Tovar ([Артикул],[Наименование_товара],[Единица_измерения],[Цена],[Поставщик],[Производитель],[Категория_товара],[Действующая_скидка],[Кол_во_на_складе],[Описание_товара],[Фото])
VALUES (@Article,@Name,@UnitId,@Price,@SupplierId,@ManufacturerId,@CategoryId,@Discount,@Quantity,@Description,@ImagePath);
SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
            cmd.Parameters.AddWithValue("@Article", article);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@UnitId", (object?)unitId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@SupplierId", (object?)supplierId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ManufacturerId", (object?)manufacturerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)categoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@ImagePath", (object?)imagePath ?? DBNull.Value);
            return (int)cmd.ExecuteScalar();
        }
    }
}
