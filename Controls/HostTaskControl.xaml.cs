using infosystems.task.shellv1.Objects;
using pwither.IO;
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
using wcheck.Statistic.Templates;
using wcheck.Utils;
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
        public bool IsAfk { get; private set; }
        public bool Check { get; private set; }
        public bool IsComplited { get; private set; }
        public ShellClientProviding ClientProviding { get; }
        public Host Host { get; private set; }
        public string TaskId { get; private set; }
        CancellationToken CancellationToken;
        public HostTaskControl(Host host, InfoSystemShell shell, CancellationToken token)
        {
            InitializeComponent();
            CancellationToken = token;
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
            if (Check)
                return;
            await Task.Run(async () => 
            {
                Check = true;
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
                            Check = false;
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
                    if(IsComplited)
                        this.Invoke(() =>
                        {
                            uiLed.Fill = _inComplete;
                        });
                }
            });
        }

        public void SetOutput(string value)
        {
            pwither.thrd.Locker.Lock locker = new pwither.thrd.Locker.Lock();
            using (locker.Write())
                if (IsAfk)
                {
                    IsAfk = false;
                    CheckAfk(CancellationToken);
                }
            this.Invoke(() => 
            {
                uiOutput.Text = value;
            });
        }

        public void SetState(string value)
        {
            pwither.thrd.Locker.Lock locker = new pwither.thrd.Locker.Lock();
            using (locker.Write())
                if (IsAfk)
                {
                    IsAfk = false;
                    CheckAfk(CancellationToken);
                }
            this.Invoke(() =>
            {
                uiState.Text = value;
            });
        }

        public async void SetComplete(string id, int count, string target)
        {
            await Task.Run(async () =>
            {
                try
                {
                    SetOutput($"Start downloading {id} on {count} parts");
                    List<byte> parts = new List<byte>();
                    for (int i = 0; i < count;)
                    {
                        SetOutput($"Downloading {100 * (i + 1) / count}%");
                        var part = await ClientProviding.GetFilePartAsync(target, id, i);
                        if (part == null)
                            continue;
                        foreach(var @byte in part.Part)
                            parts.Add(@byte);
                        i++;
                    }
                    var node = Node.Unpack(parts.ToArray(), new SocketInitializeParameters { AuthType = wshell.Enums.SocketAuthType.Aes, UseEncryption = false });
                    var template = (IStatisticTemplate)node.Child;
                    SetOutput($"Comlete");
                    IsComplited = true;
                    IsAfk = true;
                    Check = true;
                    TaskController.AddTemplate(template);
                    this.Invoke(() =>
                    {
                        uiLed.Fill = _inComplete;
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke(() => { Logger.Log(new LogContent(ex.ToString(), this, LogType.CRITICAL, ex)); });
                }
            });
        }
    }
}
