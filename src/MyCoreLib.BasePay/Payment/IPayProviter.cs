using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCoreLib.BasePay.Payment
{
    public interface IPayProviter
    {
        PayConfig Config { get; }

        void Pay();
        Task<T> PayAsync<T>();

        void PayDone();
        Task<T> PayDoneAsync<T>();
    }
}
