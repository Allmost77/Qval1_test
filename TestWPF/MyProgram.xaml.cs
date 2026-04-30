using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TestWPF.Data;
using TestWPF.Models;

namespace TestWPF
{
    public partial class MyProgram : Window
    {

        private List<Товар> _all = new();

        public MyProgram(string role, string name, string surname, string patronymic)
        {
            InitializeComponent();
            UserName.Content = $"{surname} {name} {patronymic}";
            DataContext = this;
            LoadSuppliers();
            LoadProducts();
            
        }

      

        void LoadProducts()
        {
            using var db = new AppDbContext();

            _all = db.Товары
                .Include(t => t.Поставщик)
                .Include(t => t.Производитель)
                .Include(t => t.КатегорияТовара)
                .Include(t => t.ЕдиницаИзмерения)
                .ToList();

            ApplyFilters();
        }
        void ApplyFilters()
        {
            var items = _all.AsEnumerable();

            var search = SearchBox.Text?.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                items = items.Where(t =>
                    t.Наименование_товара.ToLower().Contains(search) ||
                    t.Описание_товара.ToLower().Contains(search) ||
                    t.Артикул.ToLower().Contains(search) ||
                    t.Поставщик!.Имя.ToLower().Contains(search) ||
                    t.Производитель!.Имя.ToLower().Contains(search) ||
                    t.КатегорияТовара!.Имя.ToLower().Contains(search)
                );
            }

            if (supplierCombo.SelectedItem is Поставщик supplier && supplier.ID != 0)
            {
                items = items.Where(t => t.ПоставщикID == supplier.ID);
            }

            ProductsList.ItemsSource = items.ToList();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void SupplierBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        void LoadSuppliers()
        {
            using var db = new AppDbContext();

            var suppliers = db.Поставщики
                .OrderBy(s => s.Имя)
                .ToList();

            suppliers.Insert(0, new Поставщик
            {
                ID = 0,
                Имя = "Все поставщики"
            });

            supplierCombo.ItemsSource = suppliers;
            supplierCombo.DisplayMemberPath = "Имя";
            supplierCombo.SelectedValuePath = "ID";
            supplierCombo.SelectedIndex = 0;
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e) { new MainWindow().Show(); Close(); }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is not Товар selected)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить товар \"{selected.Наименование_товара}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using var db = new AppDbContext();

                var product = db.Товары.FirstOrDefault(t => t.ТоварID == selected.ТоварID);

                if (product == null)
                {
                    MessageBox.Show("Товар не найден");
                    return;
                }

                db.Товары.Remove(product);
                db.SaveChanges();

                LoadProducts();
            }
            catch
            {
                MessageBox.Show(
                    "Нельзя удалить товар, который присутствует в заказе.",
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Add_card(object sender, RoutedEventArgs e) { new ProductWindow().ShowDialog(); LoadProducts(); }


        private void Edit_card(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is not Товар selected)
            { MessageBox.Show("Выберите товар."); return; }
            new ProductWindow(selected.ТоварID).ShowDialog();
            LoadProducts();
        }
    }
}
