using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TestWPF.Data;

namespace TestWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void To_guest_Click(object sender, RoutedEventArgs e)
        {
            Open("Гость", "Гостевой режим", "", "");
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var pass = PasswordTextBox.Text.Trim();

            using var db = new AppDbContext();

            var user = db.Пользователи
                .Include(u => u.Роль)
                .FirstOrDefault(u => u.Логин == login && u.Пароль == pass);

            if (user == null)
            {
                MessageBox.Show(
                    "Неверный логин или пароль",
                    "Ошибка авторизации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Open(
                user.Роль?.Имя ?? "",
                user.Имя,
                user.Фамилия,
                user.Отчество);
        }

        private void Open(string role, string name, string surname, string patronymic)
        {
            new MyProgram(role, name, surname, patronymic).Show();
            Close();
        }
    }
}