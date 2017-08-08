using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseAsync.EAP
{
    public class ObserverDemo
    {
        public delegate double Handler(double basicWages);

        public class Manager
        {
            public double GetWages(double basicWages)
            {
                double totalWages = 1.5 * basicWages;
                Console.WriteLine("Manager's wages is : " + totalWages);
                return totalWages;
            }
        }

        public class Assistant
        {
            public double GetWages(double basicWages)
            {
                double totalWages = 1.2 * basicWages;
                Console.WriteLine("Assistant's wages is : " + totalWages);
                return totalWages;
            }
        }

        public class WageManager
        {
            private Handler wageHandler;

            //加入观察者
            public void Attach(Handler wageHandler1)
            {
                wageHandler += wageHandler1;
            }

            //删除观察者
            public void Detach(Handler wageHandler1)
            {
                wageHandler -= wageHandler1;
            }

            //通过GetInvodationList方法获取多路广播委托列表，如果观察者数量大于0即执行方法
            public void Execute(double basicWages)
            {
                if (wageHandler != null)
                    if (wageHandler.GetInvocationList().Length != 0)
                        wageHandler(basicWages);
            }

            static void Main(string[] args)
            {
                WageManager wageManager = new WageManager();
                //加入Manager观察者
                Manager manager = new Manager();
                Handler managerHandler = new Handler(manager.GetWages);
                wageManager.Attach(managerHandler);

                //加入Assistant观察者
                Assistant assistant = new Assistant();
                Handler assistantHandler = new Handler(assistant.GetWages);
                wageManager.Attach(assistantHandler);

                //同时加入底薪3000元，分别进行计算
                wageManager.Execute(3000);
                Console.ReadKey();
            }
        }
    }
}
