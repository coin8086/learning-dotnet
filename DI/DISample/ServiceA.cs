using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISample
{
    interface IServiceA
    {
        void Speak();
    }

    class ServiceA : IServiceA
    {
        public void Speak()
        {
            Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
        }
    }
}
