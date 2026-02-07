using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TestWPF.files;
using TestWPF.Helpers;
using TestWPF.Models;

namespace TestWPF
{
    public partial class Edit : Window
    {
        private int _id;

        public Edit()
        {
            InitializeComponent();
            ProductFormHelper.FillCombo(SupplierTextBox, ReferenceRepository.GetSuppliers());
            ProductFormHelper.FillCombo(ManufacturerTextBox, ReferenceRepository.GetManufacturers());
            ProductFormHelper.FillCombo(CategoryTextBox, ReferenceRepository.GetCategories());
            ProductFormHelper.FillCombo(UnitTextBox, ReferenceRepository.GetUnits());
        }

        public void SetProduct(Product p)
        {
            _id = p.Id;
            ArticleTextBox.Text = p.Article ?? "";
            NameTextBox.Text = p.Name ?? "";
            DescriptionTextBox.Text = p.Description ?? "";
            PriceTextBox.Text = p.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            QuantityTextBox.Text = p.Quantity.ToString();
            DiscountTextBox.Text = p.Discount.ToString();
            SelectByName(SupplierTextBox, p.Supplier);
            SelectByName(ManufacturerTextBox, p.Manufacturer);
            SelectByName(CategoryTextBox, p.Category);
            SelectByName(UnitTextBox, p.Unit);
        }

        static void SelectByName(ComboBox combo, string? name)
        {
            if (string.IsNullOrWhiteSpace(name) || combo.ItemsSource is not IEnumerable<IdNameItem> list) return;
            var item = list.FirstOrDefault(x => string.Equals(x.Name?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (item != null) combo.SelectedValue = item.Id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var (ok, article, name, desc, price, qty, discount, sId, mId, cId, uId, err) = ProductFormHelper.ReadForm(
                ArticleTextBox, NameTextBox, DescriptionTextBox, PriceTextBox, QuantityTextBox, DiscountTextBox,
                SupplierTextBox, ManufacturerTextBox, CategoryTextBox, UnitTextBox);
            if (!ok) { MessageBox.Show(err); return; }
            try
            {
                ProductRepository.UpdateProduct(_id, article, name, desc, price, qty, discount, sId, mId, cId, uId, null);
                MessageBox.Show("Сохранено."); Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
