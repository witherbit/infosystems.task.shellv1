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
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wcheck;
using wshell.Enums;
using wshell.Core;
using infosystems.task.shellv1.Pages;
using wcheck.Statistic;
using infosystems.task.shellv1.Objects;

namespace infosystems.task.shellv1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static InfoSystemShell Shell { get; set; }
        public MainPage(InfoSystemShell shell)
        {
            InitializeComponent();
            Shell = shell;
            if (Shell.NetworkingConnectionType == NetworkingConnectionType.Client)
            {
                uiTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                uiFrame.Content = new HostsPage();
            }
        }

        private void uiCloseTab_Click(object sender, RoutedEventArgs e)
        {
            Shell.Stop();
        }
    }
}
