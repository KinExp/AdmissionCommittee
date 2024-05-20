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
    /// Логика взаимодействия для ApplicentsDetailsWindow.xaml
    /// </summary>
    public partial class ApplicentsDetailsWindow : Window
    {
        DataBase dataBase = new DataBase();
        SchoolCertificatePage certificatePage = new SchoolCertificatePage();
        PersonalDataFirstPage personalFirstPage = new PersonalDataFirstPage();
        PersonalDataSecondPage personalSecondPage = new PersonalDataSecondPage();
        List<Page> pages;
        int index;
        DataTable specializations;

        public ApplicentsDetailsWindow(DataRowView dataRow = null)
        {
            InitializeComponent();
            pages = new List<Page>();
            index = 0;

            pages.Add(certificatePage);
            pages.Add(personalFirstPage);
            pages.Add(personalSecondPage);
            applicentFrame.Content = pages[index];

            string sqlQuery = "SELECT DISTINCT Код_специальности, Название_специальности FROM Специальности";

            dataBase.OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());

            specializations = new DataTable();

            adapter.Fill(specializations);
            dataBase.CloseConnection();

            foreach (DataRow row in specializations.Rows)
                personalSecondPage.nameSpecializationComboBox.Items.Add(row.ItemArray[1].ToString());

            personalSecondPage.levelEducationComboBox.Items.Add("Основное общее образование");
            personalSecondPage.levelEducationComboBox.Items.Add("Среднее общее образование");

            personalSecondPage.optionEducationComboBox.Items.Add("Очный");
            personalSecondPage.optionEducationComboBox.Items.Add("Заочный");

            personalSecondPage.formEducationComboBox.Items.Add("Бюджетная");
            personalSecondPage.formEducationComboBox.Items.Add("Платная");
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
                if (index == pages.Count - 1)
                    nextPage.Content = "Готово";
            }
            else
            {
                if (certificatePage.numberOfSchoolCertificateTextBox.Text != string.Empty
                    && certificatePage.seriesOfSchoolCertificateTextBox.Text != string.Empty
                    && certificatePage.countOfFivesTextBox.Text != string.Empty
                    && certificatePage.countOfFoursTextBox.Text != string.Empty
                    && certificatePage.countOfTriplesTextBox.Text != string.Empty
                    && personalFirstPage.numberOfPassportTextBox.Text != string.Empty
                    && personalFirstPage.seriesOfPassportTextBox.Text != string.Empty
                    && personalFirstPage.lastNameTextBox.Text != string.Empty
                    && personalFirstPage.firstNameTextBox.Text != string.Empty
                    && personalFirstPage.middleNameTextBox.Text != string.Empty
                    && personalFirstPage.birthdayDatePicker.Text != string.Empty
                    && (personalFirstPage.maleButton.IsChecked.Value || personalFirstPage.femaleButton.IsChecked.Value)
                    && personalSecondPage.addressTextBox.Text != string.Empty
                    && personalSecondPage.phoneTextBox.Text != string.Empty
                    && personalSecondPage.schoolTextBox.Text != string.Empty
                    && personalSecondPage.yearGraduationTextBox.Text != string.Empty
                    && personalSecondPage.nameSpecializationComboBox.Text != string.Empty
                    && personalSecondPage.levelEducationComboBox.Text != string.Empty
                    && personalSecondPage.optionEducationComboBox.Text != string.Empty
                    && personalSecondPage.formEducationComboBox.Text != string.Empty)
                {
                    string sqlQueryCertificate = "INSERT INTO Аттестаты (Серия_аттестата, Номер_аттестата, Количество_оценок_пять, " +
                                                 "Количество_оценок_четыре, Количество_оценок_три, Изучаемый_иностранный_язык) VALUES " +
                                                 $"({certificatePage.seriesOfSchoolCertificateTextBox.Text}, " +
                                                 $"{certificatePage.numberOfSchoolCertificateTextBox.Text}, " +
                                                 $"{certificatePage.countOfFivesTextBox.Text}, " +
                                                 $"{certificatePage.countOfFoursTextBox.Text}, " +
                                                 $"{certificatePage.countOfTriplesTextBox.Text}, " +
                                                 $"\'{certificatePage.targetLanguageTextBox.Text}\');";

                    int gender = 0;
                    if (personalFirstPage.maleButton.IsChecked.Value)
                        gender = 1;

                    string sqlQueryApplicent = "INSERT INTO Абитуриенты (Серия_паспорта, Номер_паспорта, Фамилия, Имя, Отчество, Пол, " +
                                               "Дата_рождения, Адрес_по_прописке, Телефон, Название_учебного_заведения, Год_окончания_учебного_заведения, " +
                                               "Серия_аттестата, Номер_аттестата) VALUES " +
                                               $"({personalFirstPage.seriesOfPassportTextBox.Text}, " +
                                               $"{personalFirstPage.numberOfPassportTextBox.Text}, " +
                                               $"\'{personalFirstPage.lastNameTextBox.Text}\', " +
                                               $"\'{personalFirstPage.firstNameTextBox.Text}\', " +
                                               $"\'{personalFirstPage.middleNameTextBox.Text}\', " +
                                               $"{gender}, " +
                                               $"\'{personalFirstPage.birthdayDatePicker.Text}\', " +
                                               $"\'{personalSecondPage.addressTextBox.Text}\', " +
                                               $"\'{personalSecondPage.phoneTextBox.Text}\', " +
                                               $"\'{personalSecondPage.schoolTextBox.Text}\', " +
                                               $"{personalSecondPage.yearGraduationTextBox.Text}, " +
                                               $"{certificatePage.seriesOfSchoolCertificateTextBox.Text}, " +
                                               $"{certificatePage.numberOfSchoolCertificateTextBox.Text})";

                    string IdSpecialization = "";
                    foreach (DataRow row in specializations.Rows)
                    {
                        if (row.ItemArray[1].ToString() == personalSecondPage.nameSpecializationComboBox.Text)
                        {
                            IdSpecialization = row.ItemArray[0].ToString();
                            break;
                        }
                    }

                    string sqlQueryStatements = "INSERT INTO Заявления_на_поступление (Серия_паспорта, Номер_паспорта, Код_специальности, " +
                                                "Уровень_образования, Вариант_обучения, Форма_обучения) VALUES " +
                                                $"({certificatePage.seriesOfSchoolCertificateTextBox.Text}, " +
                                                $"{certificatePage.numberOfSchoolCertificateTextBox.Text}, " +
                                                $"\'{IdSpecialization}\', " +
                                                $"\'{personalSecondPage.levelEducationComboBox.Text}\', " +
                                                $"\'{personalSecondPage.optionEducationComboBox.Text}\', " +
                                                $"\'{personalSecondPage.formEducationComboBox.Text}\');";

                    dataBase.OpenConnection();

                    SqlCommand command = new SqlCommand(sqlQueryCertificate, dataBase.GetConnection());
                    command.ExecuteNonQuery();

                    command = new SqlCommand(sqlQueryApplicent, dataBase.GetConnection());
                    command.ExecuteNonQuery();

                    command = new SqlCommand(sqlQueryStatements, dataBase.GetConnection());
                    command.ExecuteNonQuery();

                    dataBase.CloseConnection();

                    Close();
                }
                else
                {
                    ConfirmDeleteWindow error = new ConfirmDeleteWindow("Ошибка", "Не все поля заполнены!", "Ошибка");
                    error.ShowDialog();
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
