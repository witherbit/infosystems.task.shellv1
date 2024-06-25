using infosystems.task.shellv1.Controls;
using infosystems.task.shellv1.Enums;
using infosystems.task.shellv1.Forms.Abstract;
using infosystems.task.shellv1.Forms.Tests;
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
using wcheck;
using wcheck.Pages;
using wcheck.Statistic;
using wcheck.Statistic.Nodes;
using wcheck.Statistic.Styles;
using wcheck.Statistic.Templates;
using wcheck.wcontrols;
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Utils;

namespace infosystems.task.shellv1.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Page
    {
        private bool _scapStart = false;
        private bool _nmapStart = false;

        public ITest Forms { get; }
        private int _navigationCurrent = 1;
        private int _navigationMax => _navigationControls.Count;
        private List<TestingControl> _navigationControls = new List<TestingControl>();

        private static Brush _inProgress = "#77cefc".GetBrush();
        private static Brush _inComplete = "#fca577".GetBrush();
        public ShellBase ScapShell { get; }
        public ShellBase NmapShell { get; }
        public ShellOutputRedirect ScapRedirect { get; }
        public ShellOutputRedirect NmapRedirect { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public TaskPage(ShellBase nmapShell, ShellBase scapShell)
        {
            InitializeComponent();
            CancellationTokenSource = new CancellationTokenSource();
            switch (TaskController.ISType)
            {
                case Enums.ISType.GIS:
                    Forms = new GisTest();
                    uiButtonNext.IsEnabled = true;
                    foreach (var item in Forms.TestElements)
                        _navigationControls.Add(new TestingControl(item) { Margin = new Thickness(10)});
                    uiTextFormsCount.Text = $"{_navigationCurrent}/{_navigationMax}";
                    SetNavigationForms();
                    break;
                case Enums.ISType.ISPD:
                    Forms = new IspdTest();
                    uiButtonNext.IsEnabled = true;
                    foreach (var item in Forms.TestElements)
                        _navigationControls.Add(new TestingControl(item) { Margin = new Thickness(10) });
                    uiTextFormsCount.Text = $"{_navigationCurrent}/{_navigationMax}";
                    SetNavigationForms();
                    break;
                case Enums.ISType.AS:
                    Forms = new ASTest();
                    uiButtonNext.IsEnabled = true;
                    foreach (var item in Forms.TestElements)
                        _navigationControls.Add(new TestingControl(item) { Margin = new Thickness(10) });
                    uiTextFormsCount.Text = $"{_navigationCurrent}/{_navigationMax}";
                    SetNavigationForms();
                    break;
            }
            NmapShell = nmapShell;
            ScapShell = scapShell;
            TaskController.TaskPage = this;
            ScapRedirect = new ShellOutputRedirect(MainPage.Shell);
            ScapShell.SetEventRedirect(ScapRedirect);
            NmapRedirect = new ShellOutputRedirect(MainPage.Shell);
            NmapShell.SetEventRedirect(NmapRedirect);

            ScapRedirect.Complete += OnScapComplete;
            NmapRedirect.Complete += OnNmapComplete;
            ScapRedirect.State += OnScapState;
            NmapRedirect.State += OnNmapState;
            ScapRedirect.Output += OnScapOutput;
            NmapRedirect.Output += OnNmapOutput;

            ScapShell.Run();
            NmapShell.Run();

            MainPage.Shell.InvokeTabFocus(MainPage.Shell.Page);

            MainPage.Shell.InvokeStartTaskView("ee3de59e-00e8-40e9-bfcd-cba116b9a81d");
            MainPage.Shell.InvokeStartTaskView("4f1fc6ba-56b0-4844-8e2a-485578e8bc1f");


            foreach (var h in TaskController.GetHosts())
            {
                if (h.TargetIp == "localhost" || h.TargetIp == "127.0.0.1")
                    continue;
                if (h.Group == TaskController.DefaultGroup)
                {
                    var taskHost = new HostTaskControl(h, MainPage.Shell, CancellationTokenSource.Token)
                    {
                        Margin = new Thickness(5, 5, 5, 0)
                    };
                    TaskController.AddHostTask(taskHost);
                    uiHosts.Children.Add(taskHost);
                }
            }
                

            foreach(var g in TaskController.GetGroups())
            {
                foreach (var h in TaskController.GetHosts())
                {
                    if (h.TargetIp == "localhost" || h.TargetIp == "127.0.0.1")
                        continue;
                    if (h.Group == g)
                    {
                        var taskHost = new HostTaskControl(h, MainPage.Shell, CancellationTokenSource.Token)
                        {
                            Margin = new Thickness(5, 5, 5, 0)
                        };
                        TaskController.AddHostTask(taskHost);
                        uiHosts.Children.Add(taskHost);
                    }
                }
            }

            TaskController.ActivateHostTasks();
        }

        private void OnNmapOutput(string obj)
        {
            this.Invoke(() =>
            {
                uiOutputNmap.Text = obj;
            });
        }

        private void OnScapOutput(string obj)
        {
            this.Invoke(() =>
            {
                uiOutputScap.Text = obj;
            });
        }

        private void OnNmapState(string obj)
        {
            this.Invoke(() =>
            {
                if(!_nmapStart)
                {
                    _nmapStart = true;
                    uiLedNmap.Fill = _inProgress;
                }
                uiStateNmap.Text = obj;
            });
        }

        private void OnScapState(string obj)
        {
            this.Invoke(() =>
            {
                if (!_scapStart)
                {
                    _scapStart = true;
                    uiLedScap.Fill = _inProgress;
                }
                uiStateScap.Text = obj;
            });
        }

        private void OnNmapComplete(IStatisticTemplate template)
        {
            TaskController.NmapTemplate = template;
            this.Invoke(() =>
            {
                uiLedNmap.Fill = _inComplete;
                uiOutputNmap.Text = "Completed";
            });
        }

        private void OnScapComplete(IStatisticTemplate template)
        {
            TaskController.ScapTemplate = template;
            this.Invoke(() =>
            {
                uiLedScap.Fill = _inComplete;
                uiOutputScap.Text = "Completed";
            });
        }

        private void uiButtonBack_Click(object sender, RoutedEventArgs e)
        {
            _navigationCurrent--;
            SetNavigationForms();
            CheckButtons();
        }

        private void uiButtonNext_Click(object sender, RoutedEventArgs e)
        {
            _navigationCurrent++;
            SetNavigationForms();
            CheckButtons();
        }

        private void SetNavigationForms()
        {
            uiGridForms.Children.Clear();
            uiGridForms.Children.Add(_navigationControls[_navigationCurrent - 1]);
            uiTextFormsCount.Text = $"{_navigationCurrent}/{_navigationMax}";
        }
        private void CheckButtons()
        {
            if (_navigationCurrent == _navigationMax)
            {
                uiButtonNext.IsEnabled = false;
            }
            else if(_navigationCurrent > 1 && _navigationCurrent < _navigationMax)
            {
                uiButtonNext.IsEnabled = true;
                uiButtonBack.IsEnabled = true;
            }
            else if (_navigationCurrent == 1)
            {
                uiButtonBack.IsEnabled = false;
            }
        }

        private bool CheckQestionsComplete()
        {
            var arr = Forms.TestElements.ToArray();
            int count = 0;
            foreach (var item in arr)
            {
                if(item.SelectedAnswer > -1)
                    count++;
            }
            if (count == arr.Length)
                return true;
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TaskController.NmapTemplate == null || TaskController.ScapTemplate == null)
            {
                MessageBox.Show($"Невозможно перейти к отчету:\r\nОдин или несколько модулей не закончили свою работу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!CheckQestionsComplete())
            {
                MessageBox.Show($"Невозможно перейти к отчету:\r\nОпрос не пройден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var engine = new StatisticEngine();
            engine.InsertTemplate(new ISTaskTemplate(TaskController.ISType, Forms.GetResult()));
            engine.InsertTemplate(TaskController.NmapTemplate);
            engine.InsertTemplate(TaskController.ScapTemplate);
            foreach (var template in TaskController.GetHostTemplates())
            {
                engine.InsertTemplate(template);
            }
            engine.Nodes.Add(new BreakStatisticNode());
            engine.Nodes.Add(new TextStatisticNode($"Сгенерировано с помощью ПО witherbit wcheck", new TextNodeStyle
            {
                Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                FontSize = 10,
                WpfFontSize = 12,
                IsItalic = true,
            }));
            engine.Nodes.Add(new TextStatisticNode($"Версия: {SettingsParamConsts.Build}{SettingsParamConsts.BuildType}", new TextNodeStyle
            {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 10,
                    WpfFontSize = 12,
                    IsItalic = true,
                }));
            engine.Nodes.Add(new TextStatisticNode(
                $"Автор: {SettingsParamConsts.Company} - {SettingsParamConsts.Author}", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 10,
                    WpfFontSize = 12,
                    IsItalic = true,
                }));
            engine.Nodes.Add(new TextStatisticNode($"Обратная связь: {SettingsParamConsts.Support}", new TextNodeStyle
                {
                    Aligment = wcheck.Statistic.Enums.TextAligment.Right,
                    FontSize = 10,
                    WpfFontSize = 12,
                    IsItalic = true,
                }));
            MainPage.Shell.Callback.Invoke(MainPage.Shell, new Schema(CallbackType.RegisterPage).SetProviding(new StatisticPage(engine)));
        }
    }
}
