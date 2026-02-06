using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestWPF.files;
using TestWPF.Models;

namespace TestWPF
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public string _connectionString = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";

        public Add()
        {
            InitializeComponent();
            LoadSupliers();
            LoadUnit();
            LoadCategory();
            LoadManufacturer();
        }


        private void LoadSupliers()
        {
            SupplierTextBox.Items.Clear();
            SupplierTextBox.Items.Add("Все");
            var uniqueSuppliers = new HashSet<string>();
            foreach (Product product in ProductRepository.GetProducts())
            {
                var s = product.Supplier?.Trim();
                if (uniqueSuppliers.Add(s))
                {
                    SupplierTextBox.Items.Add(s);
                }
            }
            SupplierTextBox.SelectedIndex = 0;
        }
        private void LoadUnit()
        {
            UnitTextBox.Items.Clear();
            UnitTextBox.Items.Add("Все");
            var uniqueUnits = new HashSet<string>();
            foreach (Product product in ProductRepository.GetProducts())
            {
                var s = product.Unit?.Trim();
                if (uniqueUnits.Add(s))
                {
                    UnitTextBox.Items.Add(s);
                }
            }
            UnitTextBox.SelectedIndex = 0;
        }
        private void LoadCategory()
        {
            CategoryTextBox.Items.Clear();
            CategoryTextBox.Items.Add("Все");
            var uniqueCategories = new HashSet<string>();
            foreach (Product product in ProductRepository.GetProducts())
            {
                var s = product.Category?.Trim();
                if (uniqueCategories.Add(s))
                {
                    CategoryTextBox.Items.Add(s);
                }
            }
            CategoryTextBox.SelectedIndex = 0;
        }
        private void LoadManufacturer()
        {
            ManufacturerTextBox.Items.Clear();
            ManufacturerTextBox.Items.Add("Все");
            var uniqueManufacturers = new HashSet<string>();
            foreach (Product product in ProductRepository.GetProducts())
            {
                var s = product.Manufacturer?.Trim();
                if (uniqueManufacturers.Add(s))
                {
                    ManufacturerTextBox.Items.Add(s);
                }
            }
            ManufacturerTextBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public static int AddProduct(
    string article,
    string name,
    string description,
    decimal price,
    int quantity,
    int discount,
    string supplier,
    string manufacturer,
    string category,
    string unit,
    string? imagePath = null)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

           
            using var cmd = new SqlCommand(@"
INSERT INTO dbo.Tovar
(Артикул, Наименование_товара, Описание_товара, Цена, Кол_во_на_складе, Действующая_скидка,
 Поставщик, Производитель, Категория_товара, Единица_измерения, Фото)
VALUES
(@Article, @Name, @Description, @Price, @Quantity, @Discount,
 @Supplier, @Manufacturer, @Category, @Unit, @ImagePath);

SELECT CAST(SCOPE_IDENTITY() AS int);
", conn);

            cmd.Parameters.AddWithValue("@Article", article);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Discount", discount);
            cmd.Parameters.AddWithValue("@Supplier", supplier);
            cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
            cmd.Parameters.AddWithValue("@Category", category);
            cmd.Parameters.AddWithValue("@Unit", unit);
            cmd.Parameters.AddWithValue("@ImagePath", (object?)imagePath ?? DBNull.Value);

            return (int)cmd.ExecuteScalar(); 
        }

    }
}
