using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using infosystems.task.shellv1.Controls;
using infosystems.task.shellv1.Enums;
using infosystems.task.shellv1.Objects;
using infosystems.task.shellv1.Pages;
using infosystems.task.shellv1.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wcheck.Statistic.Items;
using wcheck.Statistic.Templates;
using wshell.Enums;

namespace infosystems.task.shellv1
{
    internal static class TaskController
    {
        public static Group DefaultGroup = new Group("default");
        private static Dictionary<string, GroupControl> _groups = new Dictionary<string, GroupControl>();
        private static List<Host> _hosts = new List<Host>();
        private static List<HostTaskControl> _tasks = new List<HostTaskControl>();
        private static List<IStatisticTemplate> _hostsTemplates = new List<IStatisticTemplate>();
        internal static IStatisticTemplate? ScapTemplate { get; set; }
        internal static IStatisticTemplate? NmapTemplate { get; set; }

        public static void AddTemplate(IStatisticTemplate template)
        {
                _hostsTemplates.Add(template);
        }

        public static IStatisticTemplate[] GetHostTemplates()
        {
            return _hostsTemplates.ToArray();
        }

        public static TaskPage TaskPage { get; internal set; }
        public static int HostsCount => _hosts.Count;

        public static ISType ISType { get; set; } = ISType.GIS;

        public static void AddGroup(GroupControl control)
        {
            _groups.Add(control.Group.Name, control);
        }

        public static string AddHostTask(HostTaskControl hostTaskControl)
        {
            _tasks.Add(hostTaskControl);
            return hostTaskControl.TaskId;
        }

        public static HostTaskControl GetHostTask(string taskId)
        {
            return _tasks.FirstOrDefault(x => x.TaskId == taskId);
        }

        public static void ActivateHostTasks()
        {
            foreach(var h in _tasks)
            {
                h.StartTask();
            }
        }

        public static bool ExistGroup(string name)
        {
            if(name.ToLower() == DefaultGroup.Name) return true;
            return _groups.ContainsKey(name);
        }

        public static void EditGroup(GroupControl control)
        {
            var name = control.Group.Name;
            var window = new AddNewGroupWindow(control.Group);
            if (window.ShowDialog() == true)
            {
                _groups.Remove(name);
                control.uiTextBlock.Text = control.Group.Name;
                AddGroup(control);
            }
        }
        public static void DeleteGroup(GroupControl control)
        {
            var name = control.Group.Name;
            control.Group.DeleteObject();
            _groups.Remove(name);
        }

        public static void EditHost(HostControl control)
        {
            var window = new AddNewHostWindow(control.Host);
            if (window.ShowDialog() == true)
            {
                control.uiTextBlock.Text = $"{control.Host.TargetIp} [{control.Host.Group.Name}]";
            }
        }

        public static void DeleteHost(HostControl control)
        {
            control.Host.DeleteObject();
            _hosts.Remove(control.Host);
        }
        public static void AddHost(HostControl control)
        {
            _hosts.Add(control.Host);
        }

        public static bool ExistHost(string targetIp)
        {
            var obj = _hosts.FirstOrDefault(x => x.TargetIp == targetIp);
            if (obj == null) return false;
            return true;
        }
        public static Host GetHost(string targetIp)
        {
            var obj = _hosts.FirstOrDefault(x => x.TargetIp == targetIp);
            return obj;
        }

        public static Group[] GetGroups()
        {
            var result = new List<Group>();
            foreach(var group in _groups)
            {
                result.Add(group.Value.Group);
            }
            return result.ToArray();
        }
        public static Host[] GetHosts()
        {
            return _hosts.ToArray();
        }

        public static ExportObject GetExportObject()
        {
            var hosts = GetHosts();
            var groups = GetGroups();
            List<ExportHost> he = new List<ExportHost>();
            List<ExportGroup> ge = new List<ExportGroup>();
            foreach (var group in groups)
                ge.Add(new ExportGroup
                {
                    Name = group.Name,
                });
            foreach (var host in hosts)
            {
                var g = ge.FirstOrDefault(x => x.Name == host.Group.Name);
                if(g == null)
                    he.Add(new ExportHost
                    {
                        TargetIp = host.TargetIp,
                        Group = new ExportGroup
                        {
                            Name = TaskController.DefaultGroup.Name
                        }
                    });
                else
                    he.Add(new ExportHost
                    {
                        TargetIp = host.TargetIp,
                        Group = g
                    });
            }
            return new ExportObject(he, ge, ISType);
        }

        internal static void Clear()
        {
            ISType = ISType.GIS;
            _groups.Clear();
            _hosts.Clear();
            _hostsTemplates.Clear();
            ScapTemplate = null;
            NmapTemplate = null;
            _hosts.Clear();
            TaskPage = null;
        }
    }
}
