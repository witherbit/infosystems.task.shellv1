using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infosystems.task.shellv1.Forms.Abstract
{
    public interface ITest
    {
        IEnumerable<TestElement> TestElements { get; }
        string GetResult();
    }
}
