using MyCoreLib.BaseIoC;
using System;

namespace MyCoreLib.BaseIoCTest
{
    public interface ITest
    {
        void DoWork();
    }

    public class Test : ITest
    {
        public void DoWork()
        {
            Console.WriteLine("do work!");
        }
    }

    public class Test2 : ITest
    {
        public void DoWork()
        {
            Console.WriteLine("do work2!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IIoCConfig config = IoCConfig.ReadConfig("");
            config.AddConfig<Test>("MyCoreLib.BaseIoCTest.Test");//添加配置
            config.AddConfig<Test2>("MyCoreLib.BaseIoCTest.Test2");//添加配置
            //获取容器
            IIoCContainer container = IoCContainerManager.GetIoCContainer(config);
            //根据ITest接口去获取对应的实例
            ITest test = container.Get<ITest>("MyCoreLib.BaseIoCTest.Test");
            ITest test2 = container.Get<ITest>("MyCoreLib.BaseIoCTest.Test2");

            test.DoWork();
            test2.DoWork();

            Console.Read();
        }
    }
}
