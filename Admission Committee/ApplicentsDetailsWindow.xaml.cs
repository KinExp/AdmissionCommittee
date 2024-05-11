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

namespace Admission_Committee
{
    /// <summary>
    /// Логика взаимодействия для ApplicentsDetailsWindow.xaml
    /// </summary>
    public partial class ApplicentsDetailsWindow : Window
    {
        DataBase dataBase = new DataBase();
        List<Page> pages;
        int index;

        public ApplicentsDetailsWindow()
        {
            InitializeComponent();
            pages = new List<Page>();
            index = 0;

            pages.Add(new SchoolCertificatePage());
            pages.Add(new PersonalDataFirstPage());
            pages.Add(new PersonalDataSecondPage());
            applicentFrame.Content = pages[index];
        }

        private void BackPage_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0)
            {
                index--;
                applicentFrame.Content = pages[index];
                nextPage.Content = "Далее";
            }
            if (index == 0)
                backPage.Visibility = Visibility.Hidden;
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (index < pages.Count - 1)
            {
                index++;
                applicentFrame.Content = pages[index];
                backPage.Visibility = Visibility.Visible;
            }
            if (index == pages.Count - 1)
                nextPage.Content = "Готово";
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
