using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseAsync.Observer
{
    public class Assistant: Worker
    {
        public override double GetWages(double basicWages)
        {
            double totalWages = 1.2 * basicWages;
            Console.WriteLine("Assistant's wages is : " + totalWages);
            return totalWages;
        }
    }
}
