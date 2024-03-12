using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSample
{
    class Program
    {
        public class CustomException : Exception
        {
            public CustomException(String message) : base(message)
            { }
        }

        static void HandleException()
        {
            var task1 = Task.Run(() => { throw new CustomException("This exception is expected!"); });

            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    // Handle the custom exception.
                    if (e is CustomException)
                    {
                        Console.WriteLine(e.Message);
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        static void HandleAggregateException()
        {
            try
            {
                List<Task> tasks = new List<Task>();

                tasks.Add(Task.Run(() => {
                    throw new UnauthorizedAccessException();
                }));

                tasks.Add(Task.Run(() => {
                    throw new ArgumentException("The system root is not a valid path.");
                }));

                tasks.Add(Task.Run(() => {
                    throw new NotImplementedException("This operation has not been implemented.");
                }));

                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException ae)
                {
                    //NOTE: This (Flatten) doesn't seem any difference.
                    //throw ae.Flatten();
                    throw;
                }
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:\n   {1}", e.GetType().Name, e.Message);
                }
            }
        }

        static void HandleExceptionInContinuation()
        {
            var task = Task.Run(() => { throw new ApplicationException("Something went wrong!"); });
            //var task = Task.Run(() => { });

            task.ContinueWith((t) =>
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Task Status: {t.Status}");

                if (t.Status == TaskStatus.Faulted)
                {
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Exception:");
                    Console.WriteLine(t.Exception);

                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Exception.InnerExceptions:");
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        Console.WriteLine("--------------------------------------");
                        Console.WriteLine(e);
                    }
                }
                else if (t.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine($"OK");
                }
            }).Wait();
            //Try this
            //}, TaskContinuationOptions.OnlyOnRanToCompletion).Wait();
        }

        static void HandleExceptionInContinuation2()
        {
            var task = Task.Run(() => { throw new ApplicationException("Something went wrong!"); });
            Task task2 = null;
            try
            {
                task2 = task.ContinueWith((t) => { Console.WriteLine($"OK"); },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task2.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex);
                Console.WriteLine($"Task Status: {task2?.Status}");
                Console.WriteLine($"Task Exception: {task2?.Exception}");
            }
        }

        static void WaitAndCheckException<T>(T task) 
            where T : Task
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"AggregateException:\n{ae}");
            }
            Console.WriteLine($"Task Status: {task.Status}");
            Console.WriteLine($"Task Exception: {task.Exception}");
        }

        static void HandleExceptionInContinuation3()
        {
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.Cancel();

            var task = Task.FromCanceled(token);
            Task continuation =
                task.ContinueWith(
                    antecedent => Console.WriteLine("The continuation is running."),
                    TaskContinuationOptions.NotOnCanceled);

            try
            {
                task.Wait();
                //Or this:
                //continuation.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                Console.WriteLine();
            }

            Console.WriteLine($"Task {task.Id}: {task.Status:G}");
            Console.WriteLine($"Task {continuation.Id}: {continuation.Status:G}");
        }

        static void AttachToParent()
        {
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task beginning.");
                for (int ctr = 0; ctr < 10; ctr++)
                {
                    Task.Factory.StartNew((x) => {
                        Thread.SpinWait(5000000);
                        Console.WriteLine("Attached child #{0} completed.", x);
                    },
                    ctr, TaskCreationOptions.AttachedToParent);
                }
            });

            parent.Wait();
            Console.WriteLine("Parent task completed.");
        }

        static void CallAction(Action callback)
        {
            Console.Error.WriteLine("CallAction Begin");
            callback();
            Console.Error.WriteLine("CallAction End");
        }

        async static Task CallActionAsync(Func<Task> callback)
        {
            Console.Error.WriteLine("CallActionAsync Begin");
            await callback();
            Console.Error.WriteLine("CallActionAsync End");
        }

        static void AsyncCallback()
        {
            Action action = async () =>
            {
                Console.Error.WriteLine("Async Action Begin");
                await Task.Delay(5000);
                Console.Error.WriteLine("Async Action End");
            };
            CallAction(action);

            Console.Error.WriteLine("-------------------------------");

            Func<Task> action2 = async () =>
            {
                Console.Error.WriteLine("Async Action 2 Begin");
                await Task.Delay(3000);
                Console.Error.WriteLine("Async Action 2 End");
            };
            CallActionAsync(action2).Wait();
        }

        static void CaptureVariableInLoop()
        {
            var tasks = new Task[3];
            var strs = new string[3] { "I'm", "OK", "Thanks!" };
            for (int i = 0; i < 3; i++)
            {
                var j = i;
                var x = strs[i];
                tasks[i] = Task.Run(() =>
                {
                    Console.WriteLine($"Before {i}, {j}, {x}");
                    //await Task.Delay(100);
                    //Console.WriteLine($"After {i}, {j}, {x}");
                });
            }
            Task.WaitAll(tasks);
        }

        static void Main(string[] args)
        {
            //AttachToParent();
            //HandleException();
            //HandleAggregateException();
            //HandleExceptionInContinuation();
            //HandleExceptionInContinuation2();
            //HandleExceptionInContinuation3();
            //AsyncCallback();
            CaptureVariableInLoop();
        }
    }
}
