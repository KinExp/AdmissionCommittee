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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Admission_Committee
{
    public partial class MainWindow : Window
    {
        DataBase dataBase;
        DataSet dataSet;
        List<string> tables;
        public MainWindow()
        {
            InitializeComponent();

            dataBase = new DataBase();
            dataSet = new DataSet();
            tables = new List<string>();
            tables.Add("Абитуриенты");

            //RefreshDataGrid(applicantsDataGrid, tables[0]);

        }

        private void RefreshDataGrid(DataGrid dg, string table)
        {
            string sql = "SELECT Серия_паспорта, Номер_паспорта, Фамилия, Имя, Отчество, Пол FROM " + table;

            dataBase.OpenConnection();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, dataBase.GetConnection());

            adapter.Fill(dataSet, table);
            
            DataTable dataTableOrders = new DataTable("Абитуриенты");
            dataTableOrders.Columns.Add("PassportSeries");
            dataTableOrders.Columns.Add("PassportNumber");
            dataTableOrders.Columns.Add("SecondName");
            dataTableOrders.Columns.Add("FirstName");
            dataTableOrders.Columns.Add("ThirdName");
            dataTableOrders.Columns.Add("Gender");

            foreach (DataRow row in dataSet.Tables[table].Rows)
            {
                List<string> items = new List<string>();

                var cells = row.ItemArray;
                foreach (object cell in cells)
                    items.Add(cell.ToString());

                if (items[5] == "False")
                    items[5] = "Ж";
                else
                    items[5] = "М";

                DataRow newRow = dataTableOrders.NewRow();
                newRow.ItemArray = items.ToArray();
                dataTableOrders.Rows.Add(newRow);
            }

            dg.ItemsSource = dataTableOrders.DefaultView;
        }

        private void CloseImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicentsDetailsWindow aplWin = new ApplicentsDetailsWindow();
            aplWin.ShowDialog();
        }
    }
}
