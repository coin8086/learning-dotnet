using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISample
{
    interface IServiceB
    {
        void Say();
    }

    class ServiceB : IServiceB
    {
        IServiceA _sa;

        public ServiceB(IServiceA serviceA)
        {
            _sa = serviceA;
        }

        public void Say()
        {
            Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
            _sa.Speak();
        }
    }
}
