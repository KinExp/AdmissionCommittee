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
        DataRowView personalInfo;
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

            if (dataRow != null)
            {
                personalInfo = dataRow;
                personalFirstPage.seriesOfPassportTextBox.Text = personalInfo[0].ToString();
                personalFirstPage.numberOfPassportTextBox.Text = personalInfo[1].ToString();
                personalFirstPage.lastNameTextBox.Text = personalInfo[2].ToString();
                personalFirstPage.firstNameTextBox.Text = personalInfo[3].ToString();
                personalFirstPage.middleNameTextBox.Text = personalInfo[4].ToString();
                if (personalInfo[5].ToString() == "True")
                    personalFirstPage.maleButton.IsChecked = true;
                else
                    personalFirstPage.femaleButton.IsChecked = true;
                personalFirstPage.birthdayDatePicker.Text = personalInfo[6].ToString();
                personalSecondPage.addressTextBox.Text = personalInfo[7].ToString();
                personalSecondPage.phoneTextBox.Text = personalInfo[8].ToString();
                personalSecondPage.schoolTextBox.Text = personalInfo[9].ToString();
                personalSecondPage.yearGraduationTextBox.Text = personalInfo[10].ToString();
                certificatePage.seriesOfSchoolCertificateTextBox.Text = personalInfo[11].ToString();
                certificatePage.numberOfSchoolCertificateTextBox.Text = personalInfo[12].ToString();
                certificatePage.countOfFivesTextBox.Text = personalInfo[13].ToString();
                certificatePage.countOfFoursTextBox.Text = personalInfo[14].ToString();
                certificatePage.countOfTriplesTextBox.Text = personalInfo[15].ToString();
                certificatePage.targetLanguageTextBox.Text = personalInfo[16].ToString();
                foreach (DataRow row in specializations.Rows)
                    if (row.ItemArray[0].ToString() == personalInfo[17].ToString())
                    {
                        personalSecondPage.nameSpecializationComboBox.Text = row.ItemArray[1].ToString();
                        break;
                    }
                personalSecondPage.levelEducationComboBox.Text = personalInfo[18].ToString();
                personalSecondPage.optionEducationComboBox.Text = personalInfo[19].ToString();
                personalSecondPage.formEducationComboBox.Text = personalInfo[20].ToString();
            }
            else
                personalInfo = null;
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
                    if (CheckExistenceSpecialty())
                    {
                        if (personalInfo == null)
                        {
                            AddNewStatement();

                            CustomMessageBox message = new CustomMessageBox("Успех", "Заявление добавлено!", true);
                            message.ShowDialog();
                        }
                        else
                        {
                            ChangeStatement();

                            CustomMessageBox message = new CustomMessageBox("Успех", "Заявление изменено!", true);
                            message.ShowDialog();
                        }

                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        CustomMessageBox error = new CustomMessageBox("Ошибка", "Нет такой специальности!", true);
                        error.ShowDialog();
                    }
                }
                else
                {
                    CustomMessageBox error = new CustomMessageBox("Ошибка", "Не все поля заполнены!", true);
                    error.ShowDialog();
                }
            }
        }

        private void AddNewStatement()
        {
            string sqlQuery = "INSERT INTO Аттестаты (Серия_аттестата, Номер_аттестата, Количество_оценок_пять, " +
                              "Количество_оценок_четыре, Количество_оценок_три, Изучаемый_иностранный_язык) VALUES " +
                              $"({certificatePage.seriesOfSchoolCertificateTextBox.Text}, " +
                              $"{certificatePage.numberOfSchoolCertificateTextBox.Text}, " +
                              $"{certificatePage.countOfFivesTextBox.Text}, " +
                              $"{certificatePage.countOfFoursTextBox.Text}, " +
                              $"{certificatePage.countOfTriplesTextBox.Text}, " +
                              $"\'{certificatePage.targetLanguageTextBox.Text}\');";

            dataBase.OpenConnection();
            SqlCommand command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            int gender = 0;
            if (personalFirstPage.maleButton.IsChecked.Value)
                gender = 1;

            sqlQuery = "INSERT INTO Абитуриенты (Серия_паспорта, Номер_паспорта, Фамилия, Имя, Отчество, Пол, " +
                       "Дата_рождения, Адрес_по_прописке, Номер_телефона, Название_учебного_заведения, Год_окончания, " +
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

            command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            string IdSpecialization = "";
            foreach (DataRow row in specializations.Rows)
            {
                if (row.ItemArray[1].ToString() == personalSecondPage.nameSpecializationComboBox.Text)
                {
                    IdSpecialization = row.ItemArray[0].ToString();
                    break;
                }
            }

            sqlQuery = "INSERT INTO Заявления_на_поступление (Серия_паспорта, Номер_паспорта, Код_специальности, " +
                       "Уровень_образования, Вариант_обучения, Форма_обучения) VALUES " +
                       $"({personalFirstPage.seriesOfPassportTextBox.Text}, " +
                       $"{personalFirstPage.numberOfPassportTextBox.Text}, " +
                       $"\'{IdSpecialization}\', " +
                       $"\'{personalSecondPage.levelEducationComboBox.Text}\', " +
                       $"\'{personalSecondPage.optionEducationComboBox.Text}\', " +
                       $"\'{personalSecondPage.formEducationComboBox.Text}\');";

            command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.CloseConnection();
        }

        private void ChangeStatement()
        {
            string sqlQuery = "UPDATE Аттестаты SET " +
                              $"Количество_оценок_пять = {certificatePage.countOfFivesTextBox.Text}, " +
                              $"Количество_оценок_четыре = {certificatePage.countOfFoursTextBox.Text}, " +
                              $"Количество_оценок_три = {certificatePage.countOfTriplesTextBox.Text}, " +
                              $"Изучаемый_иностранный_язык = \'{certificatePage.targetLanguageTextBox.Text}\' " +
                              $"WHERE Серия_аттестата = {certificatePage.seriesOfSchoolCertificateTextBox.Text} " +
                              $"AND Номер_аттестата = {certificatePage.numberOfSchoolCertificateTextBox.Text}";

            dataBase.OpenConnection();
            SqlCommand command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            int gender = 0;
            if (personalFirstPage.maleButton.IsChecked.Value)
                gender = 1;

            sqlQuery = "UPDATE Абитуриенты SET " +
                       $"Фамилия = \'{personalFirstPage.lastNameTextBox.Text}\', " +
                       $"Имя = \'{personalFirstPage.firstNameTextBox.Text}\', " +
                       $"Отчество = \'{personalFirstPage.middleNameTextBox.Text}\', " +
                       $"Пол = {gender}, " +
                       $"Дата_рождения = \'{personalFirstPage.birthdayDatePicker.Text}\', " +
                       $"Адрес_по_прописке = \'{personalSecondPage.addressTextBox.Text}\', " +
                       $"Номер_телефона = \'{personalSecondPage.phoneTextBox.Text}\', " +
                       $"Название_учебного_заведения = \'{personalSecondPage.schoolTextBox.Text}\', " +
                       $"Год_окончания = {personalSecondPage.yearGraduationTextBox.Text} " +
                       $"WHERE Серия_паспорта = {personalFirstPage.seriesOfPassportTextBox.Text} " +
                       $"AND Номер_паспорта = {personalFirstPage.numberOfPassportTextBox.Text}";

            command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();

            string IdSpecialization = "";
            foreach (DataRow row in specializations.Rows)
            {
                if (row.ItemArray[1].ToString() == personalSecondPage.nameSpecializationComboBox.Text)
                {
                    IdSpecialization = row.ItemArray[0].ToString();
                    break;
                }
            }

            sqlQuery = "UPDATE Заявления_на_поступление SET " +
                       $"Код_специальности = \'{IdSpecialization}\', " +
                       $"Уровень_образования = \'{personalSecondPage.levelEducationComboBox.Text}\', " +
                       $"Вариант_обучения = \'{personalSecondPage.optionEducationComboBox.Text}\', " +
                       $"Форма_обучения = \'{personalSecondPage.formEducationComboBox.Text}\' " +
                       $"WHERE Серия_паспорта = {personalFirstPage.seriesOfPassportTextBox.Text} " +
                       $"AND Номер_паспорта = {personalFirstPage.numberOfPassportTextBox.Text}";

            command = new SqlCommand(sqlQuery, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.CloseConnection();
        }

        private bool CheckExistenceSpecialty()
        {
            string sqlQuery = "SELECT Название_специальности, Уровень_образования, Вариант_обучения, Форма_обучения " +
                              "FROM Специальности WHERE " +
                              $"Название_специальности = \'{personalSecondPage.nameSpecializationComboBox.Text}\' AND " +
                              $"Уровень_образования = \'{personalSecondPage.levelEducationComboBox.Text}\' AND " +
                              $"Вариант_обучения = \'{personalSecondPage.optionEducationComboBox.Text}\' AND " +
                              $"Форма_обучения = \'{personalSecondPage.formEducationComboBox.Text}\'";

            dataBase.OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, dataBase.GetConnection());

            DataTable dt = new DataTable();

            adapter.Fill(dt);
            dataBase.CloseConnection();

            return dt.Rows.Count > 0;
        }

        private void CheckEnteredInfo()
        {
            if (personalInfo == null)
            {
                if (certificatePage.numberOfSchoolCertificateTextBox.Text != string.Empty
                    || certificatePage.seriesOfSchoolCertificateTextBox.Text != string.Empty
                    || certificatePage.countOfFivesTextBox.Text != string.Empty
                    || certificatePage.countOfFoursTextBox.Text != string.Empty
                    || certificatePage.countOfTriplesTextBox.Text != string.Empty
                    || personalFirstPage.numberOfPassportTextBox.Text != string.Empty
                    || personalFirstPage.seriesOfPassportTextBox.Text != string.Empty
                    || personalFirstPage.lastNameTextBox.Text != string.Empty
                    || personalFirstPage.firstNameTextBox.Text != string.Empty
                    || personalFirstPage.middleNameTextBox.Text != string.Empty
                    || personalFirstPage.birthdayDatePicker.Text != string.Empty
                    || personalSecondPage.addressTextBox.Text != string.Empty
                    || personalSecondPage.phoneTextBox.Text != string.Empty
                    || personalSecondPage.schoolTextBox.Text != string.Empty
                    || personalSecondPage.yearGraduationTextBox.Text != string.Empty
                    || personalSecondPage.nameSpecializationComboBox.Text != string.Empty
                    || personalSecondPage.levelEducationComboBox.Text != string.Empty
                    || personalSecondPage.optionEducationComboBox.Text != string.Empty
                    || personalSecondPage.formEducationComboBox.Text != string.Empty)
                {
                    CustomMessageBox message = new CustomMessageBox("Подтверждение", "Вы действительно хотите выйти?\nВведенные данные не сохранятся");
                    if (message.ShowDialog() == true)
                        Close();
                }
                else
                    Close();
            }
            else
            {
                if (personalFirstPage.seriesOfPassportTextBox.Text.ToString() != personalInfo[0].ToString()
                    || personalFirstPage.numberOfPassportTextBox.Text.ToString() != personalInfo[1].ToString()
                    || personalFirstPage.lastNameTextBox.Text.ToString() != personalInfo[2].ToString()
                    || personalFirstPage.firstNameTextBox.Text.ToString() != personalInfo[3].ToString()
                    || personalFirstPage.middleNameTextBox.Text.ToString() != personalInfo[4].ToString()
                    || personalFirstPage.birthdayDatePicker.Text.ToString() != personalInfo[6].ToString().Substring(0, 10)
                    || personalSecondPage.addressTextBox.Text.ToString() != personalInfo[7].ToString()
                    || personalSecondPage.phoneTextBox.Text.ToString() != personalInfo[8].ToString()
                    || personalSecondPage.schoolTextBox.Text.ToString() != personalInfo[9].ToString()
                    || personalSecondPage.yearGraduationTextBox.Text.ToString() != personalInfo[10].ToString()
                    || certificatePage.seriesOfSchoolCertificateTextBox.Text.ToString() != personalInfo[11].ToString()
                    || certificatePage.numberOfSchoolCertificateTextBox.Text.ToString() != personalInfo[12].ToString()
                    || certificatePage.countOfFivesTextBox.Text.ToString() != personalInfo[13].ToString()
                    || certificatePage.countOfFoursTextBox.Text.ToString() != personalInfo[14].ToString()
                    || certificatePage.countOfTriplesTextBox.Text.ToString() != personalInfo[15].ToString()
                    || certificatePage.targetLanguageTextBox.Text.ToString() != personalInfo[16].ToString()
                    || personalSecondPage.levelEducationComboBox.Text.ToString() != personalInfo[18].ToString()
                    || personalSecondPage.optionEducationComboBox.Text.ToString() != personalInfo[19].ToString()
                    || personalSecondPage.formEducationComboBox.Text.ToString() != personalInfo[20].ToString())
                {
                    CustomMessageBox message = new CustomMessageBox("Подтверждение", "Вы действительно хотите выйти?\nИзменённые данные не сохранятся");
                    if (message.ShowDialog() == true)
                        Close();
                }
                else
                    Close();
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CheckEnteredInfo();
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CheckEnteredInfo();
        }
    }
}
