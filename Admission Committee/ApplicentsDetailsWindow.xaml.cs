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
            applicentFrame.Content = pages[index];
        }

        private void BackPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
