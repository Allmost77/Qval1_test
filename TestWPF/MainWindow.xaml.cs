using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";
        private string login;
        private string password;
        private string role;
        private string name;
        private string surname;
        private string patronymic;
        public MainWindow()
        {
            InitializeComponent();
            CreateSqlConnection();
        }

        private void To_guest_Click(object sender, RoutedEventArgs e)
        {
            MyProgram mainWindow = new MyProgram(role, name, surname, patronymic);
            mainWindow.Show();
            this.Close();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(
                "SELECT Роль_Сотрудника, Имя, Фамилия, Отчество " +
                "FROM dbo.user_import " +
                "WHERE Логин = @login AND Пароль = @password", connection))
            {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        return;
                    }

                    
                    string role = reader["Роль_Сотрудника"].ToString();
                    string name = reader["Имя"].ToString();
                    string surname = reader["Фамилия"].ToString();
                    string patronymic = reader["Отчество"].ToString();

                    // Передача данных в новое окно
                    MyProgram main = new MyProgram(role, name, surname, patronymic);
                    main.Show();
                    this.Close();
                }
            }
           
        }


        private void CreateSqlConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show($"Победа");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database connection error: {ex.Message}");
                }
            }
        }
    }
}