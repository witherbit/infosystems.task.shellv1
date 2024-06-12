using infosystems.task.shellv1.Controls;
using infosystems.task.shellv1.Objects;
using infosystems.task.shellv1.Utils;
using infosystems.task.shellv1.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wcheck;
using wcheck.wcontrols;
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wcheck.wshell.Utils;
using wshell.Utils;

namespace infosystems.task.shellv1.Pages
{
    /// <summary>
    /// Логика взаимодействия для HostsPage.xaml
    /// </summary>
    public partial class HostsPage : Page
    {
        private int _props = 0;
        private int _shells = 0;
        private Brush _propClose => "#7b1818".GetBrush();
        private Brush _propOpen => "#177a42".GetBrush();
        public static HostsPage? Instance { get; private set; }
        public HostsPage()
        {
            InitializeComponent();

            Instance = this;

            if(MainPage.Shell.Shells.FirstOrDefault(x => x.ShellInfo.Id.ToString() == "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f") != null)
            {
                uiCircleNmap.Fill = "#fca577".GetBrush();
                _shells++;
            }
            if (MainPage.Shell.Shells.FirstOrDefault(x => x.ShellInfo.Id.ToString() == "ee3de59e-00e8-40e9-bfcd-cba116b9a81d") != null)
            {
                uiCircleScap.Fill = "#fca577".GetBrush();
                _shells++;
            }
                

            uiSelectIS.Click += (s, e) => OnShellsPropertyChanged();
            OnShellsPropertyChanged();
        }

        public void OnShellsPropertyChanged()
        {
            int props = 0;
            var nmapPath = MainPage.Shell.RequestSettingsProperty("pNmapPath", "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");
            var gatewayIp = MainPage.Shell.RequestSettingsProperty("pMainIp", "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");
            var subnetMask = MainPage.Shell.RequestSettingsProperty("pSubnet", "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");
            var checkSelectedOnly = MainPage.Shell.RequestSettingsProperty("pChkOnlySelected", "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");

            var ovalDefs = MainPage.Shell.RequestSettingsProperty("pOvalDef", "ee3de59e-00e8-40e9-bfcd-cba116b9a81d");

            var type = uiSelectIS.ISType;
            switch (type)
            {
                case Enums.ISType.GIS:
                    uiTextPropertyForms.Text = "ГИС";
                    break;
                case Enums.ISType.ISPD:
                    uiTextPropertyForms.Text = "ИСПДн";
                    break;
                case Enums.ISType.AS:
                    uiTextPropertyForms.Text = "АС";
                    break;
            }
            TaskController.ISType = uiSelectIS.ISType;

            if (nmapPath != null)
                if (File.Exists(nmapPath.Value as string))
                {
                    uiTextPropertyNmapExist.Text = "Существует";
                    uiTextPropertyNmapExist.Foreground = _propOpen;
                    props++;
                }
                else
                {
                    uiTextPropertyNmapExist.Text = "Не существует";
                    uiTextPropertyNmapExist.Foreground = _propClose;
                }

            IPAddress gate;

            if (gatewayIp != null)
                if (IPAddress.TryParse(gatewayIp.Value as string, out gate))
                {
                    uiTextPropertyGateway.Text = gate.ToString();
                    uiTextPropertyGateway.Foreground = _propOpen;
                    props++;
                }
                else
                {
                    uiTextPropertyGateway.Text = "Неверный IP";
                    uiTextPropertyGateway.Foreground = _propClose;
                }

            IPAddress subnet;

            if (subnetMask != null)
                if (IPAddress.TryParse(subnetMask.Value as string, out subnet))
                {
                    uiTextPropertySubnet.Text = subnet.ToString();
                    uiTextPropertySubnet.Foreground = _propOpen;
                    props++;
                }
                else
                {
                    uiTextPropertySubnet.Text = "Неверная маска подсети";
                    uiTextPropertySubnet.Foreground = _propClose;
                }
                    

            if (checkSelectedOnly != null)
                uiTextPropertyCheckSelected.Text = (bool)checkSelectedOnly.Value ? "Только добавленные" : "Вся сеть";

            if (ovalDefs != null)
                if (File.Exists(ovalDefs.Value as string))
                {
                    uiTextPropertyOval.Text = "Существует";
                    uiTextPropertyOval.Foreground = _propOpen;
                    props++;
                }
                else
                {
                    uiTextPropertyOval.Text = "Не существует";
                    uiTextPropertyOval.Foreground = _propClose;
                }
            _props = props;
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fca577".GetBrush();
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#1f1f1f".GetBrush();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fc8343".GetBrush();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fca577".GetBrush();
            if(textBox.Text == "Добавить")
            {
                var window = new AddNewHostWindow();
                if (window.ShowDialog() == true)
                {
                    var control = new HostControl(window.Host)
                    {
                        IsHitTestVisible = true,
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    window.Host.Delete += (s, a) =>
                    {
                        uiStackPanelHosts.Children.Remove(control);
                    };
                    uiStackPanelHosts.Children.Add(control);
                    uiScrollHosts.ScrollToEnd();
                }
            }
            else if(textBox.Text == "Импорт")
            {
                if (TaskController.GetHosts().Length > 0 || TaskController.GetGroups().Length > 0)
                    if (MessageBox.Show("Вы хотите загрузить файл сетевой конфигурации? Это приведет к удалению текущей конфигурации сети", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".netcfg";
                dlg.Filter = "Task network configuration Files (*.netcfg)|*.netcfg";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    var hctrls = new List<HostControl>();
                    var gctrls = new List<GroupControl>();
                    foreach (var child in uiStackPanelHosts.Children)
                    {
                        var ch = child as HostControl;
                        hctrls.Add(ch);
                    }
                    foreach (var child in uiStackPanelGroup.Children)
                    {
                        var ch = child as GroupControl;
                        gctrls.Add(ch);
                    }
                    foreach (var h in hctrls)
                    {
                        TaskController.DeleteHost(h);
                    }
                    foreach (var g in gctrls)
                    {
                        TaskController.DeleteGroup(g);
                    }
                    string filename = dlg.FileName;
                    var obj = Exporter.Import(filename);
                    var groups = new List<Group>();
                    foreach (var group in obj.Groups)
                    {
                        if (group.Name == TaskController.DefaultGroup.Name)
                            continue;
                        var imported = new Group(group.Name);
                        groups.Add(imported);
                        var control = new GroupControl(imported)
                        {
                            IsHitTestVisible = true,
                            Margin = new Thickness(0, 0, 0, 10)
                        };
                        imported.Delete += (s, a) =>
                        {
                            uiStackPanelGroup.Children.Remove(control);
                        };
                        uiStackPanelGroup.Children.Add(control);
                        uiScrollGroups.ScrollToEnd();
                    }
                    foreach (var host in obj.Hosts)
                    {
                        var group = groups.FirstOrDefault(x => x.Name == host.Group.Name);
                        Host imported;
                        if (group != null)
                            imported = new Host(host.TargetIp, group);
                        else
                            imported = new Host(host.TargetIp, TaskController.DefaultGroup);
                        var control = new HostControl(imported)
                        {
                            IsHitTestVisible = true,
                            Margin = new Thickness(0, 0, 0, 10)
                        };
                        imported.Delete += (s, a) =>
                        {
                            uiStackPanelHosts.Children.Remove(control);
                        };
                        uiStackPanelHosts.Children.Add(control);
                        uiScrollHosts.ScrollToEnd();
                    }
                    uiSelectIS.SetIs(obj.ISType);
                }
            }
            else if (textBox.Text == "Экспорт")
            {
                if (TaskController.GetHosts().Length < 1)
                {
                    MessageBox.Show($"Невозможно выполнить экспорт сетевой конфигурации:\r\nДолжен быть хотя-бы один хост", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".netcfg";
                dlg.Filter = "Task network configuration Files (*.netcfg)|*.netcfg";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string filename = dlg.FileName;
                    var obj = TaskController.GetExportObject();
                    obj.Export(filename);
                }
            }
        }

        private void TextBlock_MouseEnter_1(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fca577".GetBrush();
        }

        private void TextBlock_MouseLeave_1(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#1f1f1f".GetBrush();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fc8343".GetBrush();
        }

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBlock;
            textBox.Foreground = "#fca577".GetBrush();
            var window = new AddNewGroupWindow();
            if(window.ShowDialog() == true)
            {
                var control = new GroupControl(window.Group)
                {
                    IsHitTestVisible = true,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                window.Group.Delete += (s, a) =>
                {
                    uiStackPanelGroup.Children.Remove(control);
                };
                uiStackPanelGroup.Children.Add(control);
                uiScrollGroups.ScrollToEnd();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var displayType = "";
            var type = uiSelectIS.ISType;
            switch (type)
            {
                case Enums.ISType.GIS:
                    displayType = "ГИС";
                    break;
                case Enums.ISType.ISPD:
                    displayType = "ИСПДн";
                    break;
                case Enums.ISType.AS:
                    displayType = "АС";
                    break;
            }
            if(TaskController.HostsCount < 1)
            {
                MessageBox.Show("Невозможно начать задачу:\r\nОтсутствуют добавленные хосты.", $"Тестирование {displayType}", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!TaskController.ExistHost("localhost"))
                if(!TaskController.ExistHost("127.0.0.1"))
                {
                    MessageBox.Show("Невозможно начать задачу:\r\nОтсутствуют проверяющий хост.\r\nДля решения этой проблемы добавьте 'localhost' или '127.0.0.1'", $"Тестирование {displayType}", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            if (_shells != 2)
            {
                MessageBox.Show("Невозможно начать задачу:\r\nОтсутствует один или несколько модулей.\r\nОтсутствующие модули находятся в разделе 'Список компонентов задачи'.", $"Тестирование {displayType}", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if(_props != 4)
            {
                MessageBox.Show("Невозможно начать задачу:\r\nОдин или несколько параметров модулей настроены неправильно.\r\nНеверные параметры подсвечиваются красным цветом.", $"Тестирование {displayType}", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var i = MainPage.Shell.Shells.FirstOrDefault(x => x.ShellInfo.Id.ToString() == "ee3de59e-00e8-40e9-bfcd-cba116b9a81d");
            i.Run();
            var j = MainPage.Shell.Shells.FirstOrDefault(x => x.ShellInfo.Id.ToString() == "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");
            j.Run();

            MainPage.Shell.InvokeTabFocus(MainPage.Shell.Page);

            MainPage.Shell.InvokeStartTaskView("ee3de59e-00e8-40e9-bfcd-cba116b9a81d");
            MainPage.Shell.InvokeStartTaskView("4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");
        }
    }
}
