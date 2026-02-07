using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для MyProgram.xaml
    /// </summary>
    public partial class MyProgram : Window
    {

        private string connectionString = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";
        public ObservableCollection<Product> Products { get; private set; } = new();
        private List<Product> _allProducts = new();
        private string _name;
        private string _surname;
        private string _patronymic;
        private string _role;

        public MyProgram(string role, string name, string surname, string patronymic)
        {
            InitializeComponent();

            _role = role;
            UserName.Content = $"{surname} {name} {patronymic}";
            DataContext = this;
            LoadSupliers();
            LoadProducts();


        }

        private void LoadSupliers()
        {
            supplierCombo.Items.Clear();
            supplierCombo.Items.Add("Все");
            var uniqueSuppliers = new HashSet<string>();
            foreach (Product product in ProductRepository.GetProducts())
            {
                var s = product.Supplier?.Trim();
                if (uniqueSuppliers.Add(s)) {
                    supplierCombo.Items.Add(s);
                }
            }
            supplierCombo.SelectedIndex = 0;
        }

        private void LoadProducts() {

           _allProducts = ProductRepository.GetProducts();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            Products.Clear();
            string supplier = supplierCombo.SelectedItem?.ToString() ?? "Все";
            string searchText = SearchTextBox.Text.Trim();

            foreach (var p in _allProducts)
            { 
                if (supplier != "Все" && p.Supplier != supplier)
                    continue;  
                
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    if(!Contains(p.Name, searchText) &&
                            !Contains(p.Description, searchText) &&
                          !Contains(p.Category, searchText) &&
                          !Contains(p.Manufacturer, searchText) &&
                          !Contains(p.Supplier, searchText) &&
                          !Contains(p.Unit, searchText))
                          continue;
                    
                   
                }
                Products.Add(p);
            }
        }

        private bool Contains(string? text, string search) => 
            !string.IsNullOrWhiteSpace(text)  && text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void supplierCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (supplierCombo.SelectedItem == null)
                return;
            string supplier =  supplierCombo.SelectedItem.ToString()!;
            ApplyFilters();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            //if (_role != "Администратор")
            //{
            //    MessageBox.Show("У вас нет прав для удаления товара.");
            //    return;
            //}

          
            if (ProductsList.SelectedItem is not Product selected)
            {
                MessageBox.Show("Выберите товар (кликни по карточке).");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить товар?\n\nId: {selected.Id}\n{selected.Name}",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                int affected = ProductRepository.DeleteProduct(selected.Id);

                MessageBox.Show($"Запрос выполнен. Удалено строк: {affected}");

                LoadProducts(); // перезагрузка из БД + ApplyFilters
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении:\n" + ex.Message);
            }
        }

        private void Add_card(object sender, RoutedEventArgs e)
        {
            var addWindow = new Add();
            addWindow.ShowDialog();
            LoadProducts();
            LoadSupliers();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           Edit edit = new Edit();
            if (ProductsList.SelectedItem is not Product selected)
            {
                MessageBox.Show("Выберите товар (кликни по карточке).");
                return;
            }
            edit.SetProduct(selected);
            edit.ShowDialog();
            LoadProducts();
        }
    }
}
