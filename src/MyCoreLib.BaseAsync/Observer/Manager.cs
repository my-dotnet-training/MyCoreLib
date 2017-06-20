using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseAsync.Observer
{
    public class Manager:Worker
    {
        public override double GetWages(double basicWages)
        {
            double totalWages = 1.5 * basicWages;
            Console.WriteLine("Manager's wages is : " + totalWages);
            return totalWages;
        }
    }
}
