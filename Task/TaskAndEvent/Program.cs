namespace TaskAndEvent
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string message, DateTime ts)
        {
            Message = message;
            Timestamp = ts;
        }

        public string Message { get; }

        public DateTime Timestamp { get; }

        public override string ToString()
        {
            return $"{{ Message: \"{Message}\", Timestamp: \"{Timestamp}\" }}";
        }
    }

    class Publisher
    {
        public event EventHandler<CustomEventArgs>? RaiseCustomEvent;

        public void FireEvent(string msg)
        {
            EventHandler<CustomEventArgs>? raiseEvent = RaiseCustomEvent;
            if (raiseEvent != null)
            {
                var ts = DateTime.Now;
                var evt = new CustomEventArgs(msg, ts);

                Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId,2}]: Fire event {evt}");

                raiseEvent(this, evt);
            }
        }
    }

    class Subscriber
    {
        private readonly string _id;

        public Subscriber(string id, Publisher pub)
        {
            _id = id;
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        void HandleCustomEvent(object? sender, CustomEventArgs evt)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId,2}][{_id}]: Received event {evt}");
        }
    }

    class AsyncSubscriber
    {
        private readonly string _id;

        public AsyncSubscriber(string id, Publisher pub)
        {
            _id = id;
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        //NOTE the async modifier, compare this handler with the RaiseCustomEvent type and how that event is triggered.
        async void HandleCustomEvent(object? sender, CustomEventArgs evt)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId,2}][{_id}]: + Received event {evt}");

            await Task.Delay(2000);

            //Try this
            //Task.Delay(2000).Wait();

            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId,2}][{_id}]: - Continued event {evt} at {DateTime.Now}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine($"Main thread id: {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("------------");

            var pub = new Publisher();
            var sub1 = new Subscriber("sub1", pub);
            var sub2 = new Subscriber("sub2", pub);
            var sub3 = new AsyncSubscriber("sub3", pub);
            var sub4 = new AsyncSubscriber("sub4", pub);
            Task.Run(async () =>
            {
                var sub5 = new Subscriber("sub5", pub);
                await Task.Delay(5000);
            });
            Task.Run(async () =>
            {
                var sub6 = new AsyncSubscriber("sub6", pub);
                await Task.Delay(5000);
            });

            pub.FireEvent("event 1");

            Console.WriteLine("------------");

            pub.FireEvent("event 2");

            // Keep the console window open
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(false);
        }
    }
}
