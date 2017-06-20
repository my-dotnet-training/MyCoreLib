using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.​Threading.​Tasks;

namespace MyCoreLib.BaseAsync.TAP
{
    /// <summary>
    /// 并行同时访问全局变量，会出现资源争夺，大多数时间消耗在了资源等待上面
    /// 对局部变量可使用Parallel.For优化性能
    /// .netstandard2.0
    /// </summary>
    public class ParallelManager
    {
        public void SpinLockDemo()
        {
            SpinLock slock = new SpinLock(false);
            long sum1 = 0;
            long sum2 = 0;
            //Parallel.For(0, 100000, i =>
            //{
            //    sum1 += i;
            //});

            //Parallel.For(0, 100000, i =>
            //{
            //    bool lockTaken = false;
            //    try
            //    {
            //        slock.Enter(ref lockTaken);
            //        sum2 += i;
            //    }
            //    finally
            //    {
            //        if (lockTaken)
            //            slock.Exit(false);
            //    }
            //});

            Console.WriteLine("Num1的值为:{0}", sum1);
            Console.WriteLine("Num2的值为:{0}", sum2);

            Console.Read();
        }
    }
}
