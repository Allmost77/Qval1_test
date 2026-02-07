using System.Windows;
using Microsoft.Data.SqlClient;

namespace TestWPF
{
    public partial class MainWindow : Window
    {
        const string Conn = "Data Source=localhost\\SQLEXPRESS01;Database=MyBD;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0";

        public MainWindow()
        {
            InitializeComponent();
            try { using var c = new SqlConnection(Conn); c.Open(); } catch (SqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void To_guest_Click(object sender, RoutedEventArgs e) { Open("", "", "", ""); }
        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var pass = PasswordTextBox.Text.Trim();
            using var conn = new SqlConnection(Conn);
            using var cmd = new SqlCommand("SELECT Роль_Сотрудника, Имя, Фамилия, Отчество FROM dbo.user_import WHERE Логин=@l AND Пароль=@p", conn);
            cmd.Parameters.AddWithValue("@l", login);
            cmd.Parameters.AddWithValue("@p", pass);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (!r.Read()) { MessageBox.Show("Неверный логин или пароль"); return; }
            Open(r["Роль_Сотрудника"].ToString() ?? "", r["Имя"].ToString() ?? "", r["Фамилия"].ToString() ?? "", r["Отчество"].ToString() ?? "");
        }

        void Open(string role, string name, string surname, string patronymic) { new MyProgram(role, name, surname, patronymic).Show(); Close(); }
    }
}
