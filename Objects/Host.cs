using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Objects
{
    public class Host
    {
        public event EventHandler Delete;
        public event EventHandler DeleteGroup;
        public string TargetIp { get; set; }
        private Group _group;
        public Group Group { 
            get => _group; 
            set
            {
                _group = value;
                _group.Delete += (sender, e) =>
                {
                    DeleteGroup?.Invoke(this, e);
                };
            }
        }
        public Host(string targetIp, Group group) 
        {
            TargetIp = targetIp;
            Group = group;
        }
        internal void DeleteObject()
        {
            Delete?.Invoke(this, null);
        }
    }
}
