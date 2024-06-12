using infosystems.task.shellv1.Objects;
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
using wcheck.wcontrols;

namespace infosystems.task.shellv1.Controls
{
    /// <summary>
    /// Логика взаимодействия для HostControl.xaml
    /// </summary>
    public partial class HostControl : UserControl
    {
        public Host Host { get; private set; }
        public HostControl(Host host)
        {
            InitializeComponent();
            Host = host;
            Host.DeleteGroup += (sender, e) =>
            {
                Host.Group = TaskController.DefaultGroup;
                uiTextBlock.Text = $"{Host.TargetIp} [{Host.Group.Name}]";
            };
            uiTextBlock.Text = $"{Host.TargetIp} [{Host.Group.Name}]";
            TaskController.AddHost(this);
        }

        private void uiEdit_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fca577".GetBrush();
        }

        private void uiEdit_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#ffffff".GetBrush();
        }

        private void uiEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fc8343".GetBrush();
        }

        private void uiEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var path = border.Child as Path;
            path.Stroke = "#fca577".GetBrush();
            if (border == uiEdit)
            {
                TaskController.EditHost(this);
            }
            else
            {
                TaskController.DeleteHost(this);
            }
        }
    }
}
