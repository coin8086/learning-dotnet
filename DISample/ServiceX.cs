using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISample
{
    interface IServiceX<T>
    {
        void Log();
    }

    class ServiceX<T> : IServiceX<T>
    {
        public void Log()
        {
            Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
        }
    }
}
