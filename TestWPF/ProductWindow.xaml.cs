using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestWPF.Data;
using TestWPF.Models;
using System.Drawing.Imaging;

using Path = System.IO.Path;
namespace TestWPF
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private readonly AppDbContext db = new();
        private readonly Товар product = new();
        private readonly bool isEditMode;
        public ProductWindow(int? productId = null)
        {
            InitializeComponent();
            if (productId == null)
            {
                product = new Товар();
                isEditMode = false;
                Title = "Добавление товара";
            }
            else
            {
                product = db.Товары.First(t => t.ТоварID == productId);
                isEditMode = true;
                Title = "Редактирование товара";
            }

            DataContext = product;

            SupplierBox.ItemsSource = db.Поставщики.ToList();
            SupplierBox.DisplayMemberPath = "Имя";
            SupplierBox.SelectedValuePath = "ID";

            ManufacturerBox.ItemsSource = db.Производители.ToList();
            ManufacturerBox.DisplayMemberPath = "Имя";
            ManufacturerBox.SelectedValuePath = "ID";

            CategoryBox.ItemsSource = db.Категории.ToList();
            CategoryBox.DisplayMemberPath = "Имя";
            CategoryBox.SelectedValuePath = "ID";

            UnitBox.ItemsSource = db.ЕдиницыИзмерения.ToList();
            UnitBox.DisplayMemberPath = "Имя";
            UnitBox.SelectedValuePath = "ID";
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dialog.ShowDialog() != true)
                return;

            var picturesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures");
            Directory.CreateDirectory(picturesDir);

            var fileName = Guid.NewGuid() + ".jpg";
            var destPath = Path.Combine(picturesDir, fileName);


            product.Фото = fileName;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(product.Артикул))
            {
                MessageBox.Show("Введите артикул");
                return;
            }

            if (string.IsNullOrWhiteSpace(product.Наименование_товара))
            {
                MessageBox.Show("Введите наименование товара");
                return;
            }

            if (product.Цена < 0 || product.Кол_во_на_складе < 0 || product.Действующая_скидка < 0 || product.Действующая_скидка > 100)
            {
                MessageBox.Show("Проверьте цену, количество и скидку");
                return;
            }
            if (!isEditMode)
            {
                db.Товары.Add(product);
            }

            db.SaveChanges();

            Close();
        }
    }
}
