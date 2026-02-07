using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TestWPF.files;
using TestWPF.Models;

namespace TestWPF
{
    public partial class Edit : Window
    {
        private int _productId;

        public Edit()
        {
            InitializeComponent();
            FillComboFromReference(SupplierTextBox, ReferenceRepository.GetSuppliers());
            FillComboFromReference(ManufacturerTextBox, ReferenceRepository.GetManufacturers());
            FillComboFromReference(CategoryTextBox, ReferenceRepository.GetCategories());
            FillComboFromReference(UnitTextBox, ReferenceRepository.GetUnits());
        }

        /// <summary>Подставляет данные выбранной карточки в форму. Комбобоксы выбираются по совпадению названия.</summary>
        public void SetProduct(Product p)
        {
            _productId = p.Id;
            ArticleTextBox.Text = p.Article ?? "";
            NameTextBox.Text = p.Name ?? "";
            DescriptionTextBox.Text = p.Description ?? "";
            PriceTextBox.Text = p.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            QuantityTextBox.Text = p.Quantity.ToString();
            DiscountTextBox.Text = p.Discount.ToString();

            SelectComboByName(SupplierTextBox, p.Supplier?.Trim());
            SelectComboByName(ManufacturerTextBox, p.Manufacturer?.Trim());
            SelectComboByName(CategoryTextBox, p.Category?.Trim());
            SelectComboByName(UnitTextBox, p.Unit?.Trim());
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

        private static void SelectComboByName(ComboBox combo, string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            if (combo.ItemsSource is not IEnumerable<IdNameItem> list) return;
            var item = list.FirstOrDefault(x => string.Equals(x.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                combo.SelectedValue = item.Id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var article = ArticleTextBox.Text?.Trim() ?? "";
            var name = NameTextBox.Text?.Trim() ?? "";
            var description = DescriptionTextBox.Text?.Trim() ?? "";
            int? IdOrNull(object? val) => val is int id && id > 0 ? id : null;
            var supplierId = IdOrNull(SupplierTextBox.SelectedValue);
            var manufacturerId = IdOrNull(ManufacturerTextBox.SelectedValue);
            var categoryId = IdOrNull(CategoryTextBox.SelectedValue);
            var unitId = IdOrNull(UnitTextBox.SelectedValue);

            if (string.IsNullOrWhiteSpace(article))
            {
                MessageBox.Show("Введите артикул.", "Поле не заполнено", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
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
                ProductRepository.UpdateProduct(_productId, article, name, description, price, quantity, discount, supplierId, manufacturerId, categoryId, unitId, null);
                MessageBox.Show("Изменения сохранены.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
