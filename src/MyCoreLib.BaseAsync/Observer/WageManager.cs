using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseAsync.Observer
{
    public class WageManager
    {
        public delegate double WageHandler(double basicWages);

        private WageHandler wageHandler;

        /// <summary>
        /// 加入观察者
        /// </summary>
        /// <param name="wageHandler1"></param>
        public void Attach(WageHandler wageHandler1)
        {
            wageHandler += wageHandler1;
        }

        /// <summary>
        /// 删除观察者
        /// </summary>
        /// <param name="wageHandler1"></param>
        public void Detach(WageHandler wageHandler1)
        {
            wageHandler -= wageHandler1;
        }

        /// <summary>
        /// 通过GetInvodationList方法获取多路广播委托列表，
        /// 如果观察者数量大于0即执行方法
        /// </summary>
        /// <param name="basicWages"></param>
        public void Execute(double basicWages)
        {
            if (wageHandler != null)
                if (wageHandler.GetInvocationList().Length > 0)
                    wageHandler(basicWages);
        }

        public delegate void WageHandler<T>(T obj);

        public static void GetWorkerWages(Worker worker)
        {
            Console.WriteLine("Worker's total wages is " + worker.Wages);
        }

        public static void GetManagerWages(Manager manager)
        {
            Console.WriteLine("Manager's total wages is " + manager.Wages);
        }

        static void Main(string[] args)
        {
            WageManager wageManager = new WageManager();
            //加入Manager观察者
            Manager manager = new Manager();
            WageHandler managerHandler = new WageHandler(manager.GetWages);
            wageManager.Attach(managerHandler);

            //加入Assistant观察者
            Assistant assistant = new Assistant();
            WageHandler assistantHandler = new WageHandler(assistant.GetWages);
            wageManager.Attach(assistantHandler);

            //同时加入底薪3000元，分别进行计算
            wageManager.Execute(3000);

            WageHandler<Worker> workerHander = new WageHandler<Worker>(GetWorkerWages);
            assistant = new Assistant();
            assistant.Wages = 3000;
            workerHander(assistant);

            WageHandler<Manager> mHandler = new WageHandler<Manager>(GetManagerWages);
            manager = new Manager();
            manager.Wages = 4500;
            mHandler(manager);

            Console.ReadKey();
        }
    }
}
