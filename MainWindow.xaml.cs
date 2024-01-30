using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankSystem_VP
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-BK1T0PD\\SQLEXPRESS;Initial Catalog=Bank;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Введите логин")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.Text = "Введите логин";
                LoginTextBox.Foreground = Brushes.Gray;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "Введите пароль")
            {
                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = Brushes.Black;
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PasswordTextBox.Text = "Введите пароль";
                PasswordTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            // Проверка наличия введенного логина и пароля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            // Попытка авторизации пользователя
            bool isAuthenticated = AuthenticateUser(login, password);

            if (isAuthenticated)
            {
                // Действия при успешной авторизации
                MessageBox.Show("Вы успешно авторизованы.");
                Profit profit = new Profit();
                profit.Show();

            }
            else
            {
                // Действия при неуспешной авторизации
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private bool AuthenticateUser(string login, string password)
        {
            bool isAuthenticated = false;

            // Создание соединения с базой данных
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Подготовка SQL-запроса для проверки совпадения логина и пароля
                    string query = "SELECT COUNT(*) FROM Users WHERE Login=@Login AND Password=@Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    // Выполнение запроса и проверка результата
                    int result = (int)command.ExecuteScalar();
                    isAuthenticated = (result > 0);
                }
                catch (Exception ex)
                {
                    // Обработка ошибок подключения к базе данных
                    MessageBox.Show("Ошибка авторизации: " + ex.Message);
                }
            }

            return isAuthenticated;
        }
    }
}
