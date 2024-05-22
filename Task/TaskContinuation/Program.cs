namespace TaskContinuation;

class Program
{
    static Task<int> IncrementAsync(int x)
    {
        return Task.Run(() =>
        {
            var ret = x + 1;
            Console.WriteLine(ret);
            return ret;
        });
    }

#pragma warning disable CS0162
    static async Task<int> ErrorAsync(int x)
    {
        await Task.Delay(100);
        throw new ApplicationException("Some error");
        return x + 1;
    }

    static async Task<int> CancelAsync(int x)
    {
        await Task.Delay(100);
        throw new OperationCanceledException("Canceled somehow");
        return x + 1;
    }
#pragma warning restore CS0162

    static void CheckTask<T>(Task<T> task)
    {
        Console.WriteLine($"===== Task {task.Id} ===== ");
        Console.WriteLine($"Status={task.Status}");

        if (task.IsCompletedSuccessfully)
        {
            Console.WriteLine($"Result={task.Result}");
        }
        else if (task.Exception != null)
        {
            Console.WriteLine($"Exception={task.Exception}");
        }
    }

    static void CheckTask(Task task)
    {
        Console.WriteLine($"===== Task {task.Id} ===== ");
        Console.WriteLine($"Status={task.Status}");

        if (task.Exception != null)
        {
            Console.WriteLine($"Exception={task.Exception}");
        }
    }

    static void Main(string[] args)
    {
        //Continue and unwrap
        IncrementAsync(0).ContinueWith(t => IncrementAsync(t.Result))
            .Unwrap().ContinueWith(t => IncrementAsync(t.Result)).Wait();

        //Antecedent is faulted
        ErrorAsync(0).ContinueWith(t => CheckTask(t)).Wait();

        //Antecedent is canceled
        CancelAsync(0).ContinueWith(t => CheckTask(t)).Wait();

        //Continue only when not faulted
        ErrorAsync(0).ContinueWith(t =>
        {
            Console.WriteLine("This line should not be executed.");
        }, TaskContinuationOptions.NotOnFaulted).ContinueWith(t => CheckTask(t)).Wait();
    }
}
