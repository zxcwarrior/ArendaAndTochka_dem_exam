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

namespace ArendaAndTochka_dem_exam.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminDesktopPage.xaml
    /// </summary>
    public partial class AdminDesktopPage : Page
    {
        private Users SelectedUser = null;
        public AdminDesktopPage()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            DGUsers.ItemsSource = DatabaseEntities.GetContext().Users.ToList();
            RoleChngCBx.ItemsSource = DatabaseEntities.GetContext().Roles.ToList();
        }
        private void ClearEditPanel()
        {
            NameChngTBx.Clear();
            SurnameChngTBx.Clear();
            PatronomicChngTBx.Clear();
            RoleChngCBx.SelectedIndex = -1;
            LoginChngTBx.Clear();
            PassChngTBx.Clear();
            BlockStatChngTbx.Clear();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser = null;
            EditPanel.Visibility = Visibility.Visible;
            ClearEditPanel();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DGUsers.SelectedItem is Users user)
            {
                SelectedUser = user;
                NameChngTBx.Text = user.UserName;
                SurnameChngTBx.Text = user.UserSurname;
                PatronomicChngTBx.Text = user.UserPatronomic;
                LoginChngTBx.Text = user.UserLogin;
                PassChngTBx.Text = user.UserPassword;
                RoleChngCBx.SelectedValue = user.RoleID;
                BlockStatChngTbx.Text = user.IsBlocked.ToString();
                EditPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DGUsers.SelectedItem is Users userForDelete)
            {
                if (MessageBox.Show("Удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var context = DatabaseEntities.GetContext();
                    context.Users.Remove(userForDelete);
                    context.SaveChanges();
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameChngTBx.Text;
                string surname = SurnameChngTBx.Text;
                string patronomic = PatronomicChngTBx.Text;
                string login = LoginChngTBx.Text;
                string password = PassChngTBx.Text;
                int? roleId = RoleChngCBx.SelectedValue as int?;
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(patronomic) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || !roleId.HasValue)
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                var context = DatabaseEntities.GetContext();
                if (SelectedUser == null)
                {
                    var newUser = new Users
                    {
                        UserName = name,
                        UserSurname = surname,
                        UserPatronomic = patronomic,
                        UserLogin = login,
                        UserPassword = password,
                        RoleID = roleId.Value,
                        IsBlocked = false,
                        LastTimeLogin = null
                    };
                    context.Users.Add(newUser);
                }
                else
                {
                    SelectedUser.UserName = name;
                    SelectedUser.UserSurname = surname;
                    SelectedUser.UserPatronomic = patronomic;
                    SelectedUser.UserLogin = login;
                    SelectedUser.UserPassword = password;
                    SelectedUser.RoleID = roleId.Value;
                }
                context.SaveChanges();
                MessageBox.Show("Пользователь сохранён");
                ClearEditPanel();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearEditPanel();
            EditPanel.Visibility = Visibility.Collapsed;
        }
    }
}
