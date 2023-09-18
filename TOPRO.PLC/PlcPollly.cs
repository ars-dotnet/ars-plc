using TOPRO.HSL;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.PLC
{
    public class PlcPollly
    {
        public static T PlcRetry<T>(Func<T> func)
            where T : OperateResult
        {
            return Policy<T>
                .HandleResult(r => !r.IsSuccess)
                .WaitAndRetry(new TimeSpan[] { TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(1000) })
                .Execute(func);
        }

        public static T ExceptionRetry<T>(Func<T> func)
        {
            return Policy<T>
                .Handle<Exception>()
                .WaitAndRetry(new TimeSpan[] { TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(1000) })
                .Execute(func);
        }

        public static void ExceptionRetry(Action func)
        {
            Policy
               .Handle<Exception>()
               .WaitAndRetry(new TimeSpan[] { TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(1000) })
               .Execute(func);
        }
    }
}
