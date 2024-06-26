﻿using pwither.ev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wcheck.Statistic.Templates;
using wshell.Abstract;
using wshell.Objects;

namespace infosystems.task.shellv1.Objects
{
    public class ShellOutputRedirect : EventRedirect
    {
        internal event Action<string> Output;
        internal event Action<string> State;
        internal event Action<IStatisticTemplate> Complete;
        public ShellOutputRedirect(ShellBase shellBase) : base(shellBase)
        {
        }

        [LocalEvent("output")]
        public void OutputInvoke(string output)
        {
            Output?.Invoke(output);
        }
        [LocalEvent("state")]
        public void StateInvoke(string state)
        {
            State?.Invoke(state);
        }
        [LocalEvent("complete")]
        public void CompleteInvoke(IStatisticTemplate template)
        {
            Complete?.Invoke(template);
        }
    }
}
