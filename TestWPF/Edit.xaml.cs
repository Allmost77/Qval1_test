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
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private string _article;
        private string _name;
        private string _description;
        private string _supplier;
        private string _manufacturer;
        private string _category;
        private string _unit;
        private string _discount;
        private string _price;
        private string _quantity;
        private string ConnectionString = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";

        public Edit()
        {
            InitializeComponent();
            FillComboFromReference(SupplierTextBox, ReferenceRepository.GetSuppliers());
            FillComboFromReference(ManufacturerTextBox, ReferenceRepository.GetManufacturers());
            FillComboFromReference(CategoryTextBox, ReferenceRepository.GetCategories());
            FillComboFromReference(UnitTextBox, ReferenceRepository.GetUnits());
        }


        public Edit(string article, string name, string description, string supplier, string manufacturer, string category, string unit, decimal price, int quantity, int discount)
        {
            InitializeComponent();
            ArticleTextBox.Text = article;
            NameTextBox.Text = name;
            DescriptionTextBox.Text = description;
            SupplierTextBox.Text = supplier;
            ManufacturerTextBox.Text = manufacturer;
            CategoryTextBox.Text = category;
            UnitTextBox.Text = unit;
            PriceTextBox.Text = price.ToString("F2");
            QuantityTextBox.Text = quantity.ToString();
            DiscountTextBox.Text = discount.ToString();

            _article = article;
                _name = name;
                _description = description;
                _supplier = supplier;
                _manufacturer = manufacturer;
                _category = category;
                _unit = unit;
                _price = price.ToString("F2");
                _quantity = quantity.ToString();
                _discount = discount.ToString();


        }
        private static void FillComboFromReference(ComboBox combo, List<IdNameItem> items)
        {
            var list = new List<IdNameItem> { new IdNameItem { Id = 0, Name = "— не выбрано —" } };
            list.AddRange(items);
            combo.ItemsSource = list;
            combo.DisplayMemberPath = "Name";
            combo.SelectedValuePath = "Id";
            combo.SelectedIndex = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_article))
            {
                MessageBox.Show("Введите артикул.", "Поле не заполнено", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(_name))
            {
                MessageBox.Show("Введите наименование товара.", "Поле не заполнено", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(PriceTextBox.Text?.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (число ≥ 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(QuantityTextBox.Text, out var quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество (целое число ≥ 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(DiscountTextBox.Text, out var discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Введите корректную скидку (целое число от 0 до 100).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int newId = EditProduct(_article, _name, _description, price, quantity, discount, _supplierid, _manufacturer, _category, _unit, null);
                MessageBox.Show($"Товар добавлен.\nID: {newId}\nНаименование: {_name}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int EditProduct(string article,
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
INSERT INTO dbo.Tovar
([Артикул], [Наименование_товара], [Единица_измерения], [Цена], [Поставщик], [Производитель], [Категория_товара], [Действующая_скидка], [Кол_во_на_складе], [Описание_товара], [Фото])
VALUES
(@Article, @Name, @UnitId, @Price, @SupplierId, @ManufacturerId, @CategoryId, @Discount, @Quantity, @Description, @ImagePath);

SELECT CAST(SCOPE_IDENTITY() AS int);
", conn);

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
    }
}
