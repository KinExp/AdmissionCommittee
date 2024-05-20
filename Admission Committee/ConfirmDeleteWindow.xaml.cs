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

namespace Admission_Committee
{
    /// <summary>
    /// Логика взаимодействия для ConfirmDeleteWindow.xaml
    /// </summary>
    public partial class ConfirmDeleteWindow : Window
    {
        public ConfirmDeleteWindow(string label, string text, string type)
        {
            InitializeComponent();

            windowLabel.Content = label;
            windowText.Content = text;

            if (type == "Ошибка")
            {
                confirmButton.Visibility = Visibility.Hidden;
                cancelButton.Content = "ОК";
            }                
        }

        private void cancelDelete_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void confirmDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
