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
        bool exit;

        public MainWindow()
        {
            InitializeComponent();

            dataBase = new DataBase();
            exit = false;

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            string sqlQueryApplicents = "SELECT Аб.*, Ат.Средний_балл_аттестата FROM Абитуриенты Аб " +
                                        "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата";

            string sqlQueryStatements = "SELECT Код_заявления, Фамилия, Имя, Отчество, З.Серия_паспорта, З.Номер_паспорта, З.Код_специальности, Название_специальности, З.Уровень_образования, " +
                                        "З.Вариант_обучения, З.Форма_обучения FROM Заявления_на_поступление З " +
                                        "INNER JOIN Абитуриенты А ON А.Серия_паспорта = З.Серия_паспорта AND А.Номер_паспорта = З.Номер_паспорта " +
                                        "INNER JOIN Специальности С ON С.Код_специальности = З.Код_специальности AND С.Уровень_образования = З.Уровень_образования " +
                                        "AND С.Вариант_обучения = З.Вариант_обучения AND С.Форма_обучения = З.Форма_обучения";

            string sqlQuerySpecialties = "SELECT * FROM Специальности";

            dataBase.OpenConnection();

            SqlDataAdapter adapterApplicents = new SqlDataAdapter(sqlQueryApplicents, dataBase.GetConnection());
            SqlDataAdapter adapterStatements = new SqlDataAdapter(sqlQueryStatements, dataBase.GetConnection());
            SqlDataAdapter adapterSpecialties = new SqlDataAdapter(sqlQuerySpecialties, dataBase.GetConnection());

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add();
            dataSet.Tables.Add();
            dataSet.Tables.Add();

            adapterApplicents.Fill(dataSet.Tables[0]);
            adapterStatements.Fill(dataSet.Tables[1]);
            adapterSpecialties.Fill(dataSet.Tables[2]);
            
            DataTable dtApplicents = new DataTable("Абитуриенты");
            dtApplicents.Columns.Add("PassportSeries");
            dtApplicents.Columns.Add("PassportNumber");
            dtApplicents.Columns.Add("SecondName");
            dtApplicents.Columns.Add("FirstName");
            dtApplicents.Columns.Add("ThirdName");
            dtApplicents.Columns.Add("Gender");
            dtApplicents.Columns.Add("Birthday");
            dtApplicents.Columns.Add("Address");
            dtApplicents.Columns.Add("Phone");
            dtApplicents.Columns.Add("School");
            dtApplicents.Columns.Add("YearGraduation");
            dtApplicents.Columns.Add("СertificateSiries");
            dtApplicents.Columns.Add("СertificateNumber");
            dtApplicents.Columns.Add("AvarageScore");


            DataTable dtStatements = new DataTable("Заявления");
            dtStatements.Columns.Add("ID");
            dtStatements.Columns.Add("SecondName");
            dtStatements.Columns.Add("FirstName");
            dtStatements.Columns.Add("ThirdName");
            dtStatements.Columns.Add("PassportSeries");
            dtStatements.Columns.Add("PassportNumber");
            dtStatements.Columns.Add("IdSpecialty");
            dtStatements.Columns.Add("NameSpecialty");
            dtStatements.Columns.Add("Level");
            dtStatements.Columns.Add("Option");
            dtStatements.Columns.Add("Form");

            DataTable dtSpecialties = new DataTable("Специальности");
            dtSpecialties.Columns.Add("ID");
            dtSpecialties.Columns.Add("Level");
            dtSpecialties.Columns.Add("NameSpecialty");
            dtSpecialties.Columns.Add("Option");
            dtSpecialties.Columns.Add("Form");
            dtSpecialties.Columns.Add("AmountPlaces");
            dtSpecialties.Columns.Add("Cost");
            dtSpecialties.Columns.Add("Time");

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                List<string> items = new List<string>();

                var cells = row.ItemArray;
                foreach (object cell in cells)
                    items.Add(cell.ToString());

                if (items[5] == "False")
                    items[5] = "Ж";
                else
                    items[5] = "М";

                items[6] = items[6].Substring(0, 10);

                DataRow newRow = dtApplicents.NewRow();
                newRow.ItemArray = items.ToArray();
                dtApplicents.Rows.Add(newRow);
            }

            foreach(DataRow row in dataSet.Tables[1].Rows)
            {
                List<string> items = new List<string>();

                var cells = row.ItemArray;
                foreach (object cell in cells)
                    items.Add(cell.ToString());

                DataRow newRow = dtStatements.NewRow();
                newRow.ItemArray = items.ToArray();
                dtStatements.Rows.Add(newRow);
            }

            foreach(DataRow row in dataSet.Tables[2].Rows)
            {
                List<string> items = new List<string>();

                var cells = row.ItemArray;
                foreach (object cell in cells)
                    items.Add(cell.ToString());

                DataRow newRow = dtSpecialties.NewRow();
                newRow.ItemArray = items.ToArray();
                dtSpecialties.Rows.Add(newRow);
            }

            applicantsDataGrid.ItemsSource = dtApplicents.DefaultView;
            statementsDataGrid.ItemsSource = dtStatements.DefaultView;
            specialtiesDataGrid.ItemsSource = dtSpecialties.DefaultView;
        }

        private void RefreshImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RefreshDataGrid();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicentsDetailsWindow aplWin = new ApplicentsDetailsWindow();
            aplWin.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseImage_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            exit = true;
        }

        private void CloseImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (exit)
                Close();
        }
    }
}
