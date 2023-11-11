﻿using System;

namespace EventAndThread
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
            OnRaiseCustomEvent(new CustomEventArgs("Event triggered"));
        }

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs>? raiseEvent = RaiseCustomEvent;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
            {
                Console.WriteLine($"Event fired at thread id: {Thread.CurrentThread.ManagedThreadId}");

                e.Message += $" at {DateTime.Now}";
                raiseEvent(this, e);
            }
        }
    }

    //Class that subscribes to an event
    class Subscriber
    {
        private readonly string _id;

        public Subscriber(string id, Publisher pub)
        {
            _id = id;
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        // Define what actions to take when the event is raised.
        void HandleCustomEvent(object? sender, CustomEventArgs e)
        {
            Console.WriteLine($"Event handled at thread id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"{_id} received this message: {e.Message}");
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
            Console.WriteLine($"+ {_id} received this message: {e.Message} in thread {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine($"- {_id} received this message: {e.Message} in thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine($"Main thread id: {Thread.CurrentThread.ManagedThreadId}");

            var pub = new Publisher();
            var sub1 = new Subscriber("sub1", pub);
            var sub2 = new Subscriber("sub2", pub);
            var sub3 = new AsyncSubscriber("sub3", pub);
            var sub4 = new AsyncSubscriber("sub4", pub);

            pub.FireEvent();

            // Keep the console window open
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}