using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using infosystems.task.shellv1.Enums;
using pwither.formatter;

namespace infosystems.task.shellv1.Objects
{
    [BitSerializable]
    internal class ExportObject
    {
        public IEnumerable<ExportHost> Hosts { get; }
        public IEnumerable<ExportGroup> Groups { get; }
        public ISType ISType { get; }
        public ExportObject(IEnumerable<ExportHost> hosts, IEnumerable<ExportGroup> groups, ISType type)
        {
            Hosts = hosts;
            Groups = groups;
            ISType = type;
        }
    }
    [BitSerializable]
    internal class ExportHost
    {
        public string TargetIp { get; set; }
        public ExportGroup Group { get; set; }
    }
    [BitSerializable]
    internal class ExportGroup
    {
        public string Name { get; set; }
    }
}
