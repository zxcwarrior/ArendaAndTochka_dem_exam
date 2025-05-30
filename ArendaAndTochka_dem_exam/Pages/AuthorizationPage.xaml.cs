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
using ArendaAndTochka_dem_exam.Models;
using ArendaAndTochka_dem_exam.Utilities;

namespace ArendaAndTochka_dem_exam.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int failedAttempts = 0;
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void EnterBT_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (string.IsNullOrEmpty(LoginTB.Text))
            {
                error.AppendLine("Введите логин");
            }
            if (string.IsNullOrEmpty(PassPB.Password))
            {
                error.AppendLine("Введите пароль");
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var UserObj = DatabaseEntities.GetContext().Users.FirstOrDefault(u => u.UserLogin == LoginTB.Text);

                if(UserObj == null)
                {
                    MessageBox.Show("Пользователя с таким логином не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if((bool)UserObj.IsBlocked && UserObj.RoleID != 1)
                {
                    MessageBox.Show("Ваш аккаунт заблокирован", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (UserObj.LastTimeLogin.HasValue && (DateTime.Now - UserObj.LastTimeLogin.Value).Days > 31 && UserObj.RoleID != 1)
                {
                    UserObj.IsBlocked = true;
                    DatabaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Ваш аккаунт был заблокирован из-за отсутствия активности более 31 дня.");
                    return;
                }
                if(UserObj.UserPassword != PassPB.Password)
                {
                    if(UserObj.RoleID != 1)
                    {
                        failedAttempts++;
                        if(failedAttempts >= 3)
                        {
                            UserObj.IsBlocked = true;
                            DatabaseEntities.GetContext().SaveChanges();
                            MessageBox.Show("Вы ввели неправильно пароль 3 раза. Аккаунт заблокирован.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                        {
                            MessageBox.Show($"Неверный пароль. Осталось попыток: {3 - failedAttempts}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                failedAttempts = 0;
                UserObj.LastTimeLogin = DateTime.Now;
                DatabaseEntities.GetContext().SaveChanges();
                Manager.AuthUser = UserObj;

                switch (UserObj.RoleID)
                {
                    case 1:
                        Manager.DesktopFrame.Navigate(new AdminDesktopPage());
                        break;
                    case 2:
                        Manager.DesktopFrame.Navigate(new UserPage());
                        break;
                    case 3:
                        Manager.DesktopFrame.Navigate(new RukovoditelPage());                       
                        break;
                    default:
                        MessageBox.Show("Такого пользователя нет в системе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show($"Ошибка: {Ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
