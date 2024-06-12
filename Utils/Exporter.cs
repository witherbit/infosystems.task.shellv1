using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using infosystems.task.shellv1.Enums;
using infosystems.task.shellv1.Objects;
using pwither.formatter;


namespace infosystems.task.shellv1.Utils
{
    internal static class Exporter
    {
        public static void Export(this ExportObject obj, string filename)
        {
            var arr = BitSerializer.SerializeNative(obj,
                typeof(ExportHost),
                typeof(ExportGroup),
                typeof(ExportObject),
                typeof(IEnumerable<ExportHost>),
                typeof(IEnumerable<ExportGroup>),
                typeof(ISType));
            File.WriteAllBytes(filename, arr);
        }
        public static ExportObject Import(string filename)
        {
            var arr = File.ReadAllBytes(filename);
            return BitSerializer.DeserializeNative<ExportObject>(arr,
                typeof(ExportHost),
                typeof(ExportGroup),
                typeof(ExportObject),
                typeof(IEnumerable<ExportHost>),
                typeof(IEnumerable<ExportGroup>),
                typeof(ISType));
        }
    }
}
