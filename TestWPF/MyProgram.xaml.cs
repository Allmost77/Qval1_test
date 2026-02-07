using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TestWPF.files;
using TestWPF.Models;

namespace TestWPF
{
    public partial class MyProgram : Window
    {
        public ObservableCollection<Product> Products { get; } = new();
        private List<Product> _all = new();

        public MyProgram(string role, string name, string surname, string patronymic)
        {
            InitializeComponent();
            UserName.Content = $"{surname} {name} {patronymic}";
            DataContext = this;
            LoadSuppliers();
            LoadProducts();
        }

        void LoadSuppliers()
        {
            supplierCombo.Items.Clear();
            supplierCombo.Items.Add("Все");
            var seen = new HashSet<string>();
            foreach (var p in ProductRepository.GetProducts()) { var s = p.Supplier?.Trim() ?? ""; if (seen.Add(s)) supplierCombo.Items.Add(s); }
            supplierCombo.SelectedIndex = 0;
        }

        void LoadProducts() { _all = ProductRepository.GetProducts(); ApplyFilters(); }

        void ApplyFilters()
        {
            Products.Clear();
            var supplier = supplierCombo.SelectedItem?.ToString() ?? "Все";
            var search = SearchTextBox.Text.Trim();
            foreach (var p in _all)
            {
                if (supplier != "Все" && p.Supplier != supplier) continue;
                if (!string.IsNullOrWhiteSpace(search) && !Match(p, search)) continue;
                Products.Add(p);
            }
        }

        static bool Match(Product p, string s) => Contains(p.Name, s) || Contains(p.Description, s) || Contains(p.Category, s) || Contains(p.Manufacturer, s) || Contains(p.Supplier, s) || Contains(p.Unit, s);
        static bool Contains(string? t, string s) => !string.IsNullOrWhiteSpace(t) && t.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0;

        private void Button_Click(object sender, RoutedEventArgs e) { new MainWindow().Show(); Close(); }
        private void supplierCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is not Product sel) { MessageBox.Show("Выберите товар."); return; }
            if (MessageBox.Show($"Удалить {sel.Name}?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { ProductRepository.DeleteProduct(sel.Id); LoadProducts(); MessageBox.Show("Удалено."); } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Add_card(object sender, RoutedEventArgs e) { new Add().ShowDialog(); LoadProducts(); LoadSuppliers(); }

        private void Edit_card(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is not Product sel) { MessageBox.Show("Выберите товар."); return; }
            var w = new Edit(); w.SetProduct(sel); w.ShowDialog();
            LoadProducts(); LoadSuppliers();
        }
    }
}
