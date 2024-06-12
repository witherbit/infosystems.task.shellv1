using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Objects
{
    public class Group
    {
        public event EventHandler Delete;
        public string Name { get; set; }
        public Group(string name)
        {
            Name = name;
        }

        internal void DeleteObject()
        {
            Delete?.Invoke(this, null);
        }
    }
}
