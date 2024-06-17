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
using wcheck.wshell.Utils;
using wshell.Abstract;
using wshell.Net;
using wshell.Net.Nodes;
using wshell.Utils;

namespace infosystems.task.shellv1.Controls
{
    /// <summary>
    /// Логика взаимодействия для HostTaskControl.xaml
    /// </summary>
    public partial class HostTaskControl : UserControl
    {
        private static Brush _inCheck = "#77cefc".GetBrush();
        private static Brush _inProgress = "#a6ff63".GetBrush();
        private static Brush _inComplete = "#fca577".GetBrush();
        private static Brush _inAfk = "#ff6363".GetBrush();
        public event EventHandler<bool> Complited;
        public bool IsAfk { get; private set; }
        public bool IsComplited { get; private set; }
        public ShellClientProviding ClientProviding { get; }
        public Host Host { get; private set; }
        public string TaskId { get; private set; }
        public HostTaskControl(Host host, InfoSystemShell shell, CancellationToken token)
        {
            InitializeComponent();
            uiTitle.Text = $"{host.TargetIp} [{host.Group.Name}]";
            TaskId = Guid.NewGuid().ToString().Replace("-", "").Remove(0, 5).ToUpper();
            ClientProviding = shell.RequestClientProviding();
            Host = host;
            CheckAfk(token);
        }

        public async void StartTask()
        {
            await ClientProviding.GetRunShellAsync(Host.TargetIp, "ee3de59e-00e8-40e9-bfcd-cba116b9a81d");
            ClientProviding.GetRedirectAsync(Host.TargetIp, "ee3de59e-00e8-40e9-bfcd-cba116b9a81d", new Node()
                .SetAttribute("type", "start task")
                .SetAttribute("task id", TaskId));
        }

        private async void CheckAfk(CancellationToken token)
        {
            await Task.Run(async () => 
            {
                while (!token.IsCancellationRequested && !IsAfk && !IsComplited)
                {
                    var values = await ClientProviding.GetSystemParametersAsync(Host.TargetIp);
                    if(values == null)
                    {
                        IsAfk = true;
                        this.Invoke(() =>
                        {
                            if(!IsComplited)
                                uiLed.Fill = _inAfk;
                        });
                        break;
                    }
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inCheck;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inProgress;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inCheck;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inProgress;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inCheck;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inProgress;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inCheck;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inProgress;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inCheck;
                    });
                    await Task.Delay(500);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inProgress;
                    });
                }
            });
        }

        public void SetOutput(string value)
        {
            this.Invoke(() => 
            {
                uiOutput.Text = value;
            });
        }

        public void SetState(string value)
        {
            this.Invoke(() =>
            {
                uiState.Text = value;
            });
        }

        public void SetComplete()
        {
            IsComplited = true;
            IsAfk = true;
            this.Invoke(() =>
            {
                uiLed.Fill = _inComplete;
            });
            Complited?.Invoke(this, true);
        }
    }
}
