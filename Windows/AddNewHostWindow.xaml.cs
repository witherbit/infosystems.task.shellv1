using infosystems.task.shellv1.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using wcheck.wcontrols;

namespace infosystems.task.shellv1.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewHostWindow.xaml
    /// </summary>
    public partial class AddNewHostWindow : Window
    {
        public Host Host { get; private set; }
        public AddNewHostWindow()
        {
            InitializeComponent();
            LoadGroups();
        }
        public AddNewHostWindow(Host host)
        {
            InitializeComponent();
            Host = host;
            Title = "Редактирование хоста";
            uiTextBox.Text = Host.TargetIp;
            if (Host.Group == TaskController.DefaultGroup)
                uiComboBox.SelectedIndex = 0;
            else
            {
                var groups = TaskController.GetGroups().ToList();
                uiComboBox.SelectedIndex = groups.IndexOf(Host.Group) + 1;
            }
            LoadGroups();
        }

        private void LoadGroups()
        {
            var groups = TaskController.GetGroups();
            foreach (var group in groups)
            {
                uiComboBox.Items.Add(new ComboBoxItem
                {
                    Content = group.Name,
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (uiTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Ошибка при добавлении или изменении хоста:\r\nIP хоста не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TaskController.ExistHost(uiTextBox.Text))
            {
                var isEditableHost = TaskController.GetHost(uiTextBox.Text);
                if (isEditableHost != null && isEditableHost != Host)
                {
                    MessageBox.Show($"Ошибка при добавлении или изменении хоста:\r\nIP хоста не должно совпадать с существующим", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            IPAddress ipAddress;
            if (!IPAddress.TryParse(uiTextBox.Text, out ipAddress) && uiTextBox.Text != "localhost")
            {
                MessageBox.Show($"Ошибка при добавлении или изменении хоста:\r\n{uiTextBox.Text} не является IP адресом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Host == null)
            {
                Host = new Host(uiTextBox.Text, NormalizeSelected());
            }
            else
            {
                Host.TargetIp = uiTextBox.Text;
                Host.Group = NormalizeSelected();
            }
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private Group NormalizeSelected()
        {
            var selected = uiComboBox.SelectedIndex - 1;
            if (selected < 0)
                return TaskController.DefaultGroup;
            var groups = TaskController.GetGroups();
            return groups[selected];
        }
    }
}
