using infosystems.task.shellv1.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wcheck;
using wcheck.wcontrols;
using wcheck.wshell.Enums;
using wcheck.wshell.Objects;
using wshell.Abstract;
using wshell.Enums;
using wshell.Net;
using wshell.Objects;

namespace infosystems.task.shellv1
{
    public class InfoSystemShell : ShellBase
    {
        public Frame Frame => Page.uiFrame;

        public NetworkingConnectionType NetworkingConnectionType { get; private set; }

        public readonly string DefaultConnectiveShells =
             "ee3de59e-00e8-40e9-bfcd-cba116b9a81d\r\n" +
             "4f1fc6ba-56b0-4844-8e2a-485578e8bc1f";

        public readonly string ClientConnectiveShells =
             "ee3de59e-00e8-40e9-bfcd-cba116b9a81d";

        public MainPage Page { get; private set; }
        public ShellInfo[] ShellInfos { get; private set; }
        internal List<ShellBase> Shells { get; private set; }

        public InfoSystemShell() : base(new ShellInfo("Тестирование ИС", "1.0.0", new Guid("b4877dc5-a5b5-4b7e-b08b-1b1995e8c8d8"), ShellType.Task, "Задача определения соответствия ИСПДн заданному уроню защищенности", "Артем И.С."))
        {
            Settings = ShellSettings.Load(ShellInfo.Id.ToString(), new List<SettingsObject>
            {
            });
        }

        public override Schema OnHostCallback(Schema schema)
        {
            switch (schema.Type)
            {
                case CallbackType.ShellPropertyChanged:
                    if (HostsPage.Instance != null)
                        HostsPage.Instance.OnShellsPropertyChanged();
                    return new Schema(CallbackType.EmptyResponse);
                case CallbackType.CustomRequest:

                    if(schema.GetProvidingType() == typeof(string))
                        if (schema.GetProviding<string>() == "type.gethosts")
                        {
                            var hosts = "";
                            foreach(var host in TaskController.GetHosts())
                            {
                                hosts += $"{host.TargetIp},";
                            }
                            return new Schema(CallbackType.CustomResponse).SetProviding(hosts);
                        }
                        else if (schema.GetProviding<string>() == "type.gethostswithgroups")
                        {
                        var hosts = "";
                        foreach (var host in TaskController.GetHosts())
                        {
                            hosts += $"{host.TargetIp}:{host.Group.Name},";
                        }
                        return new Schema(CallbackType.CustomResponse).SetProviding(hosts);
                        }

                    return new Schema(CallbackType.EmptyResponse);
                default:
                    return new Schema(CallbackType.EmptyResponse);
            }
        }

        public override void OnPause()
        {
        }

        public override void OnResume()
        {

        }

        public override void OnRun()
        {
            //get parameters
            NetworkingConnectionType = (NetworkingConnectionType)Callback.InvokeRequest(this, new Schema(CallbackType.SettingsParameterRequest).SetProviding(SettingsParamConsts.ParameterConnection.p_Type))
                .GetProviding<SettingsObject>().Value;

            //sharing shell infos
            var shellInfosResponse = Callback.InvokeRequest(this, new Schema(CallbackType.ShellInfosRequest).SetProviding(ShellInfo));
            if (shellInfosResponse.Type == CallbackType.ShellInfosResponse)
            {
                ShellInfos = shellInfosResponse.GetProviding<ShellInfo[]>();
            }

            //open taskview shells
            Shells = new List<ShellBase>();

            foreach (var shl in ShellInfos)
            {
                var acceptedShells = NetworkingConnectionType == NetworkingConnectionType.Server ? DefaultConnectiveShells : ClientConnectiveShells;
                if (acceptedShells.Contains(shl.Id.ToString()))
                {
                    var tViewShell = Callback.InvokeRequest(this, new Schema(CallbackType.GetShellInstanceRequest).SetProviding(shl)).GetProviding<ShellBase>();
                    Shells.Add(tViewShell);
                }
            }

            //registering page
            Page = new MainPage(this);
            Callback.Invoke(this, new Schema(CallbackType.RegisterPage).SetProviding(Page));
        }

        public override void OnStop()
        {
            foreach (var shl in Shells)
                shl.Stop();
            Shells.Clear();
            Shells = null;
            TaskController.Clear();
            ShellInfos = Array.Empty<ShellInfo>();
            Callback.Invoke(this, new Schema(CallbackType.UnregisterPage).SetProviding(Page));
            Page = null;
        }

        public void Reload()
        {
            var response = Callback.InvokeRequest(this, new Schema(CallbackType.ReloadPageRequest).SetProviding(new MainPage(this)));
            Page = response.GetProviding<MainPage>();
            Run();
        }

        public override void OnSettingsEdit(SettingsObject obj, PropertyEventArgs propertyEventArgs)
        {
        }
    }
}
