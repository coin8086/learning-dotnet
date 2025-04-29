using System;

namespace AppDomainEventsDep
{
    public static class Hello
    {
        public static void Say()
        {
            Console.WriteLine($"Hello from {nameof(AppDomainEventsDep)}!");
        }
    }
}
