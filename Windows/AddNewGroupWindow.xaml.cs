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
    /// Логика взаимодействия для AddNewGroupWindow.xaml
    /// </summary>
    public partial class AddNewGroupWindow : Window
    {
        public Group Group { get; private set; }
        public AddNewGroupWindow()
        {
            InitializeComponent();
        }
        public AddNewGroupWindow(Group group)
        {
            InitializeComponent();
            Group = group;
            uiTextBox.Text = Group.Name;
            Title = "Редактирование группы";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(uiTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Ошибка при добавлении или изменении группы:\r\nимя группы не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (uiTextBox.Text.ToLower() == "default")
            {
                MessageBox.Show($"Ошибка при добавлении или изменении группы:\r\nимя группы не должно быть 'default'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TaskController.ExistGroup(uiTextBox.Text))
            {
                MessageBox.Show($"Ошибка при добавлении или изменении группы:\r\nимя группы не должно совпадать с существующим", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Group == null)
            {
                Group = new Group(uiTextBox.Text);
            }
            else
            {
                Group.Name = uiTextBox.Text;
            }
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
