using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseAsync.Observer
{
    public delegate double PriceHandler();

    public class PriceManager
    {
        private PriceHandler GetPriceHandler;

        //委托处理，当价格高于100元按8.8折计算，其他按原价计算
        public double GetPrice()
        {
            if (GetPriceHandler != null)
            {
                if (GetPriceHandler() > 100)
                    return GetPriceHandler() * 0.88;
                else
                    return GetPriceHandler();
            }
            return -1;
        }

        public void AddHandler(PriceHandler handler)
        {
            GetPriceHandler += handler;
        }

        public void RemoveHandler(PriceHandler handler)
        {
            GetPriceHandler -= handler;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PriceManager priceManager = new PriceManager();

            //调用priceManager的GetPrice方法获取价格
            //直接调用委托的Invoke获取价格，两者进行比较
            priceManager.AddHandler(new PriceHandler(ComputerPrice));
            Console.WriteLine(string.Format("GetPrice\n  Computer's price is {0}!",
                priceManager.GetPrice()));

            Console.WriteLine();

            priceManager.AddHandler(new PriceHandler(BookPrice));
            Console.WriteLine(string.Format("GetPrice\n  Book's price is {0}!",
                priceManager.GetPrice()));

            Console.ReadKey();
        }
        /// <summary>
        /// 书本价格为98元
        /// </summary>
        /// <returns></returns>
        public static double BookPrice()
        {
            return 98.0;
        }
        /// <summary>
        /// 计算机价格为8800元
        /// </summary>
        /// <returns></returns>
        public static double ComputerPrice()
        {
            return 8800.0;
        }
    }
}
