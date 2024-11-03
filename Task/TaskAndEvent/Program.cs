using System;

namespace TaskAndEvent
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    class Publisher
    {
        public event EventHandler<CustomEventArgs>? RaiseCustomEvent;

        public void FireEvent()
        {
            EventHandler<CustomEventArgs>? raiseEvent = RaiseCustomEvent;
            if (raiseEvent != null)
            {
                var ts = DateTime.Now;
                Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: Fire event at {ts}.");

                var e = new CustomEventArgs($"Event triggered at {ts}");
                raiseEvent(this, e);
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

        void HandleCustomEvent(object? sender, CustomEventArgs e)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: {_id} received message: at {DateTime.Now}\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
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
        async void HandleCustomEvent(object? sender, CustomEventArgs e)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: + {_id} received message: at {DateTime.Now}\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
            await Task.Delay(2000);
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}]: - {_id} continued at {DateTime.Now}\nSynchronizationContext.Current = {SynchronizationContext.Current}\nTaskScheduler.Current = {TaskScheduler.Current}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine($"Main thread id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"SynchronizationContext.Current = {SynchronizationContext.Current}");
            Console.WriteLine($"TaskScheduler.Current = {TaskScheduler.Current}");

            var pub = new Publisher();
            var sub1 = new Subscriber("sub1", pub);
            var sub2 = new Subscriber("sub2", pub);
            var sub3 = new AsyncSubscriber("sub3", pub);
            var sub4 = new AsyncSubscriber("sub4", pub);
            Subscriber sub5;
            Task.Run(() => sub5 = new Subscriber("sub5", pub));
            AsyncSubscriber sub6;
            Task.Run(() => sub6 = new AsyncSubscriber("sub6", pub));

            pub.FireEvent();

            // Keep the console window open
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
