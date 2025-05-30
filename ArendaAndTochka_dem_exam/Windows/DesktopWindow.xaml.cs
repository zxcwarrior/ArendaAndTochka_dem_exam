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
using ArendaAndTochka_dem_exam.Utilities;
using ArendaAndTochka_dem_exam.Pages;

namespace ArendaAndTochka_dem_exam.Windows
{
    /// <summary>
    /// Логика взаимодействия для DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : Window
    {
        public DesktopWindow()
        {
            InitializeComponent();
            Manager.DesktopFrame = DesktopFrame;
            Manager.DesktopFrame.Navigate(new AuthorizationPage());
        }
    }
}
