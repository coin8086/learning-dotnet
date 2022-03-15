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

        IServiceX<ServiceB> _sx;

        public ServiceB(IServiceA serviceA, IServiceX<ServiceB> sx)
        {
            _sa = serviceA;
            _sx = sx;
        }

        public void Say()
        {
            Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
            _sa.Speak();
            _sx.Log();
        }
    }
}
