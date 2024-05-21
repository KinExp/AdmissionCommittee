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
using static System.Net.Mime.MediaTypeNames;

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
            SearchTextBox.Text = "Выберите столбец поиска";

            RefreshDataGrids();
        }

        private void RefreshDataGrids()
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add();
            dataSet.Tables.Add();
            dataSet.Tables.Add();

            string sqlQuery = "SELECT Аб.*, Ат.Средний_балл FROM Абитуриенты Аб " +
                                        "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата";

            dataBase.OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());
            adapter.Fill(dataSet.Tables[0]);

            sqlQuery = "SELECT Код_заявления, Фамилия, Имя, Отчество, З.Серия_паспорта, З.Номер_паспорта, З.Код_специальности, Название_специальности, З.Уровень_образования, " +
                                        "З.Вариант_обучения, З.Форма_обучения FROM Заявления_на_поступление З " +
                                        "INNER JOIN Абитуриенты А ON А.Серия_паспорта = З.Серия_паспорта AND А.Номер_паспорта = З.Номер_паспорта " +
                                        "INNER JOIN Специальности С ON С.Код_специальности = З.Код_специальности AND С.Уровень_образования = З.Уровень_образования " +
                                        "AND С.Вариант_обучения = З.Вариант_обучения AND С.Форма_обучения = З.Форма_обучения";

            adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());
            adapter.Fill(dataSet.Tables[1]);

            sqlQuery = "SELECT * FROM Специальности";

            adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());
            adapter.Fill(dataSet.Tables[2]);
            dataBase.CloseConnection();

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

            foreach (DataRow row in dataSet.Tables[1].Rows)
            {
                List<string> items = new List<string>();

                var cells = row.ItemArray;
                foreach (object cell in cells)
                    items.Add(cell.ToString());

                DataRow newRow = dtStatements.NewRow();
                newRow.ItemArray = items.ToArray();
                dtStatements.Rows.Add(newRow);
            }

            foreach (DataRow row in dataSet.Tables[2].Rows)
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
            RefreshDataGrids();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicentsDetailsWindow aplWin = new ApplicentsDetailsWindow();

            if (aplWin.ShowDialog() == true)
                RefreshDataGrids();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exit = true;
        }

        private void CloseImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (exit)
            {
                CustomMessageBox question = new CustomMessageBox("Подтверждение", "Вы действительно хотите выйти?", "Подтверждение выхода");
                question.ShowDialog();
            }

        }

        private void CollapseImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FullScreenImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Maximized;
            MaxButton.Visibility = Visibility.Collapsed;
            MinButton.Visibility = Visibility.Visible;
        }

        private void MinScreenImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Normal;
            MaxButton.Visibility = Visibility.Visible;
            MinButton.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (applicantsDataGrid.SelectedItem != null || statementsDataGrid.SelectedItem != null)
            {

                CustomMessageBox deleteWindow = new CustomMessageBox("Удаление", "Вы действительно хотите удалить запись?", "Удаление");

                if (deleteWindow.ShowDialog() == true)
                {
                    DataRowView row;

                    if (tabControl.SelectedIndex == 0)
                        row = (DataRowView)applicantsDataGrid.SelectedItem;
                    else
                    {
                        row = (DataRowView)statementsDataGrid.SelectedItem;

                        string sqlQuery = "SELECT Аб.*, Ат.Средний_балл FROM Абитуриенты Аб " +
                                          "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата " +
                                          $"WHERE Серия_паспорта = {row[4]} AND Номер_паспорта = {row[5]}";

                        dataBase.OpenConnection();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());

                        DataSet ds = new DataSet();

                        adapter.Fill(ds);

                        row = ds.Tables[0].DefaultView[0];
                    }

                    string sqlQueryApplicant = $"DELETE FROM Решения_приёмной_комиссии WHERE Код_заявления = (SELECT Код_заявления FROM Заявления_на_поступление WHERE Серия_паспорта = {row[0]} AND Номер_паспорта = {row[1]})";

                    dataBase.OpenConnection();
                    SqlCommand commandApplicant = new SqlCommand(sqlQueryApplicant, dataBase.GetConnection());
                    commandApplicant.ExecuteNonQuery();

                    sqlQueryApplicant = $"DELETE FROM Заявления_на_поступление WHERE Серия_паспорта = {row[0]} AND Номер_паспорта = {row[1]}";

                    commandApplicant = new SqlCommand(sqlQueryApplicant, dataBase.GetConnection());
                    commandApplicant.ExecuteNonQuery();

                    sqlQueryApplicant = $"DELETE FROM Абитуриенты WHERE Серия_паспорта = {row[0]} AND Номер_паспорта = {row[1]}";

                    commandApplicant = new SqlCommand(sqlQueryApplicant, dataBase.GetConnection());
                    commandApplicant.ExecuteNonQuery();

                    sqlQueryApplicant = $"DELETE FROM Аттестаты WHERE Серия_аттестата = {row[11]} AND Номер_аттестата = {row[12]}";

                    commandApplicant = new SqlCommand(sqlQueryApplicant, dataBase.GetConnection());
                    commandApplicant.ExecuteNonQuery();
                    dataBase.CloseConnection();

                    RefreshDataGrids();
                }
            }
            else
            {
                CustomMessageBox error = new CustomMessageBox("Ошибка", "Строка не выбрана", "Ошибка");
                error.ShowDialog();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (applicantsDataGrid.SelectedItem != null || statementsDataGrid.SelectedItem != null)
            {
                DataRowView row;
                string sqlQuery;
                SqlDataAdapter adapter;
                DataSet ds;

                if (tabControl.SelectedIndex == 0)
                {
                    row = (DataRowView)applicantsDataGrid.SelectedItem;
                    sqlQuery = "SELECT Аб.*, Ат.Количество_оценок_пять, АТ.Количество_оценок_четыре, Ат.Количество_оценок_три, Ат.Изучаемый_иностранный_язык, " +
                                  "З.Код_специальности, З.Уровень_образования, З.Вариант_обучения, З.Форма_обучения FROM Абитуриенты Аб " +
                                  "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата " +
                                  "INNER JOIN Заявления_на_поступление З ON З.Серия_паспорта = Аб.Серия_паспорта AND З.Номер_паспорта = Аб.Номер_паспорта " +
                                   $"WHERE Аб.Серия_паспорта = {row[0]} AND Аб.Номер_паспорта = {row[1]}";

                    dataBase.OpenConnection();
                    adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());

                    ds = new DataSet();

                    adapter.Fill(ds);

                    row = ds.Tables[0].DefaultView[0];
                }
                else
                {
                    row = (DataRowView)statementsDataGrid.SelectedItem;
                    sqlQuery = "SELECT Аб.*, Ат.Количество_оценок_пять, АТ.Количество_оценок_четыре, Ат.Количество_оценок_три, Ат.Изучаемый_иностранный_язык, " +
                                  "З.Код_специальности, З.Уровень_образования, З.Вариант_обучения, З.Форма_обучения FROM Абитуриенты Аб " +
                                  "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата " +
                                  "INNER JOIN Заявления_на_поступление З ON З.Серия_паспорта = Аб.Серия_паспорта AND З.Номер_паспорта = Аб.Номер_паспорта " +
                                   $"WHERE Аб.Серия_паспорта = {row[4]} AND Аб.Номер_паспорта = {row[5]}";

                    dataBase.OpenConnection();
                    adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());

                    ds = new DataSet();

                    adapter.Fill(ds);

                    row = ds.Tables[0].DefaultView[0];
                }

                ApplicentsDetailsWindow aplWin = new ApplicentsDetailsWindow(row);

                if (aplWin.ShowDialog() == true)
                    RefreshDataGrids();
            }
            else
            {
                CustomMessageBox error = new CustomMessageBox("Ошибка", "Строка не выбрана", "Ошибка");
                error.ShowDialog();
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    SearchComboBox.Items.Clear();
                    for (int i = 0; i < applicantsDataGrid.Columns.Count; i++)
                    {
                        if (applicantsDataGrid.Columns[i].Visibility == Visibility.Visible)
                        {
                            SearchComboBox.Items.Add(applicantsDataGrid.Columns[i].Header);
                        }
                    }
                    break;
                case 1:
                    SearchComboBox.Items.Clear();
                    for (int i = 0; i < statementsDataGrid.Columns.Count; i++)
                    {
                        if (statementsDataGrid.Columns[i].Visibility == Visibility.Visible)
                        {
                            SearchComboBox.Items.Add(statementsDataGrid.Columns[i].Header);
                        }
                    }
                    break;
                case 2:
                    SearchComboBox.Items.Clear();
                    for (int i = 0; i < specialtiesDataGrid.Columns.Count; i++)
                    {
                        if (specialtiesDataGrid.Columns[i].Visibility == Visibility.Visible)
                        {
                            SearchComboBox.Items.Add(specialtiesDataGrid.Columns[i].Header);
                        }
                    }
                    addButton.Visibility = Visibility.Hidden;
                    changeButton.Visibility = Visibility.Hidden;
                    deleteButton.Visibility = Visibility.Hidden;
                    break;
                default:
                    SearchComboBox.Items.Clear();
                    addButton.Visibility = Visibility.Visible;
                    changeButton.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchComboBox.SelectedIndex != -1
                && SearchTextBox.Text != string.Empty)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        string text = SearchTextBox.Text;
                        if (SearchComboBox.Text == "Пол")
                        {
                            if (SearchTextBox.Text.ToUpper() == "Ж")
                                text = "0";
                            else if (SearchTextBox.Text.ToUpper() == "М")
                                text = "1";
                        }

                        string sqlQueryApplicents = "SELECT Аб.*, Ат.Средний_балл FROM Абитуриенты Аб " +
                                                      "INNER JOIN Аттестаты Ат ON Ат.Серия_аттестата = Аб.Серия_аттестата AND Ат.Номер_аттестата = Аб.Номер_аттестата " +
                                                      $"WHERE {SearchComboBox.Text.Replace(' ', '_')} LIKE \'{text}%\'";

                        dataBase.OpenConnection();

                        DataSet dataSetApplicents = new DataSet();
                        SqlDataAdapter adapterApplicents = new SqlDataAdapter(sqlQueryApplicents, dataBase.GetConnection());

                        dataSetApplicents.Tables.Add();
                        adapterApplicents.Fill(dataSetApplicents.Tables[0]);

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

                        foreach (DataRow row in dataSetApplicents.Tables[0].Rows)
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
                        applicantsDataGrid.ItemsSource = dtApplicents.DefaultView;

                        break;
                    case 1:
                        string table = "";
                        if (SearchComboBox.SelectedIndex >= 4)
                            table = "З.";

                        string sqlQueryStatements = "SELECT Код_заявления, Фамилия, Имя, Отчество, З.Серия_паспорта, З.Номер_паспорта, З.Код_специальности, Название_специальности, З.Уровень_образования, " +
                                                    "З.Вариант_обучения, З.Форма_обучения FROM Заявления_на_поступление З " +
                                                    "INNER JOIN Абитуриенты А ON А.Серия_паспорта = З.Серия_паспорта AND А.Номер_паспорта = З.Номер_паспорта " +
                                                    "INNER JOIN Специальности С ON С.Код_специальности = З.Код_специальности AND С.Уровень_образования = З.Уровень_образования " +
                                                    "AND С.Вариант_обучения = З.Вариант_обучения AND С.Форма_обучения = З.Форма_обучения " +
                                                    $"WHERE {table + SearchComboBox.Text.Replace(' ', '_')} LIKE \'{SearchTextBox.Text}%\'";

                        dataBase.OpenConnection();

                        DataSet dataSetStatements = new DataSet();
                        SqlDataAdapter adapterStatements = new SqlDataAdapter(sqlQueryStatements, dataBase.GetConnection());

                        dataSetStatements.Tables.Add();
                        adapterStatements.Fill(dataSetStatements.Tables[0]);

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
                        foreach (DataRow row in dataSetStatements.Tables[0].Rows)
                        {
                            List<string> items = new List<string>();

                            var cells = row.ItemArray;
                            foreach (object cell in cells)
                                items.Add(cell.ToString());

                            DataRow newRow = dtStatements.NewRow();
                            newRow.ItemArray = items.ToArray();
                            dtStatements.Rows.Add(newRow);
                        }
                        statementsDataGrid.ItemsSource = dtStatements.DefaultView;

                        break;
                    case 2:
                        string sqlQuerySpec = "SELECT * FROM Специальности " +
                                              $"WHERE {SearchComboBox.Text.Replace(' ', '_')} LIKE \'{SearchTextBox.Text}%\'";

                        dataBase.OpenConnection();

                        DataSet dataSetSpec = new DataSet();
                        SqlDataAdapter adapterSpecialties = new SqlDataAdapter(sqlQuerySpec, dataBase.GetConnection());

                        dataSetSpec.Tables.Add();
                        adapterSpecialties.Fill(dataSetSpec.Tables[0]);

                        DataTable dtSpecialties = new DataTable("Специальности");
                        dtSpecialties.Columns.Add("ID");
                        dtSpecialties.Columns.Add("Level");
                        dtSpecialties.Columns.Add("NameSpecialty");
                        dtSpecialties.Columns.Add("Option");
                        dtSpecialties.Columns.Add("Form");
                        dtSpecialties.Columns.Add("AmountPlaces");
                        dtSpecialties.Columns.Add("Cost");
                        dtSpecialties.Columns.Add("Time");

                        foreach (DataRow row in dataSetSpec.Tables[0].Rows)
                        {
                            List<string> items = new List<string>();

                            var cells = row.ItemArray;
                            foreach (object cell in cells)
                                items.Add(cell.ToString());

                            DataRow newRow = dtSpecialties.NewRow();
                            newRow.ItemArray = items.ToArray();
                            dtSpecialties.Rows.Add(newRow);
                        }
                        specialtiesDataGrid.ItemsSource = dtSpecialties.DefaultView;

                        break;
                }
            }
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                RefreshDataGrids();
            }

        }

        private void SearchComboBox_TextChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchComboBox.SelectedIndex != -1)
            {
                SearchTextBox.IsEnabled = true;
                SearchTextBox.Text = string.Empty;
            }
            else
            {
                SearchTextBox.IsEnabled = false;
                SearchTextBox.Text = "Выберите столбец поиска";
            }
        }
    }
}
