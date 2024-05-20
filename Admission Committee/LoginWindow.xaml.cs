using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Admission_Committee
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        DataBase dataBase = new DataBase();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void loginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loginBox.Foreground = Brushes.Black;
            if (loginBox.Text == "Логин")
                loginBox.Text = "";
        }

        private void loginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text == "")
            {
                loginBox.Foreground = Brushes.Gray;
                loginBox.Text = "Логин";
            }
        }

        private void fakePasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            fakePasswordBox.Visibility = Visibility.Hidden;
            passwordBox.Focus();
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.ToString() == "")
                fakePasswordBox.Visibility= Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            string loginQuerry = $"select * from Авторизация " +
                                 $"where Логин = '{loginBox.Text}' " +
                                 $"and Пароль = '{passwordBox.Password}'";

            SqlDataAdapter adapter = new SqlDataAdapter(loginQuerry, dataBase.GetConnection());
            DataTable dataTable = new DataTable();
            
            adapter.Fill(dataTable);
            

            if (dataTable.Rows.Count == 1)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            */
            if (loginBox.Text == "admin" && passwordBox.Password == "admin")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                ConfirmDeleteWindow error = new ConfirmDeleteWindow("Ошибка", "Неверный логин или пароль!", "Ошибка");
                error.ShowDialog();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
